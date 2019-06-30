using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.Models
{
    public class ClientRequestInfo
    {
        #region Private Variables
        private string clientIpAddress;
        private string clientPort;
        private string httpMethod;
        private string contentType;
        private string headers;
        private string body;
        #endregion
        #region Public Variables
        public string ClientIpAddress
        {
            get { return $"IP Address: {clientIpAddress}"; }
            set { clientIpAddress = value; }
        }
        public string ClientPort
        {
            get { return $"Port: {clientPort}"; }
            set { clientPort = value; }
        }
        public string HttpMethod
        {
            get { return $"Http Method: {httpMethod}"; }
            set { httpMethod = value; }
        }
        public string ContentType
        {
            get { return $"Content Type: {contentType}"; }
            set { contentType = value; }
        }
        public string Headers
        {
            get { return $"Headers:\n\t{headers}"; }
            set { headers = value; }
        }
        public string Body
        {
            get { return $"Body:\n{body}"; }
            set { body = value; }
        }
        #endregion
    }
}
