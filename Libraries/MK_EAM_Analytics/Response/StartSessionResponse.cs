using System;

namespace MK_EAM_Analytics.Response
{
    [System.Serializable]
    public sealed class StartSessionResponse
    {
        public Guid SessionId { get; set; }
        public bool NewVersionAvailable { get; set; }
    }
}
