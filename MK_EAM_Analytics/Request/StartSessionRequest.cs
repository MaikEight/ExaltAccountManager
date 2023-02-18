using System;

namespace MK_EAM_Analytics.Request
{
    [System.Serializable]
    public sealed class StartSessionRequest
    {
        public string clientIdHash { get; set; }
        public Version clientVersion { get; set; }
        public int amountOfAccounts { get; set; }
    }
}
