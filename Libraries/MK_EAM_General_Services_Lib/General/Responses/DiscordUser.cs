using System;

namespace MK_EAM_General_Services_Lib.General.Responses
{
    [System.Serializable]
    public sealed class DiscordUser
    {
        public string DiscordUserId { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime FirstSeen { get; set; }
        public int AmountOfAccounts { get; set; }
        public int AmountOfSessions { get; set; }
        public int minutesOfEamUseTime { get; set; }
        public bool EasterEggFound { get; set; }
    }
}
