using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
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
        System.IO.Stream output;
        #endregion

        #region Public Variables
        public bool Stop { get; set; } = false;
        public string Response { get; set; }
        public string Request { get; set; }
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
            httpListener.Prefixes.Add(Request);

            if (httpListener.Prefixes.Count > 0 && Response.Length  > 0)
            {
                // Start the Http Listener
                httpListener.Start();
                while (!Stop)
                {
                    try
                    {
                        // Wait for Http request
                        context = await httpListener.GetContextAsync();
                        request = context.Request;
                        // Obtain a response object.
                        response = context.Response;
                        var one = request.RawUrl.ToLower().Trim('/');
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

                        throw;
                    }
                }
                // Stop the Http Listener
                httpListener.Stop();
            }
        }
        #endregion

        #region Helper Methods

        #endregion
    }
}
