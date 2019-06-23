using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ReturnToSender.ValueConvertors
{
    class ColorConvertor : BaseValueConverter<ColorConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var brush = value as SolidColorBrush;
                var color = brush.Color;
                var para = parameter as String;
                var correctionFactor = float.Parse(para);
                float red = (float)color.R;
                float green = (float)color.G;
                float blue = (float)color.B;

                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;

                return new SolidColorBrush(Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
