using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_General_Services_Lib.Utils
{
    public static class WebrequestUtils
    {
        //TODO: REMOVE HttpClientHandler and use a proper certificate

        public static async Task<HttpResponseMessage> SendPostRequest(string url, object data)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            using (var client = new HttpClient(handler))
            {
                string json = JsonConvert.SerializeObject(data);
                var content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
                return await client.PostAsync(url, content);
            }
        }

        public static async Task<HttpResponseMessage> SendGetRequest(string url)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            using (var client = new System.Net.Http.HttpClient(handler))
            {
                return await client.GetAsync(url);
            }
        }

        public static async Task<HttpResponseMessage> SendDeleteRequest(string url, object data)
        {
            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
            };
            using (var client = new System.Net.Http.HttpClient(handler))
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                string json = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return await client.SendAsync(request);
            }
        }
    }
}
