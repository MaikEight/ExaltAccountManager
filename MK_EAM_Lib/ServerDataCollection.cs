using System;
using System.Collections.Generic;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class ServerDataCollection
    {
        public DateTime collectionDate;
        public List<ServerData> servers;

        public static ServerDataCollection CreateNewCollection(string charList)
        {
            try
            {
                string[] servers = new string[0];
                charList = charList.Substring(charList.IndexOf("<Servers>") + 9, charList.Length - (charList.IndexOf("<Servers>") + 9)).Trim();
                charList = charList.Substring(0, charList.IndexOf("</Servers>"));

                servers = charList.Split(new string[] { "</Server>" }, System.StringSplitOptions.None);
                ServerDataCollection collection = new ServerDataCollection()
                {
                    collectionDate = DateTime.Now,
                    servers = new List<ServerData>()
                };
                for (int i = 0; i < servers.Length - 1; i++)
                {
                    try
                    {
                        string[] lines = System.Text.RegularExpressions.Regex.Split(servers[i].Trim(), "><");

                        ServerData data = new ServerData();
                        data.name = lines[1].Substring(lines[1].IndexOf("Name>") + 5, lines[1].IndexOf("</Name") - 5);
                        data.ip = lines[2].Substring(lines[2].IndexOf("DNS>") + 4, lines[2].IndexOf("</DNS") - 4);
                        data.usage = (int)(System.Convert.ToDouble(lines[5].Substring(lines[5].IndexOf("Usage>") + 6, lines[5].IndexOf("</Usage") - 6)));
                        collection.servers.Add(data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                return collection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new ServerDataCollection() { collectionDate = DateTime.Now, servers = new List<ServerData>() };
        }
    }
}
