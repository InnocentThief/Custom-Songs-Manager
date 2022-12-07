using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace CSM.Business
{
    public class HttpServer
    {
        public int Port = 8080;

        private HttpListener listener;

        public void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Receive();
        }

        public void Stop()
        {
            listener.Stop();
        }

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
                    var responseString = "Custom Songs Manager";

                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "CSM.Business.TwitchIntegration.TwitchSucceeded.html";

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseString = reader.ReadToEnd();
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    var response = context.Response;
                    response.ContentType = "text/html";
                    response.ContentLength64 = buffer.Length;
                    response.StatusCode = 200;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.OutputStream.Close();
                }
                else if (request.HttpMethod == "POST")
                {
                    using (StreamReader streamReader = new StreamReader(context.Request.InputStream))
                    {
                        var blubber = streamReader.ReadToEnd();
                    }
                }






                Receive();
            }
        }
    }
}