using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.ValueConvertors
{
    class HttpMethodToIntValueConvertor : BaseValueConverter<HttpMethodToIntValueConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            var index = 0;
            switch (str)
            {
                case "POST":
                    index = 1;
                    break;
                case "PUT":
                    index = 2;
                    break;
                case "PATCH":
                    index = 3;
                    break;
                case "DELETE":
                    index = 4;
                    break;
            }
            return index;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = "GET";
            var index = (int)value;
            switch (index)
            {
                case 1:
                    str = "POST";
                    break;
                case 2:
                    str = "PUT";
                    break;
                case 3:
                    str = "PATCH";
                    break;
                case 4:
                    str = "DELETE";
                    break;
            }
            return str;
        }
    }
}
