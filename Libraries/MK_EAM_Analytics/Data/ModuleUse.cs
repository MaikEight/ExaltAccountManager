
namespace MK_EAM_Analytics.Data
{
    [System.Serializable]
    public  class ModuleUse
    {
        public System.Guid SessionId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public System.DateTime Time { get; set; }
    }
}
