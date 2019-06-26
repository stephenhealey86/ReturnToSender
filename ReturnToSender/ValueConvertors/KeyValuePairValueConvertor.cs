using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.ValueConvertors
{
    class KeyValuePairValueConvertor : BaseValueConverter<KeyValuePairValueConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var keyValuePair = (KeyValuePair<string, string>)value;
            if (keyValuePair.Key != null && keyValuePair.Value != null)
            {
                return $"{keyValuePair.Key} - {keyValuePair.Value}";
            }
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
