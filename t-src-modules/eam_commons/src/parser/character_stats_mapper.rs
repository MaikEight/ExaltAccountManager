/**
 * Mapping for character stats types. It maps the api-specified types to internal representations.
 * 
 */
pub fn map_character_stats_type(api_type: u8) -> u8 {
    match api_type {
        0 => 0,
        _ => api_type,
    }
}