
//enum of stat types
#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub enum StatTypeEnum {
    Stats = 0,
    Dungeons = 1,
}

//Stat type struct, id, name, description
#[derive(Debug, Clone)]
pub struct StatType {
    pub id: u8,
    pub stat_type: StatTypeEnum,
    pub name: String,
    pub description: String,
}

//constant map of stat types 
use lazy_static::lazy_static;
use std::collections::HashMap;
lazy_static! {
    pub static ref STAT_TYPE_MAP: HashMap<u8, StatType> = {
        let mut m = HashMap::new();
        m.insert(0, StatType { id: 0, stat_type: StatTypeEnum::Stats, name: "Shots fired".to_string(), description: "The total amount of shots fired.".to_string() });
        m.insert(1, StatType { id: 1, stat_type: StatTypeEnum::Stats, name: "Hits".to_string(), description: "The total amount of shots that hit an enemy.".to_string() });
        m.insert(2, StatType { id: 2, stat_type: StatTypeEnum::Stats, name: "Ability uses".to_string(), description: "The total amount of ability uses.".to_string() });
        m.insert(3, StatType { id: 3, stat_type: StatTypeEnum::Stats, name: "Tiles discovered".to_string(), description: "The total amount of tiles discovered.".to_string() });
        /*
        Stats
            4	Teleports
            5	Potions drunk
            6	Kills
            7	Assists
            8	God kills
            9	Assists against Gods
            10	Cube kills
            11	Oryx kills
            12	Quests completed
            20	 Minutes active
        Dungeons
            14  Undead Lair
            15  Abyss of Demons
            16  Snake Pit
            18  Sprite World
         */



        
        m
    };
}
