using System;
using System.Collections.Generic;
using System.Text;

namespace MK_EAM_General_Services_Lib.General.Requests
{
    [System.Serializable]
    public class SetDiscordUserIdRequest
    {
        public string DiscordUserId { get; set; }
        public string ClientIdHash { get; set; }
    }
}
