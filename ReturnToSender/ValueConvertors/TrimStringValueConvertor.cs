using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.ValueConvertors
{
    public class TrimStringValueConvertor : BaseValueConverter<TrimStringValueConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (String)value;
            if (str != null)
            {
                str = str.Length > 8 ? str.Substring(0, 5) + "..." : str;
                return str;
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
