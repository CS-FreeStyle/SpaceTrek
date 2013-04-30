using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;

namespace SpaceTrek.Model
{
    public class ObjectStream : BaseObject
    {

        private int _id_stream;
        public int id_stream
        {
            get { return _id_stream; }
            set { _id_stream = value; NotifyPropertyChanged("id_stream"); }
        }

        private int _id_channel;
        public int id_channel
        {
            get { return _id_channel; }
            set { _id_channel = value; NotifyPropertyChanged("id_channel"); }
        }

        private int _sequence;
        public int sequence
        {
            get { return _sequence; }
            set { _sequence = value; NotifyPropertyChanged("sequence"); }
        }

        private string _file;
        public string file
        {
            get { return _file; }
            set { _file = value; NotifyPropertyChanged("file"); }
        }

        private BitmapImage _file_stream;
        public BitmapImage file_stream
        {
            get { return _file_stream; }
            set { _file_stream = value; NotifyPropertyChanged("file_stream"); }
        }

        public bool is_downloaded {get;set;}

        public void LoadImage()
        {
            WebClient webClient = new WebClient();

            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);

            webClient.OpenReadAsync(new Uri(file, UriKind.Absolute));

        }

        private int counter = 0;

        public event EventHandler<EventArgs> OnImageDownloadCompleted;

        private void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {

            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(e.Result);
                file_stream = bitmapImage;
                is_downloaded = true;
                if (OnImageDownloadCompleted != null)
                    OnImageDownloadCompleted(this, e);
            }
            catch (Exception ex)
            {
                is_downloaded = false;
                counter++;
                if (counter < 4)
                    LoadImage();
            }


           
        }


    }
}
