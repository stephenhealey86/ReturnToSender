using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReturnToSender.Models
{
    public class HttpServer
    {

        #region Private Variables
        private HttpListener httpListener;
        private HttpListenerContext context;
        private HttpListenerRequest request;
        private HttpListenerResponse response;
        private byte[] buffer;
        private System.IO.Stream output;
        #endregion

        #region Public Variables
        /// <summary>
        /// Stops the http server if true
        /// </summary>
        public bool Stop { get; set; } = false;
        /// <summary>
        /// Returns true if server running
        /// </summary>
        public bool Started { get; set; } = false;
        /// <summary>
        /// The response from the http server
        /// </summary>
        public string Response { get; set; }
        /// <summary>
        /// The request Uri as a string for the http server
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// Indicator string for the start/stop button
        /// </summary>
        public string ButtonStatus => Started == true ? "Stop" : "Start";
        /// <summary>
        /// Indicator string for the verification button
        /// </summary>
        public string VerificationString { get; set; } = "JSON";
        #endregion

        #region Constructors
        public HttpServer()
        {
            httpListener = new HttpListener();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Starts the Http server
        /// </summary>
        public async Task Start()
        {
            // Ensure Request ends with a /
            var uri = Request.Trim('/') + ('/');
            httpListener.Prefixes.Add(uri);

            if (httpListener.Prefixes.Count > 0 && Response.Length  > 0)
            {
                // Start the Http Listener
                httpListener.Start();
                Started = true;
                while (!Stop)
                {
                    try
                    {
                        // Wait for Http request
                        context = await httpListener.GetContextAsync();
                        if (Stop)
                        {
                            break;
                        }
                        request = context.Request;
                        // Obtain a response object.
                        response = context.Response;
                        var one = request.Url.AbsoluteUri.ToLower().Trim('/');
                        var two = Request.ToLower().Trim('/');
                        if (!String.Equals(one, two) || one.Length != two.Length)
                        {
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            buffer = Encoding.UTF8.GetBytes("Not Found");
                        }
                        else
                        {
                            buffer = Encoding.UTF8.GetBytes(Response);
                        }
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        // Close the output stream.
                        output.Close();
                    }
                    catch (Exception e)
                    {
                        Started = false;
                        Stop = true;
                    }
                }
                // Stop the Http Listener
                httpListener.Stop();
                // Reset control bools
                Started = false;
                Stop = false;
            }
        }
        #endregion

        #region Helper Methods

        #endregion
    }
}
