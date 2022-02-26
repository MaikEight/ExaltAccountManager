using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class ItemsSaveFile
    {
        public List<AccountItems> accountItems = new List<AccountItems>();
    }

    [System.Serializable]
    public class AccountItems
    {
        public string name;
        public string email;
        public System.DateTime date;

        public List<SaveItem> vaultItems = new List<SaveItem>();
        public List<SaveItem> giftItems = new List<SaveItem>();
        public List<SaveItem> potionItems = new List<SaveItem>();

        public AccountItems() { }
        public AccountItems(string _name, string _email, string _charList)
        {
            try
            {
                name = _name;
                email = _email;
                date = System.DateTime.Now;

                string charList = _charList.Substring(_charList.IndexOf("<Vault>") + 7, _charList.Length - _charList.IndexOf("<Vault>") - 7);

                string vault = charList.Substring(0, charList.IndexOf("</Vault>"));
                string[] chests = Regex.Split(vault.Replace("<Chest>", ""), "</Chest>");

                for (int i = 0; i < chests.Length - 1; i++)
                {
                    string[] item = Regex.Split(chests[i], ",");
                    foreach (string it in item)
                    {
                        if (!string.IsNullOrWhiteSpace(it))
                            vaultItems.Add(new SaveItem(it));
                    }
                }

                if (charList.Contains("<Gifts>"))
                {
                    string gifts = charList.Substring(charList.IndexOf("<Gifts>") + 7, charList.Length - charList.IndexOf("<Gifts>") - 7);
                    gifts = gifts.Substring(0, gifts.IndexOf("</Gifts>"));
                    string[] item = Regex.Split(gifts, ",");
                    foreach (string it in item)
                    {
                        if (!string.IsNullOrWhiteSpace(it))
                            giftItems.Add(new SaveItem(it));
                    }
                }

                if (charList.Contains("<Potions>"))
                {
                    string pots = charList.Substring(charList.IndexOf("<Potions>") + 9, charList.Length - charList.IndexOf("<Potions>") - 9);
                    pots = pots.Substring(0, pots.IndexOf("</Potions>"));
                    string[] item = Regex.Split(pots, ",");
                    foreach (string it in item)
                    {
                        if (!string.IsNullOrWhiteSpace(it))
                            potionItems.Add(new SaveItem(it));
                    }
                }
            }
            catch
            {
                name = "FAILED";
            }
        }
    }

    [System.Serializable]
    public class SaveItem
    {
        public int id = -1;
        public string data = string.Empty;

        public SaveItem() { }
        public SaveItem(string itm)
        {
            try
            {
                if (itm.Contains("#"))
                {
                    data = itm.Substring(itm.IndexOf('#'), itm.Length - itm.IndexOf('#'));
                    id = int.Parse(itm.Substring(0, itm.IndexOf('#')));
                }
                else
                {
                    id = int.Parse(itm);
                }
            }
            catch
            {

            }
        }
    }
}
