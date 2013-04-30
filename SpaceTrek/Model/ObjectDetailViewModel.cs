using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SpaceTrek.Helper;

namespace SpaceTrek.Model
{
    class ObjectDetailViewModel:BaseLoader
    {
        private SpaceObject _spaceObject;
        public SpaceObject spaceObject
        {
            get { return _spaceObject; }
            set { _spaceObject = value; NotifyPropertyChanged("NearestSpaceObject"); }
        }

        public ObjectDetailViewModel()
        {
            this.OnDownloadRequestCompleted += ObjectDetailViewModel_OnDownloadRequestCompleted;
        }

        void ObjectDetailViewModel_OnDownloadRequestCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            IsBusy = false;
            try
            {
                if (e.Error == null)
                {
                    //if (e.UserState == "spaceObject")
                    //{
                        spaceObject.channels = JsonConvert.DeserializeObject<ObservableCollection<SpaceChannel>>(e.Result);
                    //    LoadStream();
                    //}
                    //else if (e.UserState == "stream")
                    //{
                    //    listStream = JsonConvert.DeserializeObject<ObservableCollection<ObjectStream>>(e.Result);
                    //}

                    IsError = false;
                    Message = String.Empty;

                }
                else
                {
                    IsError = true;
                    Message = "We've failed to load your request";

                }
            }
            catch (Exception ex)
            {
                IsError = true;
                Message = "We've failed to load your request";
            }
        }


        public void Load()
        {
           
            this.spaceObject.Load(false);
            //if (!IsBusy)
            //{
            //    IsBusy = true;
            //    Uri uri = new Uri(string.Format(EndpointHelper.GET_CHANNEL, spaceObject.id_object));

            //    ConstructRequest(uri, "GET", null, "spaceObject");
            //}
        }
    }
}
