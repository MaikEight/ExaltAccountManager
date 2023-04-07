
namespace MK_EAM_General_Services_Lib.General.Responses
{
    [System.Serializable]
    public sealed class GetFileResponse
    {
        public string fileName { get; set; }
        public byte[] data { get; set; }
    }
}
