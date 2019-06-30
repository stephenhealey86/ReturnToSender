using ReturnToSender.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        public string VerificationString { get; set; }
        /// <summary>
        /// Http method choosen by user
        /// </summary>
        public string HttpMethod { get; set; } = "GET";
        /// <summary>
        /// The Http response content type
        /// </summary>
        public string ContentType { get; set; } = "JSON - application/json";
        /// <summary>
        /// Event for the VM to subscribe to for passing error messages to the VM error handler
        /// </summary>
        public EventHandler<ServerErrorEventArgs> ServerErrorEvent;

        public EventHandler<EventArgs> ServerEvent;
        /// <summary>
        /// The content of the clients request
        /// </summary>
        public ClientRequestInfo ClientRequest { get; set; }
        #endregion

        #region Constructors
        public HttpServer()
        {
            httpListener = new HttpListener();
            ClientRequest = new ClientRequestInfo();
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

            // Ensure Response as value
            Response = Response ?? "{}";

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
                        response.ContentType = ContentType;
                        var uriPath = request.Url.AbsolutePath.ToLower().Trim('/');
                        if (!Request.ToLower().Trim('/').EndsWith(uriPath))
                        {
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            buffer = Encoding.UTF8.GetBytes("Not Found");
                        }
                        else if (request.HttpMethod == HttpMethod)
                        {
                            buffer = Encoding.UTF8.GetBytes(Response);
                            UpdateClientRequest(request);
                        }
                        else
                        {
                            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                            var message = $"Wrong Http Method, {HttpMethod} does not match {request.HttpMethod}";
                            buffer = Encoding.UTF8.GetBytes(message);
                            OnServerErrorEvent(new ServerErrorEventArgs(message));
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
                        OnServerErrorEvent(new ServerErrorEventArgs($"Internal Error: {e.Message}"));
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
        /// <summary>
        /// Raises an event that passes the error message to the viewmodel
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServerErrorEvent(ServerErrorEventArgs e)
        {
            ServerErrorEvent?.Invoke(this, e);
        }
        /// <summary>
        /// Raise event in http server so viewmodel can sunscribe
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnServerEvent(EventArgs e)
        {
            ServerEvent?.Invoke(this, e);
        }
        /// <summary>
        /// Updates the public var ClientRequest
        /// </summary>
        /// <param name="request"><see cref="HttpListenerRequest"/></param>
        private void UpdateClientRequest(HttpListenerRequest request)
        {
            ClientRequest = new ClientRequestInfo();
            ClientRequest.ClientIpAddress = request.RemoteEndPoint.Address.ToString();
            ClientRequest.ClientPort = request.RemoteEndPoint.Port.ToString();
            ClientRequest.HttpMethod = request.HttpMethod;
            ClientRequest.ContentType = request.ContentType;
            ClientRequest.Headers = request.Headers.ToString().Trim();
            ClientRequest.Body = new StreamReader(request.InputStream).ReadToEnd();

            OnServerEvent(new EventArgs());
        }
        #endregion
    }
}
