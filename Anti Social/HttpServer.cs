using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Windows;

namespace SocialSilence
{
   public class HttpServer:IDisposable
    {
        private readonly HttpListener _listener;
        private readonly Thread _listenerThread;
        private readonly Thread[] _workers;
        private readonly ManualResetEvent _stop, _ready;
        private Queue<HttpListenerContext> _queue;


        public HttpServer(int MaxThreads)
        {
            _workers = new Thread[MaxThreads];
            _queue = new Queue<HttpListenerContext>();
            _stop = new ManualResetEvent(false);
            _ready = new ManualResetEvent(false);
            _listener = new HttpListener();
            _listenerThread = new Thread(HandleRequests);
        }

        private void HandleRequests()
        {
            while (_listener.IsListening)
            {

                var context = _listener.BeginGetContext(new AsyncCallback(GetContextCallBack), _listener);

                if (0 == WaitHandle.WaitAny(new WaitHandle[] { _stop, context.AsyncWaitHandle }))
                {
                    return;
                }
            }
        }

        private void GetContextCallBack(IAsyncResult result)
        {
            try
            {
                lock(_queue)
                {
                    _queue.Enqueue(_listener.EndGetContext(result));                   
                    _ready.Set();
                 }
            }
            catch
            { return; }
            
        }

        public void Start(int port)
        {
            try
            {
                _listener.Prefixes.Add(String.Format(@"http://+:{0}/", port));
                _listener.Start();
                _listenerThread.Start();

                for (int i = 0; i < _workers.Length; i++)
                {
                    _workers[i] = new Thread(Worker);
                    _workers[i].Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Worker(object obj)
        {
            WaitHandle[] wait = new[] { _ready, _stop };
            while (0 == WaitHandle.WaitAny(wait))
            {
                HttpListenerContext context;
                lock (_queue)
                {
                    if (_queue.Count > 0)
                        context = _queue.Dequeue();
                    else
                    {
                        _ready.Reset();
                        continue;
                    }
                }

                try
                {
                    ProcessRequest(context);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                }

            }
        }

        public void Dispose()
        {
            Stop();
        }

        private void Stop()
        {
            try
            {
                _stop.Set();
                _listenerThread.Join();
                foreach (Thread worker in _workers)
                {
                    worker.Join();
                }
               // App.Current.MainWindow.Close();
                _listener.Stop();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }


        public void  ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            System.Text.StringBuilder sb = new StringBuilder();

            //sb.Append("");
            //sb.Append(string.Format("HttpMethod: {0}", request.HttpMethod));
            //sb.Append(string.Format("Uri:        {0}", request.Url.AbsoluteUri));
           // sb.Append(string.Format("LocalPath:  {0}", request.Url.LocalPath));
           // foreach (string key in request.QueryString.Keys)
          //  {
          //      sb.Append(string.Format("Query:      {0} = {1}", key, request.QueryString[key]));
          //  }

          //  sb.Append("");
            sb.Append("This site has been Blocked Using DaksaTech Social Silence. Please check the About Us section for more Products");
            string responseString = sb.ToString();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.StatusCode = (int)HttpStatusCode.OK;
            using (System.IO.Stream outputStream = response.OutputStream)
            {
                outputStream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
