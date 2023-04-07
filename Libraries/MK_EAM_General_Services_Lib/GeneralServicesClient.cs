using MK_EAM_General_Services_Lib.General.Responses;
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

        public string BASE_URL { get; private set; } = "https://localhost:5001/";

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

        public async Task<Version> GetLatestEamVersion()
        {
            #region GetLatestEamVersion            

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendGetRequest(BASE_URL + "v1/ExaltAccountManager/version");

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Version>(await responseMessage.Content.ReadAsStringAsync());
                }
            }
            catch { }

            return new Version(0, 0, 0);

            #endregion
        }

        public async Task<EAMReleaseInfoResponse> GetEamReleaseInfo()
        {
            #region GetEamReleaseInfo            

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendGetRequest(BASE_URL + "v1/ExaltAccountManager/release");

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {                    
                    return JsonConvert.DeserializeObject<EAMReleaseInfoResponse>(await responseMessage.Content.ReadAsStringAsync());
                }
            }
            catch { }

            return new EAMReleaseInfoResponse();

            #endregion
        }

        public async Task<GetVaultPeekerHashOfFilesResponse> GetVaultPeekerHashOfFiles()
        {
            #region GetVaultPeekerHashOfFiles

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendGetRequest(BASE_URL + "v1/VaultPeeker/GetHashOfFiles");

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    string json = await responseMessage.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetVaultPeekerHashOfFilesResponse>(json);
                }
            }
            catch { }

            return new GetVaultPeekerHashOfFilesResponse();

            #endregion
        }

        public async Task<GetFileResponse> GetVaultPeekerRendersPng()
        {
            #region GetVaultPeekerGetRendersPng

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendGetRequest(BASE_URL + "v1/VaultPeeker/GetRendersPng");

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<GetFileResponse>(await responseMessage.Content.ReadAsStringAsync());
                }
            }
            catch { }

            return new GetFileResponse();

            #endregion
        }

        public async Task<GetFileResponse> GetVaultPeekerItemsConfig()
        {
            #region GetVaultPeekerItemsConfig

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendGetRequest(BASE_URL + "v1/VaultPeeker/GetItemsConfig");

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<GetFileResponse>(await responseMessage.Content.ReadAsStringAsync());
                }
            }
            catch { }

            return new GetFileResponse();

            #endregion
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
