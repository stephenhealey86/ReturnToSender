using System.Windows.Media;

namespace ReturnToSender.Models
{
    public static class ThemeTypes
    {
        public const string Default = "Default";
        public const string Dark = "Dark";
        public const string Planet = "Planet";

        public static Theme GetTheme(string str)
        {
            Theme theme = new Theme();
            switch (str)
            {
                case ThemeTypes.Dark:
                    theme.BackGround = ThemeDark.BackGround;
                    theme.ForeGround = ThemeDark.ForeGround;
                    break;
                case ThemeTypes.Planet:
                    theme.BackGround = ThemePlanet.BackGround;
                    theme.ForeGround = ThemePlanet.ForeGround;
                    break;
                default:
                    break;
            }
            return theme;
        }
    }

    public class Theme
    {
        public SolidColorBrush BackGround { get; set; } = ThemeDefault.BackGround;
        public SolidColorBrush ForeGround { get; set; } = ThemeDefault.ForeGround;
    }

    public static class ThemeDefault
    {
        public static SolidColorBrush BackGround => new SolidColorBrush(Colors.White);
        public static SolidColorBrush ForeGround => new SolidColorBrush(Colors.Gray);
    }

    public static class ThemeDark
    {
        public static SolidColorBrush BackGround => new SolidColorBrush(Color.FromRgb(128,128,128));
        public static SolidColorBrush ForeGround => new SolidColorBrush(Color.FromRgb(0, 0, 0));
    }

    public static class ThemePlanet
    {
        public static SolidColorBrush BackGround => new SolidColorBrush(Color.FromRgb(200, 200, 200));
        public static SolidColorBrush ForeGround => new SolidColorBrush(Color.FromRgb(128, 128, 128));
    }
}
