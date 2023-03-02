using MK_EAM_Analytics.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MK_EAM_Analytics.Request;
using MK_EAM_Analytics.Response.Data;
using System.Security.Cryptography;
using System.Linq;

namespace MK_EAM_Analytics
{
    public class AnalyticsClient
    {
        public static AnalyticsClient Instance { get; private set; }

        public Guid SessionId { get; private set; } = Guid.Parse("45414D00-0000-0000-0000-004D61696B38");
        public bool NewVersionAvailable { get; private set; }
        public string BASE_URL { get; private set; } = "https://localhost:5001/v1/Analytics"; //https://api.exalt-account-manager.eu:44322/v1/Analytics

        public AnalyticsClient(string baseUrl)
        {
            if (AnalyticsClient.Instance == null)
            {
                AnalyticsClient.Instance = this;
                if (!string.IsNullOrEmpty(baseUrl))
                    AnalyticsClient.Instance.BASE_URL = baseUrl;
                return;
            }
            throw new NotSupportedException("More than one instance of AnalyticsClient detected!");
        }

        public async Task<bool> StartSession(int amountOfAccounts, string clientIdHash, Version clientVersion)
        {
            #region StartSession

            if (SessionId == Guid.Empty)
                return false;

            MK_EAM_Analytics.Request.StartSessionRequest req = new MK_EAM_Analytics.Request.StartSessionRequest()
            {
                AmountOfAccounts = amountOfAccounts,
                ClientIdHash = clientIdHash,
                ClientVersion = clientVersion,
            };            

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "/session/start", req);

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.Created)
                {
                    StartSessionResponse ssr = JsonConvert.DeserializeObject<StartSessionResponse>(await responseMessage.Content.ReadAsStringAsync());
                    SessionId = ssr.SessionId;
                    NewVersionAvailable = ssr.NewVersionAvailable;
                    return true;
                }
            }
            catch { }

            SessionId = Guid.Empty;
            return false;

            #endregion
        }

        public async Task<bool> EndSeesion()
        {
            #region EndSession

            if (SessionId == Guid.Empty)
                return true;

            MK_EAM_Analytics.Request.EndSessionRequest req = new MK_EAM_Analytics.Request.EndSessionRequest()
            {
                SessionId = SessionId,
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "/session/end", req);
                await resp;
                HttpResponseMessage responseMessage = resp.Result;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.Accepted)
                {
                    SessionId = Guid.Empty;
                    return true;
                }
            }
            catch { }

            return false;

            #endregion
        }

        public async Task<bool> UpdateLlamaFound()
        {
            #region UpdateLlamaFound

            if (SessionId == Guid.Empty)
                return false;

            MK_EAM_Analytics.Request.LlamaFoundRequest req = new MK_EAM_Analytics.Request.LlamaFoundRequest()
            {
                SessionId = SessionId,
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "/llama", req);
                await resp;
                HttpResponseMessage responseMessage = resp.Result;
                responseMessage.EnsureSuccessStatusCode();
                return responseMessage.StatusCode == HttpStatusCode.Accepted;
            }
            catch { }
            return false;

            #endregion
        }

        public async Task<bool> UpdateAmountOfAccounts(int amountOfAccounts)
        {
            #region UpdateAmountOfAccounts

            if (SessionId == Guid.Empty)
                return false;

            MK_EAM_Analytics.Request.AmountOfAccountsRequest req = new Request.AmountOfAccountsRequest()
            {
                SessionId = SessionId,
                AmountOfAccounts = amountOfAccounts
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "/amountOfAccounts", req);
                await resp;
                HttpResponseMessage responseMessage = resp.Result;
                responseMessage.EnsureSuccessStatusCode();
                return responseMessage.StatusCode == HttpStatusCode.Accepted;
            }
            catch { }
            return false;

            #endregion
        }

        public async Task<bool> AddLogin(string hashOfMail, string serverName)
        {
            #region AddLogin

            if (SessionId == Guid.Empty)
                return false;

            MK_EAM_Analytics.Request.OpenGameRequest req = new Request.OpenGameRequest()
            {
                SessionId = SessionId,
                HashOfMail = hashOfMail,
                ServerName = serverName
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "/login", req);
                await resp;
                HttpResponseMessage responseMessage = resp.Result;
                responseMessage.EnsureSuccessStatusCode();
                return responseMessage.StatusCode == HttpStatusCode.Accepted;
            }
            catch { }
            return false;

            #endregion
        }

        public async Task<bool> AddModuleOpened(ModuleType moduleType)
        {
            #region AddModuleOpened

            if (SessionId == Guid.Empty)
                return false;

            MK_EAM_Analytics.Request.OpenModuleRequest req = new Request.OpenModuleRequest()
            {
                SessionId = SessionId,
                ModuleType = moduleType
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "/module", req);
                await resp;
                HttpResponseMessage responseMessage = resp.Result;
                responseMessage.EnsureSuccessStatusCode();
                return responseMessage.StatusCode == HttpStatusCode.Accepted;
            }
            catch { }
            return false;

            #endregion
        }

        public async Task<bool> DeleteUser(string clientIdHash)
        {
            #region DeleteUser

            if (SessionId == Guid.Empty)
                return false;

            MK_EAM_Analytics.Request.DeleteUserDataRequest req = new Request.DeleteUserDataRequest()
            {
                ClientIdHash = clientIdHash,
                SessionId = SessionId
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendDeleteRequest(BASE_URL + "/user", req);

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();

                return responseMessage.StatusCode == HttpStatusCode.OK;
            }
            catch { }

            return false;

            #endregion
        }

        public async Task<Response.Data.User> GetUserData(string clientIdHash)
        {
            #region GetUserData

            if (SessionId == Guid.Empty)
                return null;

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendGetRequest(BASE_URL + "/user?clientIdHash=" + clientIdHash + "&sessionId=" + SessionId.ToString());

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Response.Data.User>(await responseMessage.Content.ReadAsStringAsync());
                }

                return null;
            }
            catch { }

            return null;

            #endregion
        }
    }
}
