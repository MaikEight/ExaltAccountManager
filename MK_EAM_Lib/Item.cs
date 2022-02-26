using System;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class Item
    {
        public int id = -1;
        public string name = string.Empty;
        public int slotType = -1;
        public ItemType itemType = ItemType.Empty;
        public int tier = -1;
        public int x = 0;
        public int y = 0;
        public int fameBonus = -1;
        public int feedPower = -1;
        public int bagType = -1;
        public bool soulbound = false;
        public int utSt = -1;

        public Item() { }

        public Item(string str)
        {
            try
            {
                string[] sp = System.Text.RegularExpressions.Regex.Split(str.Replace("[", "").Replace("]", ""), ",");
                sp[0] = sp[0].TrimStart(' ');
                string ids = sp[0].Substring(0, sp[0].IndexOf(":")).Trim('\'').Replace(" ", "");

                if (ids.Equals("-1"))
                    id = -1;
                else
                    id = Convert.ToInt32(ids);

                name = sp[0].Substring(sp[0].IndexOf(": ") + 2, sp[0].Length - (sp[0].IndexOf(": ") + 2)).Replace("\"", "").Trim('\'');
                slotType = int.Parse(sp[1].Trim(' '));
                itemType = Item.GetItemType(slotType);
                tier = int.Parse(sp[2].Trim(' '));
                x = int.Parse(sp[3].Trim(' '));
                y = int.Parse(sp[4].Trim(' '));
                fameBonus = int.Parse(sp[5].Trim(' '));
                feedPower = int.Parse(sp[6].Trim(' '));
                bagType = int.Parse(sp[7].Trim(' '));
                soulbound = bool.Parse(sp[8].Trim(' '));
                utSt = int.Parse(sp[9].Trim(' '));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        public override string ToString()
        {
            return string.Format("ID:\t\t{1}{0}" +
                                 "Name:\t\t{2}{0}" +
                                 "SlotType:\t\t{3}{0}" +
                                 "Tier:\t\t{4}{0}" +
                                 "X:\t\t{5}{0}" +
                                 "Y:\t\t{6}{0}" +
                                 "FameBonus:\t{7}{0}" +
                                 "FeedPower:\t{8}{0}" +
                                 "BagType:\t{9}{0}" +
                                 "Soulbound:\t{10}{0}" +
                                 "UT/ST:\t\t{11}", Environment.NewLine, id, name, slotType, tier, x, y, fameBonus, feedPower, bagType, soulbound, (utSt == 0 ? "-" : utSt == 1 ? "UT" : "ST"));
        }

        public static ItemType GetItemType(Item item) => GetItemType(item.id);
        public static ItemType GetItemType(int id)
        {
            switch (id)
            {
                case -1:
                    return ItemType.Empty;
                case 0:
                    return ItemType.Unknown;
                case 1:
                    return ItemType.Sword;
                case 2:
                    return ItemType.Dagger;
                case 3:
                    return ItemType.Bow;
                case 4:
                    return ItemType.Tome;
                case 5:
                    return ItemType.Shield;
                case 6:
                    return ItemType.Light_Armor;
                case 7:
                    return ItemType.Heavy_Armor;
                case 8:
                    return ItemType.Wand;
                case 9:
                    return ItemType.Ring;
                case 10:
                    return ItemType.Item;
                case 11:
                    return ItemType.Spell;
                case 12:
                    return ItemType.Seal;
                case 13:
                    return ItemType.Cloak;
                case 14:
                    return ItemType.Robe;
                case 15:
                    return ItemType.Quiver;
                case 16:
                    return ItemType.Helm;
                case 17:
                    return ItemType.Staff;
                case 18:
                    return ItemType.Poison;
                case 19:
                    return ItemType.Skull;
                case 20:
                    return ItemType.Trap;
                case 21:
                    return ItemType.Orb;
                case 22:
                    return ItemType.Prism;
                case 23:
                    return ItemType.Scepter;
                case 24:
                    return ItemType.Katana;
                case 25:
                    return ItemType.Star;
                case 26:
                    return ItemType.Egg;
                case 27:
                    return ItemType.Wakizashi;
                case 28:
                    return ItemType.Lute;
                case 29:
                    return ItemType.Mace;
                default:
                    return ItemType.Unknown;
            }
        }
    }

    public enum ItemType
    {
        All,
        Unknown,
        Empty,
        Sword,
        Dagger,
        Bow,
        Tome,
        Shield,
        Light_Armor,
        Heavy_Armor,
        Wand,
        Ring,
        Item,
        Spell,
        Seal,
        Cloak,
        Robe,
        Quiver,
        Helm,
        Staff,
        Poison,
        Skull,
        Trap,
        Orb,
        Prism,
        Scepter,
        Katana,
        Star,
        Egg,
        Wakizashi,
        Lute,
        Mace
    }
}
