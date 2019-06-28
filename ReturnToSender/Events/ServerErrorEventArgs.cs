using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.Events
{
    public class ServerErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
        public ServerErrorEventArgs(string message)
        {
            ErrorMessage = message;
        }
    }
}
