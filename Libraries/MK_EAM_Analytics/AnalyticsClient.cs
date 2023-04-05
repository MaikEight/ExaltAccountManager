using MK_EAM_Analytics.Response;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using MK_EAM_Analytics.Request;
using MK_EAM_Analytics.Data;
using System.Drawing;

namespace MK_EAM_Analytics
{
    public sealed class AnalyticsClient
    {
        public static AnalyticsClient Instance { get; private set; }

        public string BASE_URL { get; private set; } = "https://localhost:7066/v1/Analytics";
        public const int HEARTBEAT_INTERVAL_IN_MS = 59_000;

        public Guid SessionId { get; private set; } = Guid.Parse("45414D20-0000-6279-0000-204D61696B38");
        public bool NewVersionAvailable { get; private set; }

        private System.Timers.Timer timerHeartBeat = null;

        public AnalyticsClient(string baseUrl)
        {
            if (AnalyticsClient.Instance == null)
            {
                AnalyticsClient.Instance = this;
                if (!string.IsNullOrEmpty(baseUrl))
                    AnalyticsClient.Instance.BASE_URL = baseUrl;

                timerHeartBeat = new System.Timers.Timer(HEARTBEAT_INTERVAL_IN_MS);
                timerHeartBeat.Elapsed += (object sender, System.Timers.ElapsedEventArgs e) =>
                {
                    _ = HeartBeat();
                };
                timerHeartBeat.Start();

                return;
            }
            throw new NotSupportedException("More than one instance of AnalyticsClient detected!");
        }

        private async Task<bool> HeartBeat()
        {
            #region HeartBeat

            if (SessionId == Guid.Empty)
                return false;

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPatchRequest(BASE_URL + "/heartbeat", SessionId);

                HttpResponseMessage responseMessage = await resp;
                responseMessage.EnsureSuccessStatusCode();
                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                return false;
            }
            catch { }

            return false;

            #endregion
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

        public async Task<bool> AddCaptchaResult(DateTime startTime, CaptchaResult captchaResult, byte[] qimg, byte[] img, PointF[] data)
        {
            #region AddCaptchaResult

            if (SessionId == Guid.Empty)
                return false;

            MK_EAM_Analytics.Request.CaptchaResultRequest req = new Request.CaptchaResultRequest()
            {
               SessionId = SessionId,
               StartTime = startTime,
               CaptchaResult = captchaResult,
               qimg = qimg,
               img = img,
               data = data
            };

            try
            {
                Task<HttpResponseMessage> resp = Utils.WebrequestUtils.SendPostRequest(BASE_URL + "/captcha", req);
                await resp;
                HttpResponseMessage responseMessage = resp.Result;
                responseMessage.EnsureSuccessStatusCode();
                return responseMessage.StatusCode == HttpStatusCode.Accepted;
            }
            catch { }
            return false;

            #endregion
        }
    }
}
