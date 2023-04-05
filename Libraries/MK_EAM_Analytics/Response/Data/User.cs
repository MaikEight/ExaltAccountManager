using System;
using System.Collections.Generic;
using System.Text;

namespace MK_EAM_Analytics.Response.Data
{
    [System.Serializable]
    public class User
    {
        public MK_EAM_Analytics.Data.User UserData { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
