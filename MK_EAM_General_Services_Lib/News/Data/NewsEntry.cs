
using System;

namespace MK_EAM_General_Services_Lib.News.Data
{
    [System.Serializable]
    public sealed class NewsEntry
    {
        public Guid NewsId { get; set; }
        public int OrderId { get; set; }
        public int TypeId { get; set; }
        public NewsType Type { get => (NewsType)TypeId; }
        public UIData UiData { get; set; }
        public Guid PollId { get; set; }
    }

    public enum NewsType
    {
        Text = 0,
        Headline = 1,
        Image = 100,
        Poll = 200
    }
}
