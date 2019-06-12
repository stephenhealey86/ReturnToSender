using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReturnToSender.Models
{
    class HttpServer
    {
        #region Private Variables
        private HttpListener httpListener;
        private HttpListenerContext context;
        private HttpListenerRequest request;
        private HttpListenerResponse response;
        private byte[] buffer;
        System.IO.Stream output;
        #endregion

        #region Public Variables
        public bool Stop { get; set; } = false;
        #endregion

        #region Constructors
        public HttpServer(string Uri)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(Uri);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the Http server
        /// </summary>
        /// <returns>True if has atleast one Uri</returns>
        public async Task Start()
        {
            if (httpListener.Prefixes.Count > 0)
            {
                // Start the Http Listener
                httpListener.Start();
                while (!Stop)
                {
                    // Wait for Http request
                    context = await httpListener.GetContextAsync();
                    request = context.Request;
                    // Obtain a response object.
                    response = context.Response;
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // Close the output stream.
                    output.Close();
                }
                // Stop the Http Listener
                httpListener.Stop();
            }
        }

        public void AddResponse(string res)
        {
            buffer = Encoding.UTF8.GetBytes(res);
        }
        #endregion

        #region Helper Methods

        #endregion
    }
}
