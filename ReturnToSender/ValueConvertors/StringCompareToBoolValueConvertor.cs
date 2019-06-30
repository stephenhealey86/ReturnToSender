using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.ValueConvertors
{
    public class StringCompareToBoolValueConvertor : BaseValueConverter<StringCompareToBoolValueConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = (string)value;
            var strPara = (string)parameter;
            if (String.Compare(strValue, strPara) == 0)
            {
                return true;
            }
            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
