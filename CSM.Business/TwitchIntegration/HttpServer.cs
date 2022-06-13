using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Business.TwitchIntegration
{
    public sealed class HttpServer
    {
        #region Private fields

        private int port = 8080;
        private HttpListener listener;

        #endregion

        public event EventHandler<string> AccessTokenChanged;

        public void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{port}");
            listener.Start();
            Receive();
        }

        public void Stop()
        {
            listener.Stop();
        }

        #region Helper methods

        private void Receive()
        {
            listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (listener.IsListening)
            {
                var context = listener.EndGetContext(result);
                var request = context.Request;

                if (request.HttpMethod == "GET")
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "CSM.Business.TwitchIntegration.TwitchGetAccessToken.html";
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var responseString = reader.ReadToEnd();
                            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                            var response = context.Response;
                            response.ContentType = "text/html";
                            response.ContentLength64 = buffer.Length;
                            response.StatusCode = 200;
                            response.OutputStream.Write(buffer, 0, buffer.Length);
                            response.OutputStream.Close();
                        }
                    }
                }
                else if (request.HttpMethod == "POST")
                {
                    using (StreamReader streamReader = new StreamReader(context.Request.InputStream))
                    {
                        var content = streamReader.ReadToEnd();
                        if (!content.Contains("error"))
                        {
                            var accessToken = "";
                            AccessTokenChanged?.Invoke(this, accessToken);
                        }
                    }
                }

                Receive();
            }
        }

        #endregion

    }
}