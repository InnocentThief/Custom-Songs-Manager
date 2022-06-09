using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSM.Business
{
    public class HttpServer
    {
        public int Port = 8080;

        private HttpListener listener;

        public void Start()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:7681/");
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

                // do something with the request
                Console.WriteLine($"{request.Url}");

                Receive();
            }
        }
    }
}
