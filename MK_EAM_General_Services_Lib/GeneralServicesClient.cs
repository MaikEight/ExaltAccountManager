using MK_EAM_General_Services_Lib.News.Data;
using MK_EAM_General_Services_Lib.News.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MK_EAM_General_Services_Lib
{
    public sealed class GeneralServicesClient
    {
        public static GeneralServicesClient Instance { get; private set; }

        public string BASE_URL { get; private set; } = "https://localhost:5001/"; //https://api.exalt-account-manager.eu:44322/

        public GeneralServicesClient(string baseUrl)
        {
            if (GeneralServicesClient.Instance == null)
            {
                GeneralServicesClient.Instance = this;
                if (!string.IsNullOrEmpty(baseUrl))
                    GeneralServicesClient.Instance.BASE_URL = baseUrl;
                return;
            }
            throw new NotSupportedException("More than one instance of GeneralServicesClient detected!");
        }

        public async Task<List<NewsData>> GetNews(DateTime startTime, string clientIdHash, int amount = 5)
        {
            #region GetNews            

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendGetRequest(BASE_URL + $"v1/news?startTime={startTime}&amount={amount}&clientIdHash={clientIdHash}");

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    GetNewsResponse gnr = JsonConvert.DeserializeObject<GetNewsResponse>(await responseMessage.Content.ReadAsStringAsync(), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                    if (gnr != null && gnr.News != null)
                    {
                        return gnr.News;
                    }
                }
            }
            catch { }

            return new List<NewsData>();

            #endregion
        }

        public async Task<PollData> PostPoll(Guid pollId, int entryId, string clientIdHash)
        {
            #region PostPoll

            MK_EAM_General_Services_Lib.News.Requests.PostNewsPollRequest req = new News.Requests.PostNewsPollRequest()
            {
                ClientIdHash = clientIdHash,
                EntryId = entryId,
                PollId = pollId
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "v1/news/poll", req);

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.Accepted)
                {
                    PostNewResponse pnr = JsonConvert.DeserializeObject<PostNewResponse>(await responseMessage.Content.ReadAsStringAsync());

                    if (pnr != null && pnr.Success)
                    {
                        return pnr.PollData;
                    }
                    return null;
                }
            }
            catch { }

            return null;

            #endregion
        }
    }
}
