
namespace MK_EAM_Analytics.Response
{
    [System.Serializable]
    public class GetUsersWithSessionsResponse
    {
        public System.Guid UserId { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public System.Collections.Generic.List<Data.User> Users { get; set; }
    }
}
