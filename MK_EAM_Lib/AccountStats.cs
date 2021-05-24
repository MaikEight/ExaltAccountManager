namespace MK_EAM_Lib
{
    [System.Serializable]
    public class AccountStats
    {
        public string email;
        public System.Collections.Generic.List<CharacterClass> classes;
        public int bestCharFame;
        public int totalFame;
        public int fame;
        public System.DateTime date;

        public AccountStats() { date = System.DateTime.Now; }
        public AccountStats(string _email, string request)
        {
            email = _email;
            classes = new System.Collections.Generic.List<CharacterClass>();
            date = System.DateTime.Now;

            if (request.Contains("<Stats>"))
            {
                string[] chars = new string[0];
                try
                {
                    request = request.Substring(request.IndexOf("<Stats>") + 7, request.Length - (request.IndexOf("<Stats>") + 7)).Trim();
                    request = request.Substring(0, request.IndexOf("</Stats>"));
                    chars = request.Split(new string[] { "</ClassStats>" }, System.StringSplitOptions.None);
                }
                catch { }               

                for (int i = 0; i < chars.Length; i++)
                {
                    try
                    {
                        string[] lines = System.Text.RegularExpressions.Regex.Split(chars[i].Trim(), "><");
                        if (lines[0].Contains("<BestCharFame>")) //Last set with other data
                        {
                            bestCharFame = System.Convert.ToInt32(lines[0].Substring(lines[0].IndexOf("<BestCharFame>") + 14, lines[0].IndexOf("</BestCharFame") - 14));
                            totalFame = System.Convert.ToInt32(lines[1].Substring(lines[1].IndexOf("TotalFame>") + 10, lines[1].IndexOf("</TotalFame") - 10));
                            fame = System.Convert.ToInt32(lines[2].Substring(lines[2].IndexOf("Fame>") + 5, lines[2].IndexOf("</Fame>") - 5));
                        }
                        else
                        {
                            lines[0] = lines[0].Substring(lines[0].IndexOf("=\"") + 2, lines[0].Length - (lines[0].IndexOf("=\"") + 2));
                            int key = System.Convert.ToInt32(lines[0].Substring(0, lines[0].Length - 1), 16);
                            int lvl = System.Convert.ToInt32(lines[1].Substring(lines[1].IndexOf("BestLevel>") + 10, lines[1].IndexOf("</BestLevel") - 10));
                            int fam = System.Convert.ToInt32(lines[2].Substring(lines[2].IndexOf("BestFame>") + 9, lines[2].IndexOf("</BestFame>") - 9));
                            classes.Add(new CharacterClass() { charClass = CharacterClassesUtil.dicClasses[key], bestLevel = lvl, bestFame = fam });
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
