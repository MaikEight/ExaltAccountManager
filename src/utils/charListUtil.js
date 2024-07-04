import { invoke } from "@tauri-apps/api";

async function storeCharList(charList, email) {
    //format the charList to a model::dataset

    const accountId = charList?.Chars?.Account?.AccountId;
    const dataset = {
        email: email,
        account: charToAccountModel(charList?.Chars?.Account),
        class_stats: charList?.Chars?.Account?.Stats?.ClassStats
            ? Array.isArray(charList.Chars.Char)
                ? charList.Chars.Account.Stats.ClassStats.map(stats => charToClassStatsModel(stats, accountId))
                : [charToClassStatsModel(charList.Chars.Account.Stats.ClassStats, accountId)]
            : [],
        character: charList?.Chars?.Char
            ? Array.isArray(charList.Chars.Char)
                ? charList.Chars.Char.map(c => charToCharModel(c))
                : [charToCharModel(charList.Chars.Char)]
            : [],
    };

    //store the dataset in the sqlite db
    return await invoke('insert_char_list_dataset', { dataset: dataset });
}

function getRequestState(charList) {
    const hasErrors = charList?.Error !== undefined;

    if (hasErrors) {
        console.info('charList has error', charList.Error);
        const error = charList.Error?.toLowerCase();

        if (error.includes("passworderror")) {
            return "WrongPassword";
        } else if (error.includes("wait") || error.includes("try again later")) {
            return "TooManyRequests";
        } else if (error.includes("captchalock")) {
            return "Captcha";
        } else if (error.includes("suspended")) {
            return "AccountSuspended";
        } else {
            return "Error";
        }
    }

    return "Success";
}

function charToAccountModel(account) {
    if (!account) return null;
    const formatChestData = (chest) => {
        //chest is an array of strings
        //each string is a comma separated list of items
        //return a single string with all the items
        //example:
        //chest = ["9053#4835986114186395,19260","6135,6135,7738,7731,7731,7731"]
        //return "9053#4835986114186395,19260,6135,6135,7738,7731,7731,7731"
        try {
            if (chest === undefined || chest === null) return "";
            if (typeof chest === "string") return chest;

            return chest.reduce((acc, curr) => {
                return acc + (acc === "" ? "" : ",") + curr;
            }, "");
        } catch (error) {
            console.error('formatChestData', error, chest);
            throw error;
        }
    };

    return {
        entry_id: null,
        account_id: account.AccountId,
        credits: account.Credits ? parseInt(account.Credits, 10) : 0,
        fortune_token: account.FortuneToken ? parseInt(account.FortuneToken, 10) : 0,
        unity_campaign_points: account.UnityCampaignPoints ? parseInt(account.UnityCampaignPoints, 10) : 0,
        early_game_event_tracker: account.EarlyGameEventTracker ? parseInt(account.EarlyGameEventTracker, 10) : 0,
        creation_timestamp: account.CreationTimestamp,
        enchanter_support_dust: account.EnchanterSupportDust ? parseInt(account.EnchanterSupportDust, 10) : 0,
        vault: formatChestData(account.Vault?.Chest),
        material_storage: formatChestData(account.MaterialStorage?.Chest),
        gifts: account.Gifts,
        temporary_gifts: account.TemporaryGifts,
        potions: account.Potions,
        max_num_chars: account.MaxNumChars ? parseInt(account.MaxNumChars, 10) : 0,
        last_server: account.LastServer,
        originating: account.Originating,
        pet_yard_type: account.PetYardType ? parseInt(account.PetYardType, 10) : 0,
        forge_fire_energy: account.ForgeFireEnergy ? parseInt(account.ForgeFireEnergy, 10) : 0,
        regular_forge_fire_blueprints: account.RegularForgeFireBlueprints,
        name: account.Name,
        best_char_fame: account.Stats?.BestCharFame ? parseInt(account.Stats.BestCharFame, 10) : 0,
        total_fame: account.Stats?.TotalFame ? parseInt(account.Stats.TotalFame, 10) : 0,
        fame: account.Stats?.Fame ? parseInt(account.Stats.Fame, 10) : 0,
        guild_name: account.Guild?.Name,
        guild_rank: account.Guild?.Rank ? parseInt(account.Guild.Rank, 10) : 0,
        access_token_timestamp: account.AccessTokenTimestamp,
        access_token_expiration: account.AccessTokenExpiration,
        owned_skins: account.OwnedSkins,
    }
}

function charToClassStatsModel(stats, accountId) {
    if (!stats) return null;

    return {
        id: null,
        entry_id: null,
        account_id: accountId,
        class_type: stats._attributes?.objectType ? parseInt(stats._attributes.objectType, 16) : -1,
        best_level: stats.BestLevel ? parseInt(stats.BestLevel, 10) : 0,
        best_base_fame: stats.BestFame ? parseInt(stats.BestFame, 10) : 0,
        best_total_fame: stats.BestTotalFame ? parseInt(stats.BestTotalFame, 10) : 0,
    }
};

function charToCharModel(char) {
    if (!char) return null;

    return {
        entry_id: null,
        char_id: char._attributes?.id ? parseInt(char._attributes.id, 10) : -1,
        char_class: char.ObjectType ? parseInt(char.ObjectType, 10) : -1,
        seasonal: char.Seasonal?.toLowerCase() === "true",
        level: char.Level ? parseInt(char.Level, 10) : 1,
        exp: char.Exp ? parseInt(char.Exp, 10) : 0,
        current_fame: char.CurrentFame ? parseInt(char.CurrentFame, 10) : 0,
        equipment: char.Equipment,
        equip_qs: char.EquipQS,
        max_hit_points: char.MaxHitPoints ? parseInt(char.MaxHitPoints, 10) : 1,
        hit_points: char.HitPoints ? parseInt(char.HitPoints, 10) : 0,
        max_magic_points: char.MaxMagicPoints ? parseInt(char.MaxMagicPoints, 10) : 1,
        magic_points: char.MagicPoints ? parseInt(char.MagicPoints, 10) : 0,
        attack: char.Attack ? parseInt(char.Attack, 10) : 0,
        defense: char.Defense ? parseInt(char.Defense, 10) : 0,
        speed: char.Speed ? parseInt(char.Speed, 10) : 0,
        dexterity: char.Dexterity ? parseInt(char.Dexterity, 10) : 0,
        hp_regen: char.HpRegen ? parseInt(char.HpRegen, 10) : 0,
        mp_regen: char.MpRegen ? parseInt(char.MpRegen, 10) : 0,
        health_stack_count: char.HealthStackCount ? parseInt(char.HealthStackCount, 10) : 0,
        magic_stack_count: char.MagicStackCount ? parseInt(char.MagicStackCount, 10) : 0,
        dead: char.Dead?.toLowerCase() === "true",
        pet_name: char.Pet?.name,
        pet_type: char.Pet?.type ? parseInt(char.Pet.type, 10) : -1,
        pet_instance_id: char.Pet?.instanceId ? parseInt(char.Pet.instanceId, 10) : -1,
        pet_rarity: char.Pet?.rarity ? parseInt(char.Pet.rarity, 10) : -1,
        pet_max_ability_power: char.Pet?.maxAbilityPower ? parseInt(char.Pet.maxAbilityPower, 10) : 0,
        pet_skin: char.Pet?.skin ? parseInt(char.Pet.skin, 10) : -1,
        pet_shader: char.Pet?.shader ? parseInt(char.Pet.shader, 10) : -1,
        pet_created_on: char.Pet?.createdOn,
        pet_inc_inv: char.Pet?.incInv ? parseInt(char.Pet.incInv, 10) : 0,
        pet_inv: char.Pet?.inv,
        pet_ability1_type: char.Pet?.Abilities?.Ability[0]?._attributes?.type ? parseInt(char.Pet?.Abilities?.Ability[0]?._attributes?.type, 10) : -1,
        pet_ability1_power: char.Pet?.Abilities?.Ability[0]?._attributes?.power ? parseInt(char.Pet?.Abilities?.Ability[0]?._attributes?.power, 10) : 0,
        pet_ability1_points: char.Pet?.Abilities?.Ability[0]?._attributes?.points ? parseInt(char.Pet?.Abilities?.Ability[0]?._attributes?.points, 10) : 0,
        pet_ability2_type: char.Pet?.Abilities?.Ability[1]?._attributes?.type ? parseInt(char.Pet?.Abilities?.Ability[1]?._attributes?.type, 10) : -1,
        pet_ability2_power: char.Pet?.Abilities?.Ability[1]?._attributes?.power ? parseInt(char.Pet?.Abilities?.Ability[1]?._attributes?.power, 10) : 0,
        pet_ability2_points: char.Pet?.Abilities?.Ability[1]?._attributes?.points ? parseInt(char.Pet?.Abilities?.Ability[1]?._attributes?.points, 10) : 0,
        pet_ability3_type: char.Pet?.Abilities?.Ability[2]?._attributes?.type ? parseInt(char.Pet?.Abilities?.Ability[2]?._attributes?.type, 10) : -1,
        pet_ability3_points: char.Pet?.Abilities?.Ability[2]?._attributes?.power ? parseInt(char.Pet?.Abilities?.Ability[2]?._attributes?.power, 10) : 0,
        petAbility3Points: char.Pet?.Abilities?.Ability[2]?._attributes?.points ? parseInt(char.Pet?.Abilities?.Ability[2]?._attributes?.points, 10) : 0,
        account_name: char.Account?.Name,
        backpack_slots: char.BackpackSlots ? parseInt(char.BackpackSlots, 10) : 0,
        has3_quickslots: char.Has3Quickslots ? parseInt(char.Has3Quickslots, 10) : 0,
        creation_date: char.CreationDate,
        pc_stats: char.PCStats,
        tex1: char.Tex1 ?? null,
        tex2: char.Tex2 ?? null,
        texture: char.Texture ?? null,
        xp_boosted: char.XpBoosted ? parseInt(char.XpBoosted, 10) : 0,
        xp_timer: char.XpTimer ? parseInt(char.XpTimer, 10) : 0,
        ld_timer: char.LdTimer ? parseInt(char.LdTimer, 10) : 0,
        lt_timer: char.LtTimer ? parseInt(char.LtTimer, 10) : 0,
    };
}

export { storeCharList, getRequestState };