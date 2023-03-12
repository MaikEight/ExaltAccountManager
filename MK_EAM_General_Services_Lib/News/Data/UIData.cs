using System.Drawing;

namespace MK_EAM_General_Services_Lib.News.Data
{
    [System.Serializable]
    public class UIData
    {
        
    }

    [System.Serializable]
    public sealed class TextUIData : UIData
    {
        public string Text { get; set; }
    }
    [System.Serializable]
    public sealed class ImageUIData : UIData
    {
        public string ImageUrl { get; set; }
        public Size MaxSize { get; set; } = new Size(0, 0);
        public Size MinSize { get; set; } = new Size(0,0);
        /// <summary>
        /// PictureBoxSizeModes:
        /// 0 Normal;
        /// 1 StretchImage;
        /// 2 AutoSize;
        /// 3 CenterImage;
        /// 4 Zoom;
        /// </summary>
        public int PictureBoxSizeMode { get; set; } = 4;
    }
    [System.Serializable]
    public sealed class PollUIData : UIData
    {
        public string Headline { get; set; }
        public PollData PollData { get; set; }
        public string[] EntrieTexts { get; set; }
        public string[] EntrieImageUrls { get; set; }
    }
}
