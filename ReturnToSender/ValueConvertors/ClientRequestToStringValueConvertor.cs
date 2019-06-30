using ReturnToSender.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.ValueConvertors
{
    public class ClientRequestToStringValueConvertor : BaseValueConverter<ClientRequestToStringValueConvertor>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var clientRequest = value as ClientRequestInfo;
            var stringToReturn = new StringBuilder();
            if (clientRequest != null)
            {
                stringToReturn.AppendLine(clientRequest.ClientIpAddress);
                stringToReturn.AppendLine(clientRequest.ClientPort);
                stringToReturn.AppendLine(clientRequest.Headers);
                stringToReturn.AppendLine(clientRequest.HttpMethod);
                stringToReturn.AppendLine(clientRequest.ContentType);
                stringToReturn.AppendLine(clientRequest.Body);
            }
            return stringToReturn.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
