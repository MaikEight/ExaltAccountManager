using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Analytics.Utils
{
    public static class WebrequestUtils
    {    
        private static bool UseHandler(string url) => url.StartsWith("https://localhost");

        public static async Task<HttpResponseMessage> SendPostRequest(string url, object data)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };

            using (HttpClient client = UseHandler(url) ? new HttpClient(handler) : new HttpClient())
            {                
                string json = JsonConvert.SerializeObject(data);
                var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                return await client.PostAsync(url, content);
            }
        }

        public static async Task<HttpResponseMessage> SendGetRequest(string url)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };

            using (HttpClient client = UseHandler(url) ? new HttpClient(handler) : new HttpClient())
            {
                return await client.GetAsync(url);
            }
        }

        public static async Task<HttpResponseMessage> SendDeleteRequest(string url, object data)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };

            using (HttpClient client = UseHandler(url) ? new HttpClient(handler) : new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                string json = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return await client.SendAsync(request);
            }
        }

        public static async Task<HttpResponseMessage> SendPatchRequest(string url, object data)
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };

            using (HttpClient client = UseHandler(url) ? new HttpClient(handler) : new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
                string json = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return await client.SendAsync(request);
            }
        }
    }
}
