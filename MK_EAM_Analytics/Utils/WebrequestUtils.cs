using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Analytics.Utils
{
    public static class WebrequestUtils
    {
        public static async Task<HttpResponseMessage> SendPostRequest(string url, object data)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback  = (message, cert, chain, errors) => { return true; }
            };
            using (var client = new System.Net.Http.HttpClient(handler) )
            {                
                var content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                return await client.PostAsync(url, content);
            }
        }
    }
}
