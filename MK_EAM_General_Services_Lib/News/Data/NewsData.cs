using System;
using System.Collections.Generic;

namespace MK_EAM_General_Services_Lib.News.Data
{
    [System.Serializable]
    public sealed class NewsData
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public int Importance { get; set; }

        public List<NewsEntry> newsEntries { get; set; }

        public NewsData()
        {
            newsEntries = new List<NewsEntry>();
        }

        public NewsData(Guid id, DateTime date, string title, int importance)
        {
            Id = id;
            Date = date;
            Title = title;
            Importance = importance;

            newsEntries = new List<NewsEntry>();
        }
    }

}
