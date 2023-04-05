using System;

namespace MK_EAM_General_Services_Lib.News.Requests
{
    [System.Serializable]
    public sealed class PostNewsPollRequest
    {
        public Guid PollId { get; set; }
        public int EntryId { get; set; }
        public string ClientIdHash { get; set; }
    }
}
