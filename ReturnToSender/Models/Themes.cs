using System.Windows.Media;

namespace ReturnToSender.Models
{
    public static class ThemeTypes
    {
        public const string Theme = "Theme";
        public const string Default = "Default";
        public const string Dark = "Dark";
        public const string Planet = "Planet";
        public const string Transparent = "Transparent";

        public static Theme GetTheme(int num)
        {
            Theme theme = new Theme();
            switch (num)
            {
                case 1:
                    theme = ThemeDark.Theme;
                    break;
                case 2:
                    theme = ThemePlanet.Theme;
                    break;
                case 3:
                    theme = ThemeTransparent.Theme;
                    break;
                default:
                    theme = ThemeDefault.Theme;
                    break;
            }
            return theme;
        }
    }

    public class Theme
    {
        public SolidColorBrush BackGround { get; set; } = new SolidColorBrush(Colors.White);
        public SolidColorBrush ForeGround { get; set; } = new SolidColorBrush(Colors.Gray);
        public SolidColorBrush Brand { get; set; } = new SolidColorBrush(Color.FromRgb(128, 0, 128));
        public SolidColorBrush HoverBackGround { get; set; } = new SolidColorBrush(Colors.LightGray);
        public SolidColorBrush HoverForeGround { get; set; } = new SolidColorBrush(Colors.Black);
    }

    public static class ThemeDefault
    {
        public static Theme Theme => new Theme()
        {
            BackGround = new SolidColorBrush(Colors.White),
            ForeGround = new SolidColorBrush(Colors.Gray),
            Brand = new SolidColorBrush(Color.FromRgb(128, 0, 128)),
            HoverBackGround = new SolidColorBrush(Colors.LightGray),
            HoverForeGround = new SolidColorBrush(Colors.Black)
        };
    }

    public static class ThemeDark
    {
        public static Theme Theme => new Theme()
        {
            BackGround = new SolidColorBrush(Color.FromRgb(70, 70, 70)),
            ForeGround = new SolidColorBrush(Colors.White),
            Brand = new SolidColorBrush(Colors.White),
            HoverBackGround = new SolidColorBrush(Colors.LightGray),
            HoverForeGround = new SolidColorBrush(Colors.Black)
        };

    }

    public static class ThemePlanet
    {
        public static Theme Theme => new Theme()
        {
            BackGround = new SolidColorBrush(Color.FromRgb(0, 128, 255)),
            ForeGround = new SolidColorBrush(Colors.White),
            Brand = new SolidColorBrush(Colors.White),
            HoverBackGround = new SolidColorBrush(Colors.LightGray),
            HoverForeGround = new SolidColorBrush(Colors.Black)
        };
    }

    public static class ThemeTransparent
    {
        public static Theme Theme => new Theme()
        {
            BackGround = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255)),
            ForeGround = new SolidColorBrush(Colors.Gray),
            Brand = new SolidColorBrush(Color.FromRgb(128, 0, 128)),
            HoverBackGround = new SolidColorBrush(Colors.LightGray),
            HoverForeGround = new SolidColorBrush(Colors.Black)
        };
    }
}
