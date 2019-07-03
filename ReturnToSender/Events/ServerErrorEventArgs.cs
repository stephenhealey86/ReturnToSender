using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.Events
{
    /// <summary>
    /// Server error event args class for passing error messages to viewmodel
    /// </summary>
    public class ServerErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Error message to pass to the viewmodel
        /// </summary>
        public string ErrorMessage { get; set; } = "";
        /// <summary>
        /// Constructor, takes error message as string
        /// </summary>
        /// <param name="message"></param>
        public ServerErrorEventArgs(string message)
        {
            ErrorMessage = message;
        }
    }
}
