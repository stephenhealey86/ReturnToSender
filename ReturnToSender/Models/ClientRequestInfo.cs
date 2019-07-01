using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReturnToSender.Models
{
    public class ClientRequestInfo
    {
        #region Private Variables
        private string date;
        private string time;
        private string clientIpAddress;
        private string clientPort;
        private string keepAlive;
        private string httpMethod;
        private string contentType;
        private string headers;
        private string body;
        #endregion
        #region Public Variables
        public string Date
        {
            get { return $"Date:\t{date}"; }
            set { date = value; }
        }
        public string Time
        {
            get { return $"Time:\t{time}"; }
            set { time = value; }
        }
        public string ClientIpAddress
        {
            get { return $"IP Address:\t{clientIpAddress}"; }
            set { clientIpAddress = value; }
        }
        public string ClientPort
        {
            get { return $"Port:\t{clientPort}"; }
            set { clientPort = value; }
        }
        public string KeepAlive
        {
            get { return $"Keep Alive:\t{keepAlive}"; }
            set { keepAlive = value; }
        }
        public string HttpMethod
        {
            get { return $"Http Method:\t{httpMethod}"; }
            set { httpMethod = value; }
        }
        public string ContentType
        {
            get { return $"Content Type:\t{contentType ?? "N/A"}"; }
            set { contentType = value; }
        }
        public string Headers
        {
            get { return $"Headers:\n\t{headers}"; }
            set { headers = value.Replace("\n", "\n\t"); }
        }
        public string Body
        {
            get { body = body.Length > 0 ? "\n\t" + body : "\tN/A"; return $"Body:{body}"; }
            set { body = value.Replace("\n", "\n\t"); }
        }
        #endregion
    }
}
