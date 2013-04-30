using Newtonsoft.Json;
using SpaceTrek.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrek.Model
{
    public class HomeViewModel : BaseLoader
    {

        private SpaceObject _NearestSpaceObject;
        public SpaceObject NearestSpaceObject
        {
            get { return _NearestSpaceObject; }
            set { _NearestSpaceObject = value; NotifyPropertyChanged("NearestSpaceObject"); }
        }

        public HomeViewModel()
        {
            this.OnDownloadRequestCompleted += HomeViewModel_OnDownloadRequestCompleted;
        }

        void HomeViewModel_OnDownloadRequestCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            IsBusy = false;
            try
            {
                if (e.Error == null)
                {
                    NearestSpaceObject = JsonConvert.DeserializeObject<SpaceObject>(e.Result);

                    IsError = false;
                    Message = String.Empty;

                }
                else {
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
            if (!IsBusy)
            {
                IsBusy = true;
                Uri uri = new Uri(EndpointHelper.OBJECT_CLOSEST);

                ConstructRequest(uri, "GET", null, null);
            }
        }
    }
}
