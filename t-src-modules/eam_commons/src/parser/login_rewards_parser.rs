use roxmltree::Document;

use crate::models::{LoginRewardEntry, LoginRewardsResponse};

/// Parses a `dailyLogin/fetchCalendar` response.
///
/// Only the `<NonConsecutive>` block carries reward items; `<Consecutive>` is
/// ignored. The `<Unlockable days=N>` attribute is authoritative for "logins
/// this month" (claimed + still-unlockable combined) = the number of unlocked
/// tiers.
pub fn parse_login_rewards(xml: &str) -> Result<LoginRewardsResponse, String> {
    let doc = Document::parse(xml).map_err(|e| format!("Failed to parse LoginRewards XML: {}", e))?;

    let root = doc
        .descendants()
        .find(|n| n.has_tag_name("LoginRewards"))
        .ok_or_else(|| "No <LoginRewards> element found".to_string())?;

    let server_time = root.attribute("serverTime").unwrap_or("").to_string();

    // Logins this month come straight from <Unlockable days=N>.
    let unlockable_days = doc
        .descendants()
        .find(|n| n.has_tag_name("Unlockable"))
        .and_then(|n| n.attribute("days"))
        .and_then(|d| d.trim().parse::<i32>().ok())
        .unwrap_or(0);

    let mut entries = Vec::new();

    if let Some(non_consecutive) = doc.descendants().find(|n| n.has_tag_name("NonConsecutive")) {
        for login in non_consecutive.children().filter(|n| n.has_tag_name("Login")) {
            let day = login
                .children()
                .find(|n| n.has_tag_name("Days"))
                .and_then(|n| n.text())
                .and_then(|t| t.trim().parse::<i32>().ok())
                .unwrap_or(0);

            let item_node = login.children().find(|n| n.has_tag_name("ItemId"));
            let item_id = item_node
                .and_then(|n| n.text())
                .and_then(|t| t.trim().parse::<i32>().ok())
                .unwrap_or(-1);
            let quantity = item_node
                .and_then(|n| n.attribute("quantity"))
                .and_then(|q| q.trim().parse::<i32>().ok())
                .unwrap_or(1);

            let gold = login
                .children()
                .find(|n| n.has_tag_name("Gold"))
                .and_then(|n| n.text())
                .and_then(|t| t.trim().parse::<i32>().ok())
                .unwrap_or(0);

            let claimed = login.children().any(|n| n.has_tag_name("Claimed"));

            // Skip malformed entries with no tier index.
            if day <= 0 {
                continue;
            }

            entries.push(LoginRewardEntry {
                day,
                item_id,
                quantity,
                gold,
                claimed,
            });
        }
    }

    Ok(LoginRewardsResponse {
        server_time,
        unlockable_days,
        entries,
    })
}

#[cfg(test)]
mod tests {
    use super::*;

    const SAMPLE: &str = include_str!("test_data/login_rewards_sample.xml");

    #[test]
    fn test_parse_login_rewards_sample() {
        let parsed = parse_login_rewards(SAMPLE).expect("should parse sample");

        assert_eq!(parsed.server_time, "1783293127.793961");
        assert_eq!(parsed.unlockable_days, 5);
        assert_eq!(parsed.entries.len(), 31);

        // Day 1 = item 3138 x1, claimed.
        let d1 = &parsed.entries[0];
        assert_eq!(d1.day, 1);
        assert_eq!(d1.item_id, 3138);
        assert_eq!(d1.quantity, 1);
        assert!(d1.claimed);

        // Day 4 has quantity 2.
        let d4 = parsed.entries.iter().find(|e| e.day == 4).unwrap();
        assert_eq!(d4.item_id, 3176);
        assert_eq!(d4.quantity, 2);
        assert!(d4.claimed);

        // Days 1..=5 are claimed, days 6..=31 are not.
        for e in &parsed.entries {
            if e.day <= 5 {
                assert!(e.claimed, "day {} should be claimed", e.day);
            } else {
                assert!(!e.claimed, "day {} should not be claimed", e.day);
            }
        }
    }

    #[test]
    fn test_parse_login_rewards_invalid_xml() {
        assert!(parse_login_rewards("not valid xml").is_err());
    }
}
