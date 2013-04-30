using Newtonsoft.Json;
using SpaceTrek.Helper;
using SpaceTrek.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace SpaceTrek
{
    public class SpaceObject : BaseLoader
    {

        public event EventHandler<ChannelUserEventArgs> OnChannelCreationCompleted;
        
        private string _image;
        public string image
        {
            get { return _image; }
            set { _image = value; NotifyPropertyChanged("image"); }
        }

        void SpaceObject_OnConstrucRequestCompleted(object sender, EventArgs e)
        {
             try
            {
                DownloadStringCompletedEventArgs result = e as DownloadStringCompletedEventArgs;
                SpaceObject retval = JsonConvert.DeserializeObject<SpaceObject>(result.Result);

                this.Copy(retval);

               

                IsError = true;
                Message = String.Empty;
            }
            catch (Exception ex)
            {
                IsError = false;
                Message = "We've failed to load your request";
            }
        }


       


        private int _id_object;
        public int id_object
        {
            get { return _id_object; }
            set { _id_object = value; NotifyPropertyChanged("id_object"); }
        }

        private SpaceType _type;
        public SpaceType type
        {
            get { return _type; }
            set { _type = value; NotifyPropertyChanged("type"); }
        }

        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("name"); }
        }

        private string _description;
        public string description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged("description"); }
        }


        public SpaceObject()
        {
            occurences = new ObservableCollection<SpaceObjectOccurence>();
            channels = new ObservableCollection<SpaceChannel>();
            this.OnDownloadRequestCompleted += SpaceObject_OnConstrucRequestCompleted;
            this.OnUploadRequestCompleted += SpaceObject_OnUploadRequestCompleted;
        }

        void SpaceObject_OnUploadRequestCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {

                    ChannelRootObject channel = JsonConvert.DeserializeObject<ChannelRootObject>(e.Result);
                    
                    if (channel.id != null)
                    {

                        if (OnChannelCreationCompleted != null)
                            OnChannelCreationCompleted(this, new ChannelUserEventArgs(channel.id));

                        IsError = false;
                        Message = String.Empty;
                    }
                    else{
                        IsError = true;
                        Message = "We've failed to load your request";
                    }

                    IsError = false;
                    Message = String.Empty;
                }
                else {
                    IsError = true;
                    Message = "We've failed to load process your request";
                    
                }
            }
            catch (Exception ex)
            {
                IsError = false;
                Message = "We've failed to load your request";
            }
        }


        private ObservableCollection<SpaceObjectOccurence> _occurences;
        public ObservableCollection<SpaceObjectOccurence> occurences
        {
            get { return _occurences; }
            set { _occurences = value; NotifyPropertyChanged("occurences"); }
        }

        private ObservableCollection<SpaceChannel> _channels;
        public ObservableCollection<SpaceChannel> channels
        {
            get { return _channels ; }
            set { _channels = value; NotifyPropertyChanged("channels"); }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Copy(SpaceObject spaceObject)
        {
            this.description = spaceObject.description;
            this.occurences = spaceObject.occurences;
            this.channels = spaceObject.channels;
        }

        //public void Load(bool isRefresh)
        //{
        //    IsRefresh = isRefresh;
        //    Uri uri = new Uri(String.Format(EndpointHelper.OBJECT_DETAIL, id_object));

          
        //    ConstructRequest(uri, "GET", null, null);
          
        //}

        public void Load(bool isRefresh,double lat =-6.907,double lon = 107.611,double alt = 15)
        {
            IsRefresh = isRefresh;
            Uri uri = new Uri(String.Format(EndpointHelper.OBJECT_DETAIL, id_object,lat,lon,alt)+"&timestamp=" + Guid.NewGuid().ToString());


            ConstructRequest(uri, "GET", null, null);

        }

        public void CreateChannel()
        { 
            Uri uri = new Uri(String.Format(EndpointHelper.USER_CREATE_CHANNEL,id_object));
            Dictionary<string,object> parameters  = new Dictionary<string,object>();
            parameters.Add("id_user",App.CurrentUser.id_user);
            string postData = ConstructQueryString(parameters);
            ConstructRequest(uri, "POST", postData, "create_channel");
        }
        

       
    }
}
