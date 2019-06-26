using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.Models
{
    public static class HttpContentType
    {
        public static KeyValuePair<string,string> JSON = new KeyValuePair<string, string>("JSON", "application/json");
        public static KeyValuePair<string,string> Text = new KeyValuePair<string, string>("Text", "text/plain");
        public static KeyValuePair<string,string> JavaScript = new KeyValuePair<string, string>("JavaScript", "text/javascript");
        public static KeyValuePair<string,string> XML = new KeyValuePair<string, string>("XML", "text/xml");
        public static KeyValuePair<string,string> HTML = new KeyValuePair<string, string>("HTML", "text/html");
    }
}
