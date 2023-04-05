using MK_EAM_General_Services_Lib.News.Data;
using System.Collections.Generic;

namespace MK_EAM_General_Services_Lib.News.Responses
{
    public sealed class GetNewsResponse
    {
        public List<NewsData> News { get; set; }
    }
}
