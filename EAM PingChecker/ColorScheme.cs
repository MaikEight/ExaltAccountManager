using System.Drawing;

namespace EAM_PingChecker
{
    public static class ColorScheme
    {
        public static Color GetColorDef(bool useDarkmode) => useDarkmode ? Color.FromArgb(32, 32, 32) : Color.FromArgb(255, 255, 255);
        public static Color GetColorSecond(bool useDarkmode) => useDarkmode ? Color.FromArgb(23, 23, 23) : Color.FromArgb(250, 250, 250);
        public static Color GetColorThird(bool useDarkmode) => useDarkmode ? Color.FromArgb(0, 0, 0) : Color.FromArgb(230, 230, 230);
        public static Color GetColorFont(bool useDarkmode) => useDarkmode ? Color.White : Color.Black;
    }
}
