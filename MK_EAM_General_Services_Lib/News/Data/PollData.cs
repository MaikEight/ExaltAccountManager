using System;
using System.Collections.Generic;
using System.Text;

namespace MK_EAM_General_Services_Lib.News.Data
{
    [System.Serializable]
    public sealed class PollData
    {
        public Guid PollId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        
        public int EntriesAmount { get; set; }
        public int[] Entries { get; set; }
        public int OwnEntry { get; set; } = -1;
    }
}
