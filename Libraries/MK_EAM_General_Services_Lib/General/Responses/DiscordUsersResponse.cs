using System.Collections.Generic;

namespace MK_EAM_General_Services_Lib.General.Responses
{
    [System.Serializable]
    public class DiscordUsersResponse
    {
        public List<DiscordUser> Users { get; set; }
        public List<string> FailedUserIds { get; set; }
    }
}
