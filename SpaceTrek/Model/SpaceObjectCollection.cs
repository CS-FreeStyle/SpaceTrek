using Newtonsoft.Json;
using SpaceTrek.Helper;
using SpaceTrek.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

namespace SpaceTrek
{
    public class SpaceObjectCollection : BaseLoader
    {
        public List<KeyValuePair<int, DateTime>> CreateCalendar()
        {
            DateTime now = DateTime.Now;
            List<KeyValuePair<int, DateTime>> dateTimes = new List<KeyValuePair<int, DateTime>>();
            foreach (var x in App.MasterSpaceObjects.SpaceItems)
            {
                foreach (var occ in x.occurences)
                {
                    DateTime val = new DateTime(now.Year, now.Month, now.Day) + TimeSpan.FromDays(occ.days);
                    dateTimes.Add(new KeyValuePair<int, DateTime>(x.id_object, val));
                }
            }
            return dateTimes;
        }

        public List<KeyValuePair<int, DateTime>> CalendarItems = new List<KeyValuePair<int, DateTime>>();

        private ObservableCollection<SpaceObject> MasterItems;

        private ObservableCollection<SpaceObject> _SpaceItems;
        public ObservableCollection<SpaceObject> SpaceItems
        {
            get { return _SpaceItems; }
            set { _SpaceItems = value; NotifyPropertyChanged("SpaceItems"); }
        }

        //public void Load(bool isRefresh)
        //{
        //    IsRefresh = isRefresh;
        //    Uri uri = new Uri( EndpointHelper.OBJECT_LIST);
           
        //    if (!IsLoad)
        //    {
        //        ConstructRequest(uri, "GET", null, null);
        //    }
        //}

        public void Load(bool isRefresh,double lat =-6.907,double lon = 107.611,double alt = 15)
        {
            IsRefresh = isRefresh;
            Uri uri = new Uri(String.Format(EndpointHelper.OBJECT_LIST,lat, lon, alt));



            if (!IsLoad)
            {
                ConstructRequest(uri, "GET", null, null);
            }
        }

        public SpaceObjectCollection()
        {
            SpaceItems = new ObservableCollection<SpaceObject>();
            this.OnDownloadRequestCompleted += SpaceObjectCollection_OnConstrucRequestCompleted;
        }

        /// <summary>
        /// Return space collection based on space type
        /// </summary>
        /// <param name="spaceType"></param>
        public void Filter(SpaceType spaceType)
        {
            if (spaceType == SpaceType.all)
                SpaceItems = MasterItems;
            else
                SpaceItems = new ObservableCollection<SpaceObject>( MasterItems.Where(item => item.type == spaceType).ToList()) ;
        }

        void SpaceObjectCollection_OnConstrucRequestCompleted(object sender, EventArgs e)
        {
            try
            {
                DownloadStringCompletedEventArgs result = e as DownloadStringCompletedEventArgs;
                List<SpaceObject> retval = JsonConvert.DeserializeObject<List<SpaceObject>>(result.Result);

                MasterItems = new ObservableCollection<SpaceObject>(retval);

                if (IsRefresh)
                    SpaceItems.Clear();

                foreach (var x in retval)
                {
                    SpaceItems.Add(x);
                    x.Load(false,App.lat,App.lon,App.alt);
                }
                IsError = true;
                Message = String.Empty;
                IsLoad = true;

                //create calendar
                CalendarItems = CreateCalendar();
            }
            catch (Exception ex)
            {
                IsError = false;
                Message = "We've failed to load your request";
            }
        }

        public void Filter(string category)
        {
           // Load(true, App.lat, App.lon, App.alt);
            if (category != "All")
            {
                SpaceType type = SpaceType.meteor;
                if (category == "Meteor") { type = SpaceType.meteor; }
                else if (category == "Satellite") { type = SpaceType.satellite; }
                else if (category == "Others") { type = SpaceType.other; }
                else if (category == "Station") { type = SpaceType.station; }

                Filter(type);
            }
            else
                Filter(SpaceType.all);
        }

        public void Order()
        {
            SpaceItems = new ObservableCollection<SpaceObject>(SpaceItems.OrderBy(item => item.name).ToList());
        }


        internal void Load(SpaceObjectCollection spaceObjectCollection)
        {
            foreach (var x in spaceObjectCollection.SpaceItems)
            {
                SpaceItems.Add(x);
            }
            IsLoad = true;
        }
    }
}
