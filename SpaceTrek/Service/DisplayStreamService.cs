using Newtonsoft.Json;
using SpaceTrek.Helper;
using SpaceTrek.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace SpaceTrek.Service
{
    /// <summary>
    /// Helper class to display stream of object
    /// </summary>
    public class DisplayStreamService : BaseLoader
    {
        public List<ObjectStream> SpaceObjectStreamCollection { get; set; }
        public SpaceObject CurrentSpaceObject { get; set; }
        public SpaceChannel CurrentSpaceChannel { get; set; }

        public int stream_iterator {get;set;}

        private int Attempt = 0;

  

        public void LoadBitmap(ref BitmapImage outputImage)
        {
            try
            {
                if (stream_iterator < SpaceObjectStreamCollection.Count - 10)
                {
                    outputImage = SpaceObjectStreamCollection[stream_iterator].file_stream;
                }
                else
                {
                    Load();
                }
                stream_iterator++;
            }
            catch { }
        }

        public void Reset()
        { 
            stream_iterator = 0;
        }

        public void Replay()
        {
            Reset();
        }

        public DisplayStreamService()
        {
            stream_iterator = 0;
            SpaceObjectStreamCollection = new List<ObjectStream>();
            this.OnDownloadRequestCompleted += DisplayStreamService_OnDownloadRequestCompleted;
        }

        void Insert(List<ObjectStream> streams)
        {
         
            for (int i = 0; i < streams.Count; i++)
            {
                if (SpaceObjectStreamCollection.Where(item => item.sequence == streams[i].sequence).Count() == 0)
                {
                    if (streams[i].sequence < SpaceObjectStreamCollection.Count)
                        SpaceObjectStreamCollection.Insert(streams[i].sequence, streams[i]);
                    else
                        SpaceObjectStreamCollection.Add(streams[i]);
                    streams[i].OnImageDownloadCompleted += DisplayStreamService_OnImageDownloadCompleted;
                    streams[i].LoadImage();
                }
            }
        }

        int counter = 0;
        bool is_active = false;

        public event EventHandler<EventArgs> OnReadyToPlay;

        void DisplayStreamService_OnImageDownloadCompleted(object sender, EventArgs e)
        {
            counter++;
            if (counter < (int)(0.3* SpaceObjectStreamCollection.Count))
            {

            }
                //PLAY
            else {
                if (!is_active)
                {
                    if (OnReadyToPlay != null)
                        OnReadyToPlay(this, e);
                }
               
            }
        }

        void DisplayStreamService_OnDownloadRequestCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    StreamDataRootObject data = JsonConvert.DeserializeObject<StreamDataRootObject>(e.Result);
                    Insert(data.stream);
                }
                else { 
                
                }
            }
            catch (Exception ex) { 
            
            
            }
        }

        public void Load(int from_id = 0)
        { 
            Uri uri = new Uri(string.Format(EndpointHelper.CHANNEL_DOWNLOAD,CurrentSpaceChannel.id_channel));

            ConstructRequest(uri,"GET",null,Attempt);
        }

        public void InitCurrentSpaceChannel(SpaceChannel spc)
        {
            CurrentSpaceChannel = spc;
        }

        public void InitCurrentSpaceObject(SpaceObject obj)
        {
            CurrentSpaceObject = obj;
        }



        internal void Display()
        {
            Load();
        }
    }
}
