using System.Diagnostics;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    public static class PingServer
    {
        const int pingPort = 2050;

        public static async Task<int> PingServerInfo(ServerData server)
        {
            try
            {
                using (System.Net.Sockets.TcpClient c = new System.Net.Sockets.TcpClient())
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    c.Connect(server.ip, pingPort);
                    watch.Stop();
                    c.Close();
                    return (watch.ElapsedMilliseconds > 2000 ? -1 : (int)watch.ElapsedMilliseconds);
                }
            }
            catch
            {
                return - 1;
            }
        }        
    }
}
