namespace MK_EAM_Lib
{
    public static class CharacterClassesUtil
    {
        public static readonly System.Collections.Generic.Dictionary<int, CharClasses> dicClasses = new System.Collections.Generic.Dictionary<int, CharClasses>()
        {
            { 768, CharClasses.Rogue},
            { 775, CharClasses.Archer},
            { 782, CharClasses.Wizard},
            { 784, CharClasses.Priest},
            { 797, CharClasses.Warrior},
            { 798, CharClasses.Knight},
            { 799, CharClasses.Paladin},
            { 800, CharClasses.Assassin},
            { 801, CharClasses.Necromancer},
            { 802, CharClasses.Huntress},
            { 803, CharClasses.Mystic},
            { 804, CharClasses.Trickster},
            { 805, CharClasses.Sorcerer},
            { 806, CharClasses.Ninja},
            { 785, CharClasses.Samurai},
            { 796, CharClasses.Bard},
            { 817, CharClasses.Summoner},
        };

        public static readonly System.Collections.Generic.Dictionary<CharClasses, CharacterStats> dicCharClassToMaxStats = new System.Collections.Generic.Dictionary<CharClasses, CharacterStats>()
        {
            {CharClasses.Rogue, new CharacterStats()        { maxHP = 720, maxMP = 252, atk = 50, def = 25, spd = 75, dex = 75, vit = 40, wis = 50 } },
            {CharClasses.Archer, new CharacterStats()       { maxHP = 700, maxMP = 252, atk = 75, def = 25, spd = 50, dex = 50, vit = 40, wis = 50 } },
            {CharClasses.Wizard, new CharacterStats()       { maxHP = 670, maxMP = 385, atk = 75, def = 25, spd = 50, dex = 75, vit = 40, wis = 60 } },
            {CharClasses.Priest, new CharacterStats()       { maxHP = 670, maxMP = 385, atk = 50, def = 25, spd = 55, dex = 55, vit = 40, wis = 75 } },
            {CharClasses.Warrior, new CharacterStats()      { maxHP = 770, maxMP = 252, atk = 75, def = 25, spd = 50, dex = 50, vit = 75, wis = 50 } },
            {CharClasses.Knight, new CharacterStats()       { maxHP = 770, maxMP = 252, atk = 50, def = 40, spd = 50, dex = 50, vit = 75, wis = 50 } },
            {CharClasses.Paladin, new CharacterStats()      { maxHP = 770, maxMP = 252, atk = 55, def = 30, spd = 55, dex = 55, vit = 60, wis = 75 } },
            {CharClasses.Assassin, new CharacterStats()     { maxHP = 720, maxMP = 252, atk = 60, def = 25, spd = 75, dex = 75, vit = 40, wis = 60 } },
            {CharClasses.Necromancer, new CharacterStats()  { maxHP = 670, maxMP = 385, atk = 75, def = 25, spd = 50, dex = 60, vit = 40, wis = 75 } },
            {CharClasses.Huntress, new CharacterStats()     { maxHP = 700, maxMP = 252, atk = 75, def = 25, spd = 50, dex = 50, vit = 40, wis = 50 } },
            {CharClasses.Mystic, new CharacterStats()       { maxHP = 670, maxMP = 385, atk = 65, def = 25, spd = 60, dex = 65, vit = 40, wis = 75 } },
            {CharClasses.Trickster, new CharacterStats()    { maxHP = 720, maxMP = 252, atk = 65, def = 25, spd = 75, dex = 75, vit = 40, wis = 60 } },
            {CharClasses.Sorcerer, new CharacterStats()     { maxHP = 670, maxMP = 385, atk = 70, def = 25, spd = 60, dex = 60, vit = 75, wis = 60 } },
            {CharClasses.Ninja, new CharacterStats()        { maxHP = 720, maxMP = 252, atk = 70, def = 25, spd = 60, dex = 70, vit = 60, wis = 70 } },
            {CharClasses.Samurai, new CharacterStats()      { maxHP = 720, maxMP = 252, atk = 75, def = 30, spd = 55, dex = 50, vit = 60, wis = 60 } },
            {CharClasses.Bard, new CharacterStats()         { maxHP = 670, maxMP = 385, atk = 55, def = 25, spd = 55, dex = 70, vit = 45, wis = 75 } },
            {CharClasses.Summoner, new CharacterStats()     { maxHP = 670, maxMP = 385, atk = 50, def = 25, spd = 60, dex = 75, vit = 40, wis = 75 } }
        };
    }

    [System.Serializable]
    public class CharacterClass
    {
        public CharClasses charClass;
        public int bestFame;
        public int bestLevel;
    }

    [System.Serializable]
    public class CharacterStats
    {
        public CharClasses charClass;
        public int level;
        public int fame;
        public bool hasBackpack;
        public bool hasAdventurersBelt;

        public int hp;
        public int mp;

        public int maxHP;
        public int maxMP;

        public int spd;
        public int dex;
        public int atk;
        public int def;
        public int vit;
        public int wis;

        public int xOf8 = 0;

        public CharacterStats() { }
        public CharacterStats(string c)
        {
            try
            {
                c = c.Substring(c.IndexOf("<ObjectType>") + 12, c.Length - (c.IndexOf("<ObjectType>") + 12));
                charClass = CharacterClassesUtil.dicClasses[System.Convert.ToInt32(c.Substring(0, c.IndexOf("</ObjectType>")))];

                c = c.Substring(c.IndexOf("<Level>") + 7, c.Length - (c.IndexOf("<Level>") + 7));
                level = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</Level>")));

                c = c.Substring(c.IndexOf("<CurrentFame>") + 13, c.Length - (c.IndexOf("<CurrentFame>") + 13));
                fame = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</CurrentFame>")));

                c = c.Substring(c.IndexOf("<MaxHitPoints>") + 14, c.Length - (c.IndexOf("<MaxHitPoints>") + 14));
                maxHP = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</MaxHitPoints>")));

                c = c.Substring(c.IndexOf("<HitPoints>") + 11, c.Length - (c.IndexOf("<HitPoints>") + 11));
                hp = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</HitPoints>")));

                c = c.Substring(c.IndexOf("<MaxMagicPoints>") + 16, c.Length - (c.IndexOf("<MaxMagicPoints>") + 16));
                maxMP = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</MaxMagicPoints>")));

                c = c.Substring(c.IndexOf("<MagicPoints>") + 13, c.Length - (c.IndexOf("<MagicPoints>") + 13));
                mp = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</MagicPoints>")));

                c = c.Substring(c.IndexOf("<Attack>") + 8, c.Length - (c.IndexOf("<Attack>") + 8));
                atk = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</Attack>")));

                c = c.Substring(c.IndexOf("<Defense>") + 9, c.Length - (c.IndexOf("<Defense>") + 9));
                def = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</Defense>")));

                c = c.Substring(c.IndexOf("<Speed>") + 7, c.Length - (c.IndexOf("<Speed>") + 7));
                spd = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</Speed>")));

                c = c.Substring(c.IndexOf("<Dexterity>") + 11, c.Length - (c.IndexOf("<Dexterity>") + 11));
                dex = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</Dexterity>")));

                c = c.Substring(c.IndexOf("<HpRegen>") + 9, c.Length - (c.IndexOf("<HpRegen>") + 9));
                vit = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</HpRegen>")));

                c = c.Substring(c.IndexOf("<MpRegen>") + 9, c.Length - (c.IndexOf("<MpRegen>") + 9));
                wis = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</MpRegen>")));

                c = c.Substring(c.IndexOf("<HasBackpack>") + 13, c.Length - (c.IndexOf("<HasBackpack>") + 13));
                hasBackpack = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</HasBackpack>"))) == 1;

                c = c.Substring(c.IndexOf("<Has3Quickslots>") + 16, c.Length - (c.IndexOf("<Has3Quickslots>") + 16));
                hasAdventurersBelt = System.Convert.ToInt32(c.Substring(0, c.IndexOf("</Has3Quickslots>"))) == 1;

                CalculateXof8();
            }
            catch { }
        }

        public void CalculateXof8()
        {
            xOf8 = 0;
            try
            {
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].maxHP <= maxHP) ? 1 : 0;
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].maxMP <= maxMP) ? 1 : 0;
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].atk <= atk) ? 1 : 0;
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].def <= def) ? 1 : 0;
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].spd <= spd) ? 1 : 0;
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].dex <= dex) ? 1 : 0;
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].vit <= vit) ? 1 : 0;
                xOf8 += (CharacterClassesUtil.dicCharClassToMaxStats[charClass].wis <= wis) ? 1 : 0;
            }
            catch { }
        }
    }
    [System.Serializable]
    public enum CharClasses
    {
        Rogue,
        Archer,
        Wizard,
        Priest,
        Warrior,
        Knight,
        Paladin,
        Assassin,
        Necromancer,
        Huntress,
        Mystic,
        Trickster,
        Sorcerer,
        Ninja,
        Samurai,
        Bard,
        Summoner
    }
}
