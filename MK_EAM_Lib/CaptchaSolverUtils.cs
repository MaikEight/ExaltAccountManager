using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    public static class CaptchaSolverUtils
    {
        public const string URL_CAPTCHA_REFRESH = "https://www.realmofthemadgod.com/captcha/refreshChallenge";

        public async static Task<CaptchaRefreshResponse> RequestChallenge(AccountInfo acc)
        {
            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "guid", acc.Email },
                { "password", acc.password },
                { "game_net", "Unity" },
                { "play_platform", "Unity" },
                { "game_net_user_id", "" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            try
            {
                Task<HttpResponseMessage> response = SendPostRequest(URL_CAPTCHA_REFRESH, content);
                HttpResponseMessage responseMessage = await response;
                responseMessage.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<CaptchaRefreshResponse>(await responseMessage.Content.ReadAsStringAsync());
            }
            catch { }
            return null;
        }       

        public async static Task<bool> SubmitSolution(AccountInfo acc, PointF[] points)
        {
            string data = GetDataFromPoints(points);
            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "guid", acc.Email },
                { "password", acc.password },
                { "data", data },
                { "game_net", "Unity" },
                { "play_platform", "Unity" },
                { "game_net_user_id", "" }
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            try
            {
                Task<HttpResponseMessage> response = SendPostRequest(URL_CAPTCHA_REFRESH, content);
                HttpResponseMessage responseMessage = await response;
                responseMessage.EnsureSuccessStatusCode();

                string responseString = await responseMessage.Content.ReadAsStringAsync();
                return responseString.Equals("<Success/>"); 
            }
            catch { }

            return false;
        }

        private static string GetDataFromPoints(PointF[] points)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[[");
            sb.Append(points[0].X.ToString("0.000").Replace(',', '.'));
            sb.Append(", ");
            sb.Append(points[0].Y.ToString("0.000").Replace(',', '.'));
            sb.Append("],[");
            sb.Append(points[1].X.ToString("0.000").Replace(',', '.'));
            sb.Append(", ");
            sb.Append(points[1].Y.ToString("0.000").Replace(',', '.'));
            sb.Append("],[");
            sb.Append(points[2].X.ToString("0.000").Replace(',', '.'));
            sb.Append(", ");
            sb.Append(points[2].Y.ToString("0.000").Replace(',', '.'));
            sb.Append("]]");
            
            return sb.ToString();
        }

        private static async Task<HttpResponseMessage> SendPostRequest(string url, FormUrlEncodedContent data)
        {     
            using (HttpClient client = new HttpClient())
            {                
                return await client.PostAsync(url, data);
            }
        }

        [System.Serializable]
        public class CaptchaRefreshResponse
        {
            public string qimg { get; set; }
            public string img { get; set; }
            public int submitsLeft { get; set; }
        }
    }
}
