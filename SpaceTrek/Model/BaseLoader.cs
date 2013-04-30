using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SpaceTrek.Model
{
    public class BaseLoader : BaseObject
    {
        public bool IsRefresh {get;set;}

        private bool _IsLoad;
        public bool IsLoad
        {
            get { return _IsLoad; }
            set { _IsLoad = value; NotifyPropertyChanged("IsLoad"); }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; NotifyPropertyChanged("IsBusy"); }
        }


        private bool _IsError;
        public bool IsError
        {
            get { return _IsError; }
            set { _IsError = value; NotifyPropertyChanged("IsError"); }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { _Message = value; NotifyPropertyChanged("Message"); }
        }

        public event EventHandler<DownloadStringCompletedEventArgs> OnDownloadRequestCompleted;
        public event EventHandler<UploadStringCompletedEventArgs> OnUploadRequestCompleted;

  

        public void ConstructRequest(Uri uri, string method, string postData, object userToken,string username=null,string password=null)
        {
            WebClient client = new WebClient();
            if (username != null && password != null)
            {
                client.Credentials = new NetworkCredential(username, password);
            }
            if (method == "POST")
            {
                client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Encoding = Encoding.UTF8;
                client.UploadStringAsync(uri, method, postData, userToken);
            }
            else if (method == "GET")
            {
                client.DownloadStringCompleted += client_DownloadStringCompleted;
                client.DownloadStringAsync(uri, userToken);

            }
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (OnDownloadRequestCompleted != null)
                OnDownloadRequestCompleted(sender, e);
        }

        void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (OnUploadRequestCompleted != null)
                OnUploadRequestCompleted(sender, e);
        }

        public string ConstructQueryString(IDictionary<string, object> parameters)
        {
            string query = String.Empty;
            if (parameters != null)
            {
                int i = 0;
                foreach (var x in parameters)
                {
                    if (i == parameters.Count - 1)
                        query += String.Format("{0}={1}", x.Key, x.Value);
                    else
                        query += String.Format("{0}={1}&", x.Key, x.Value);
                    i++;
                }
            }
            query += String.Format("&{0}={1}", "timestamp", DateTime.UtcNow.Ticks.ToString());

            return query;
        }
    }
}
