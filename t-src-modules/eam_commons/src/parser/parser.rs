use roxmltree::{Document, Node};
use std::collections::HashMap;

use crate::models::{Account, CharListDataset, Character, ClassStats, ParsedItem};

// ─────────────────────────────────────────────────────────────────────────────
// Public API
// ─────────────────────────────────────────────────────────────────────────────

#[derive(Debug, thiserror::Error)]
pub enum ParseError {
    #[error("invalid xml: {0}")]
    Xml(String),
    #[error("missing node: {0}")]
    Missing(&'static str),
}

pub fn parse_char_list(
    email: &str,
    xml: &str,
    entry_id: Option<&str>,
) -> Result<CharListDataset, ParseError> {
    let doc = Document::parse(xml).map_err(|e| ParseError::Xml(e.to_string()))?;

    // AccountId is used in a few places and handy to denorm into items
    let account_id = text_at_path(&doc, &["Chars", "Account", "AccountId"]).map(|s| s.to_string());

    // Build Account
    let account = build_account(&doc)?;

    // Class stats
    let class_stats = build_class_stats(&doc, account_id.as_deref());

    // Characters
    let characters = build_characters(&doc)?;

    // Unique maps for enchant resolution
    let mut uniq = UniqueResolver::from_doc(&doc)?;

    // Normalize items from all sources
    let items = collect_all_items(
        &doc,
        entry_id.map(|s| s.to_string()),
        account_id.clone(),
        &mut uniq,
    );

    Ok(CharListDataset {
        email: email.to_string(),
        account,
        class_stats,
        character: characters,
        items,
    })
}

// ─────────────────────────────────────────────────────────────────────────────
// Account mapping
// ─────────────────────────────────────────────────────────────────────────────

fn build_account(doc: &Document) -> Result<Account, ParseError> {
    let acc = find_path(doc, &["Chars", "Account"]).ok_or(ParseError::Missing("Account"))?;

    // Util that joins <Chest>...</Chest> into one comma string (like your FE)
    let format_chests = |base: Option<Node>| -> String {
        if let Some(base) = base {
            let parts: Vec<String> = base
                .children()
                .filter(|n| n.has_tag_name("Chest"))
                .filter_map(|c| c.text())
                .map(|s| normalize_csv_str(s))
                .filter(|s| !s.is_empty())
                .collect();
            parts.join(",")
        } else {
            String::new()
        }
    };

    let acc_id = text_of(acc, "AccountId");
    let stats = acc.children().find(|n| n.has_tag_name("Stats"));

    Ok(Account {
        entry_id: None,
        account_id: acc_id.map(|s| s.to_string()),
        credits: i32_of(acc, "Credits"),
        fortune_token: i32_of(acc, "FortuneToken"),
        unity_campaign_points: i32_of(acc, "UnityCampaignPoints"),
        early_game_event_tracker: i32_of(acc, "EarlyGameEventTracker"),
        creation_timestamp: text_of(acc, "CreationTimestamp").map(|s| s.to_string()),
        enchanter_support_dust: i32_of(acc, "EnchanterSupportDust").or(Some(0)),
        vault: Some(format_chests(find_path_under(acc, &["Vault"]))),
        material_storage: Some(format_chests(find_path_under(acc, &["MaterialStorage"]))),
        gifts: text_of(acc, "Gifts")
            .map(|s| normalize_csv_str(s))
            .or(Some(String::new())),
        temporary_gifts: text_of(acc, "TemporaryGifts")
            .map(|s| normalize_csv_str(s))
            .or(Some(String::new())),
        potions: text_of(acc, "Potions")
            .map(|s| normalize_csv_str(s))
            .or(Some(String::new())),
        max_num_chars: i32_of(acc, "MaxNumChars"),
        last_server: text_of(acc, "LastServer").map(|s| s.to_string()),
        originating: text_of(acc, "Originating").map(|s| s.to_string()),
        pet_yard_type: i32_of(acc, "PetYardType"),
        forge_fire_energy: i32_of(acc, "ForgeFireEnergy"),
        regular_forge_fire_blueprints: text_of(acc, "RegularForgeFireBlueprints")
            .map(|s| normalize_csv_str(s))
            .or(Some(String::new())),
        name: text_of(acc, "Name").map(|s| s.to_string()),
        best_char_fame: stats.and_then(|st| i32_of_node(st, "BestCharFame")),
        total_fame: stats.and_then(|st| i32_of_node(st, "TotalFame")),
        fame: stats.and_then(|st| i32_of_node(st, "Fame")),
        guild_name: acc
            .children()
            .find(|n| n.has_tag_name("Guild"))
            .and_then(|g| text_of(g, "Name"))
            .map(|s| s.to_string()),
        guild_rank: acc
            .children()
            .find(|n| n.has_tag_name("Guild"))
            .and_then(|g| i32_of_node(g, "Rank")),
        access_token_timestamp: text_of(acc, "AccessTokenTimestamp").map(|s| s.to_string()),
        access_token_expiration: text_of(acc, "AccessTokenExpiration").map(|s| s.to_string()),
        owned_skins: text_of(acc, "OwnedSkins")
            .map(|s| normalize_csv_str(s))
            .or(Some(String::new())),
    })
}

// ─────────────────────────────────────────────────────────────────────────────
// Class stats mapping
// ─────────────────────────────────────────────────────────────────────────────

fn build_class_stats(doc: &Document, account_id: Option<&str>) -> Vec<ClassStats> {
    let mut out = Vec::new();
    if let Some(stats_node) = find_path(doc, &["Chars", "Account", "Stats"]) {
        for cs in children_as_array(stats_node, "ClassStats") {
            // objectType is hex in the attribute ("0x300")
            let class_type = cs
                .attribute("objectType")
                .and_then(|hx| i32::from_str_radix(hx.trim_start_matches("0x"), 16).ok());

            let best_level = i32_of_node(cs, "BestLevel");
            // Your FE used BestFame; some payloads use BestBaseFame — support both:
            let best_base_fame =
                i32_of_node(cs, "BestFame").or_else(|| i32_of_node(cs, "BestBaseFame"));
            let best_total_fame = i32_of_node(cs, "BestTotalFame");

            out.push(ClassStats {
                id: None,
                entry_id: None,
                account_id: account_id.map(|s| s.to_string()),
                class_type,
                best_level,
                best_base_fame,
                best_total_fame,
            });
        }
    }
    out
}

// ─────────────────────────────────────────────────────────────────────────────
// Character mapping
// ─────────────────────────────────────────────────────────────────────────────

fn build_characters(doc: &Document) -> Result<Vec<Character>, ParseError> {
    let mut out = Vec::new();
    for c in doc.descendants().filter(|n| n.has_tag_name("Char")) {
        let char_id = c.attribute("id").and_then(|s| s.parse::<i32>().ok());

        let pet_node = c.children().find(|n| n.has_tag_name("Pet"));

        let (
            pet_name,
            pet_type,
            pet_instance_id,
            pet_rarity,
            pet_max_ability_power,
            pet_skin,
            pet_shader,
            pet_created_on,
            pet_inc_inv,
            pet_inv,
            pet_ability1_type,
            pet_ability1_power,
            pet_ability1_points,
            pet_ability2_type,
            pet_ability2_power,
            pet_ability2_points,
            pet_ability3_type,
            pet_ability3_power,
            pet_ability3_points,
        ) = if let Some(p) = pet_node {
            (
                p.attribute("name").map(|s| s.to_string()),
                p.attribute("type").and_then(|s| s.parse::<i32>().ok()),
                p.attribute("instanceId")
                    .and_then(|s| s.parse::<i32>().ok()),
                p.attribute("rarity").and_then(|s| s.parse::<i32>().ok()),
                p.attribute("maxAbilityPower")
                    .and_then(|s| s.parse::<i32>().ok()),
                p.attribute("skin").and_then(|s| s.parse::<i32>().ok()),
                p.attribute("shader").and_then(|s| s.parse::<i32>().ok()),
                p.attribute("createdOn").map(|s| s.to_string()),
                p.attribute("incInv").and_then(|s| s.parse::<i32>().ok()),
                p.attribute("inv").map(|s| s.to_string()),
                ability_attr(p, 1, "type"),
                ability_attr(p, 1, "power"),
                ability_attr(p, 1, "points"),
                ability_attr(p, 2, "type"),
                ability_attr(p, 2, "power"),
                ability_attr(p, 2, "points"),
                ability_attr(p, 3, "type"),
                ability_attr(p, 3, "power"),
                ability_attr(p, 3, "points"),
            )
        } else {
            (
                None, None, None, None, None, None, None, None, None, None, None, None, None, None,
                None, None, None, None, None,
            )
        };

        out.push(Character {
            id: None,
            entry_id: None,
            char_id,
            char_class: i32_of(c, "ObjectType"),
            seasonal: bool_of(c, "Seasonal"),
            level: i32_of(c, "Level"),
            exp: i64_of(c, "Exp"),
            current_fame: i32_of(c, "CurrentFame"),
            equipment: text_of(c, "Equipment").map(|s| s.to_string()),
            equip_qs: text_of(c, "EquipQS").map(|s| s.to_string()),
            max_hit_points: i32_of(c, "MaxHitPoints"),
            hit_points: i32_of(c, "HitPoints"),
            max_magic_points: i32_of(c, "MaxMagicPoints"),
            magic_points: i32_of(c, "MagicPoints"),
            attack: i32_of(c, "Attack"),
            defense: i32_of(c, "Defense"),
            speed: i32_of(c, "Speed"),
            dexterity: i32_of(c, "Dexterity"),
            hp_regen: i32_of(c, "HpRegen"),
            mp_regen: i32_of(c, "MpRegen"),
            health_stack_count: i32_of(c, "HealthStackCount"),
            magic_stack_count: i32_of(c, "MagicStackCount"),
            dead: bool_of(c, "Dead"),
            pet_name,
            pet_type,
            pet_instance_id,
            pet_rarity,
            pet_max_ability_power,
            pet_skin,
            pet_shader,
            pet_created_on,
            pet_inc_inv,
            pet_inv,
            pet_ability1_type,
            pet_ability1_power,
            pet_ability1_points,
            pet_ability2_type,
            pet_ability2_power,
            pet_ability2_points,
            pet_ability3_type,
            pet_ability3_power,
            pet_ability3_points,
            account_name: find_path_under(c, &["Account"])
                .and_then(|n| text_of(n, "Name"))
                .map(|s| s.to_string()),
            backpack_slots: i32_of(c, "BackpackSlots"),
            has3_quickslots: i32_of(c, "Has3Quickslots"),
            creation_date: text_of(c, "CreationDate").map(|s| s.to_string()),
            pc_stats: text_of(c, "PCStats").map(|s| compact_ascii_ws(s)),
            tex1: text_of(c, "Tex1").map(|s| s.to_string()),
            tex2: text_of(c, "Tex2").map(|s| s.to_string()),
            texture: text_of(c, "Texture").map(|s| s.to_string()),
            xp_boosted: i32_of(c, "XpBoosted"),
            xp_timer: i32_of(c, "XpTimer"),
            ld_timer: i32_of(c, "LDTimer"),
            lt_timer: i32_of(c, "LTTimer"),
            crucible_active: text_of(c, "CrucibleActive").map(|s| s.to_string()),
        });
    }
    Ok(out)
}

// ─────────────────────────────────────────────────────────────────────────────
// Items collection (normalized)
// ─────────────────────────────────────────────────────────────────────────────

#[derive(Debug, Clone)]
enum Storage {
    Char(i32), // char id
    Vault { chest_idx: i32 },
    MatStorage { page_idx: i32 },
    Gifts,
    TempGifts,
    Potions,
}

impl Storage {
    fn storage_type_id(&self) -> String {
        match self {
            Storage::Char(cid) => format!("char:{cid}"),
            Storage::Vault { .. } => "vault".to_string(),
            Storage::MatStorage { .. } => "mat_storage".to_string(),
            Storage::Gifts => "gifts".to_string(),
            Storage::TempGifts => "temp_gifts".to_string(),
            Storage::Potions => "potions".to_string(),
        }
    }
    fn container_index(&self) -> Option<i32> {
        match self {
            Storage::Vault { chest_idx } => Some(*chest_idx),
            Storage::MatStorage { page_idx } => Some(*page_idx),
            Storage::Gifts | Storage::TempGifts | Storage::Potions => Some(0),
            Storage::Char(_) => None, // for chars, we set per-slice below
        }
    }
}

type Key = (i32, Option<String>); // (item_id, uniqueId?)
type UniqueMap = HashMap<Key, Vec<String>>;

struct UniqueResolver {
    per_char: HashMap<i32, UniqueMap>,
    gifts: UniqueMap,
    temp_gifts: UniqueMap,
    account: UniqueMap, // vault / mat / potions
}

impl UniqueResolver {
    fn from_doc(doc: &Document) -> Result<Self, ParseError> {
        let mut per_char = HashMap::new();
        for c in doc.descendants().filter(|n| n.has_tag_name("Char")) {
            if let Some(cid) = c.attribute("id").and_then(|s| s.parse::<i32>().ok()) {
                let mut map = UniqueMap::new();
                if let Some(uii) = c.children().find(|n| n.has_tag_name("UniqueItemInfo")) {
                    index_unique_block(uii, &mut map);
                }
                per_char.insert(cid, map);
            }
        }

        let gifts = find_first(doc, "UniqueGiftItemInfo")
            .map(|n| {
                let mut m = UniqueMap::new();
                index_unique_block(n, &mut m);
                m
            })
            .unwrap_or_default();

        let temp_gifts = find_first(doc, "UniqueTemporaryGiftItemInfo")
            .map(|n| {
                let mut m = UniqueMap::new();
                index_unique_block(n, &mut m);
                m
            })
            .unwrap_or_default();

        let account = find_path(doc, &["Chars", "Account"])
            .and_then(|acc| acc.children().find(|n| n.has_tag_name("UniqueItemInfo")))
            .map(|n| {
                let mut m = UniqueMap::new();
                index_unique_block(n, &mut m);
                m
            })
            .unwrap_or_default();

        Ok(Self {
            per_char,
            gifts,
            temp_gifts,
            account,
        })
    }

    fn pop(map: &mut UniqueMap, item_id: i32, uid: Option<&str>) -> Option<String> {
        let key = (item_id, uid.map(|s| s.to_string()));
        map.get_mut(&key).and_then(|v| {
            if v.is_empty() {
                None
            } else {
                Some(v.remove(0))
            }
        })
    }

    fn resolve_for(
        &mut self,
        storage: &Storage,
        item_id: i32,
        unique: Option<&str>,
    ) -> Option<String> {
        match storage {
            Storage::Char(cid) => {
                if let Some(m) = self.per_char.get_mut(cid) {
                    if let Some(u) = unique {
                        if let Some(v) = Self::pop(m, item_id, Some(u)) {
                            return Some(v);
                        }
                    }
                    return Self::pop(m, item_id, None);
                }
                None
            }
            Storage::Gifts => {
                if let Some(u) = unique {
                    if let Some(v) = Self::pop(&mut self.gifts, item_id, Some(u)) {
                        return Some(v);
                    }
                }
                Self::pop(&mut self.gifts, item_id, None)
            }
            Storage::TempGifts => {
                if let Some(u) = unique {
                    if let Some(v) = Self::pop(&mut self.temp_gifts, item_id, Some(u)) {
                        return Some(v);
                    }
                }
                Self::pop(&mut self.temp_gifts, item_id, None)
            }
            Storage::Vault { .. } | Storage::MatStorage { .. } | Storage::Potions => {
                if let Some(u) = unique {
                    if let Some(v) = Self::pop(&mut self.account, item_id, Some(u)) {
                        return Some(v);
                    }
                }
                Self::pop(&mut self.account, item_id, None)
            }
        }
    }
}

fn index_unique_block(uii: Node, map: &mut UniqueMap) {
    for item in uii.children().filter(|n| n.has_tag_name("ItemData")) {
        let item_id = item.attribute("type").and_then(|s| s.parse::<i32>().ok());
        let uid = item.attribute("id").map(|s| s.to_string());
        if let Some(id) = item_id {
            if let Some(val) = item.text().map(|t| t.trim().to_string()) {
                if !val.is_empty() {
                    map.entry((id, uid)).or_default().push(val);
                }
            }
        }
    }
}

#[derive(Debug, Clone)]
struct Token {
    item_id: i32,
    unique_id: Option<String>,
}

fn parse_token_list(s: &str) -> Vec<Token> {
    let mut v = Vec::new();
    for raw in s.split(',') {
        let t = raw.trim();
        if t.is_empty() {
            continue;
        }
        let (lhs, uid) = if let Some((l, r)) = t.split_once('#') {
            (l.trim(), Some(r.trim().to_string()))
        } else {
            (t, None)
        };
        if let Ok(id) = lhs.parse::<i32>() {
            v.push(Token {
                item_id: id,
                unique_id: uid,
            });
        }
    }
    v
}

fn collect_all_items(
    doc: &Document,
    entry_id: Option<String>,
    account_id: Option<String>,
    uniq: &mut UniqueResolver,
) -> Vec<ParsedItem> {
    let mut out = Vec::new();

    // Characters: slice equipment into containers 0,1,2,3...
    for cnode in doc.descendants().filter(|n| n.has_tag_name("Char")) {
        let cid = cnode
            .attribute("id")
            .and_then(|s| s.parse::<i32>().ok())
            .unwrap_or_default();
        let storage = Storage::Char(cid);

        let bp_slots = i32_of_node(cnode, "BackpackSlots").unwrap_or(0);
        let bp_count = (bp_slots / 8).clamp(0, 8); // generous upper bound

        if let Some(eq) = cnode.children().find(|n| n.has_tag_name("Equipment")) {
            let tokens = parse_token_list(eq.text().unwrap_or_default());
            emit_character_equipment(
                &tokens,
                bp_count,
                &storage,
                entry_id.clone(),
                account_id.clone(),
                uniq,
                &mut out,
            );
        }
    }

    // Vault
    if let Some(vault) = find_path(doc, &["Chars", "Account", "Vault"]) {
        for (idx, chest) in vault
            .children()
            .filter(|n| n.has_tag_name("Chest"))
            .enumerate()
        {
            let storage = Storage::Vault {
                chest_idx: idx as i32,
            };
            emit_from_flat_list(
                chest.text().unwrap_or_default(),
                &storage,
                entry_id.clone(),
                account_id.clone(),
                uniq,
                storage.container_index(),
                &mut out,
            );
        }
    }

    // Material storage
    if let Some(ms) = find_path(doc, &["Chars", "Account", "MaterialStorage"]) {
        for (idx, page) in ms
            .children()
            .filter(|n| n.has_tag_name("Chest"))
            .enumerate()
        {
            let storage = Storage::MatStorage {
                page_idx: idx as i32,
            };
            emit_from_flat_list(
                page.text().unwrap_or_default(),
                &storage,
                entry_id.clone(),
                account_id.clone(),
                uniq,
                storage.container_index(),
                &mut out,
            );
        }
    }

    // Gifts
    if let Some(gifts) = find_path(doc, &["Chars", "Account", "Gifts"]) {
        let storage = Storage::Gifts;
        emit_from_flat_list(
            gifts.text().unwrap_or_default(),
            &storage,
            entry_id.clone(),
            account_id.clone(),
            uniq,
            storage.container_index(),
            &mut out,
        );
    }

    // TemporaryGifts
    if let Some(tg) = find_path(doc, &["Chars", "Account", "TemporaryGifts"]) {
        let storage = Storage::TempGifts;
        emit_from_flat_list(
            tg.text().unwrap_or_default(),
            &storage,
            entry_id.clone(),
            account_id.clone(),
            uniq,
            storage.container_index(),
            &mut out,
        );
    }

    // Potions
    if let Some(p) = find_path(doc, &["Chars", "Account", "Potions"]) {
        let storage = Storage::Potions;
        emit_from_flat_list(
            p.text().unwrap_or_default(),
            &storage,
            entry_id,
            account_id,
            uniq,
            storage.container_index(),
            &mut out,
        );
    }

    out
}

/// Characters: slice the long equipment into containers (0=equip 4 slots, 1=inv 8 slots, 2..=backpacks)
fn emit_character_equipment(
    tokens: &Vec<Token>,
    backpack_count: i32,
    storage: &Storage,
    entry_id: Option<String>,
    account_id: Option<String>,
    uniq: &mut UniqueResolver,
    out: &mut Vec<ParsedItem>,
) {
    const EQUIP: usize = 4;
    const INV: usize = 8;
    const BP: usize = 8;

    let base = EQUIP + INV; // 12
    let expected = base + (backpack_count.max(0) as usize) * BP;

    // copy + pad/truncate
    let mut flat = tokens.clone();
    if flat.len() < expected {
        flat.extend(
            std::iter::repeat(Token {
                item_id: -1,
                unique_id: None,
            })
            .take(expected - flat.len()),
        );
    } else if flat.len() > expected {
        flat.truncate(expected);
    }

    // container 0: [0..4)
    emit_slice(
        &flat[0..EQUIP],
        0,
        storage,
        entry_id.clone(),
        account_id.clone(),
        uniq,
        out,
    );

    // container 1: [4..12)
    emit_slice(
        &flat[EQUIP..base],
        1,
        storage,
        entry_id.clone(),
        account_id.clone(),
        uniq,
        out,
    );

    // backpacks
    for bi in 0..backpack_count.max(0) as usize {
        let start = base + bi * BP;
        let end = start + BP;
        emit_slice(
            &flat[start..end],
            (2 + bi) as i32,
            storage,
            entry_id.clone(),
            account_id.clone(),
            uniq,
            out,
        );
    }
}

fn emit_slice(
    slice: &[Token],
    container_index: i32,
    storage: &Storage,
    entry_id: Option<String>,
    account_id: Option<String>,
    uniq: &mut UniqueResolver,
    out: &mut Vec<ParsedItem>,
) {
    for (slot_idx, tok) in slice.iter().enumerate() {
        let enchant = if tok.item_id >= 0 {
            uniq.resolve_for(storage, tok.item_id, tok.unique_id.as_deref())
        } else {
            None
        };

        out.push(ParsedItem {
            entry_id: entry_id.clone(),
            account_id: account_id.clone(),
            storage_type_id: storage.storage_type_id(),
            container_index: Some(container_index),
            slot_index: Some(slot_idx as i32),
            quantity: 1,
            item_id: tok.item_id,
            unique_id_raw: tok.unique_id.clone(),
            enchant_b64: enchant,
            enchant_json: None, // decoder later
        });
    }
}

fn emit_from_flat_list(
    raw: &str,
    storage: &Storage,
    entry_id: Option<String>,
    account_id: Option<String>,
    uniq: &mut UniqueResolver,
    container_index: Option<i32>,
    out: &mut Vec<ParsedItem>,
) {
    let toks = parse_token_list(raw);
    for (i, tok) in toks.into_iter().enumerate() {
        let enchant = if tok.item_id >= 0 {
            uniq.resolve_for(storage, tok.item_id, tok.unique_id.as_deref())
        } else {
            None
        };

        out.push(ParsedItem {
            entry_id: entry_id.clone(),
            account_id: account_id.clone(),
            storage_type_id: storage.storage_type_id(),
            container_index,
            slot_index: Some(i as i32),
            quantity: 1,
            item_id: tok.item_id,
            unique_id_raw: tok.unique_id,
            enchant_b64: enchant,
            enchant_json: None,
        });
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// XML helpers
// ─────────────────────────────────────────────────────────────────────────────

/// Join a CSV-ish string by comma with each token trimmed, removing blanks and newlines.
fn normalize_csv_str(raw: &str) -> String {
    raw.split(',')
        .map(|s| s.trim())
        .filter(|s| !s.is_empty())
        .collect::<Vec<_>>()
        .join(",")
}

/// Remove all ASCII whitespace (space, tab, CR, LF). Good for base64-ish blobs like PCStats.
fn compact_ascii_ws(raw: &str) -> String {
    raw.chars().filter(|c| !c.is_ascii_whitespace()).collect()
}

fn find_first<'a>(doc: &'a roxmltree::Document<'a>, name: &str) -> Option<roxmltree::Node<'a, 'a>> {
    doc.descendants().find(|n| n.has_tag_name(name))
}

fn find_path<'a>(
    doc: &'a roxmltree::Document<'a>,
    path: &[&str],
) -> Option<roxmltree::Node<'a, 'a>> {
    let mut cur: Option<roxmltree::Node<'a, 'a>> = None;
    for (i, tag) in path.iter().enumerate() {
        cur = if i == 0 {
            doc.descendants().find(|n| n.has_tag_name(*tag))
        } else {
            cur.and_then(|n| n.children().find(|c| c.has_tag_name(*tag)))
        };
        if cur.is_none() {
            return None;
        }
    }
    cur
}

fn find_path_under<'a>(
    node: roxmltree::Node<'a, 'a>,
    path: &[&str],
) -> Option<roxmltree::Node<'a, 'a>> {
    let mut cur: Option<roxmltree::Node<'a, 'a>> = Some(node);
    for tag in path {
        cur = cur.and_then(|n| n.children().find(|c| c.has_tag_name(*tag)));
        if cur.is_none() {
            return None;
        }
    }
    cur
}

fn text_of<'a>(node: roxmltree::Node<'a, 'a>, child_tag: &str) -> Option<&'a str> {
    for c in node.children() {
        if c.has_tag_name(child_tag) {
            if let Some(t) = c.text() {
                return Some(t); // &'a str
            }
        }
    }
    None
}

fn text_at_path<'a>(doc: &'a roxmltree::Document<'a>, path: &[&str]) -> Option<&'a str> {
    find_path(doc, path).and_then(|n| n.text())
}

fn i32_of(node: Node, tag: &str) -> Option<i32> {
    text_of(node, tag).and_then(|s| s.trim().parse::<i32>().ok())
}
fn i64_of(node: Node, tag: &str) -> Option<i64> {
    text_of(node, tag).and_then(|s| s.trim().parse::<i64>().ok())
}
fn bool_of(node: Node, tag: &str) -> Option<bool> {
    text_of(node, tag).map(|s| s.eq_ignore_ascii_case("true"))
}
fn i32_of_node(node: Node, tag: &str) -> Option<i32> {
    i32_of(node, tag)
}

fn children_as_array<'a>(node: Node<'a, 'a>, name: &str) -> Vec<Node<'a, 'a>> {
    node.children().filter(|n| n.has_tag_name(name)).collect()
}

fn ability_attr(pet: Node, idx: i32, attr: &str) -> Option<i32> {
    pet.children()
        .filter(|n| n.has_tag_name("Abilities"))
        .flat_map(|ab| ab.children().filter(|a| a.has_tag_name("Ability")))
        .nth((idx - 1).max(0) as usize)
        .and_then(|a| a.attribute(attr))
        .and_then(|v| v.parse::<i32>().ok())
}
