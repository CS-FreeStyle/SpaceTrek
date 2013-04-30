using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTrek.Model
{
    public class ObjectPosition : BaseObject
    {

        private int _id_post;
        public int id_post
        {
            get { return _id_post; }
            set { _id_post = value; NotifyPropertyChanged("id_post"); }
        }

        private int _id_object;
        public int id_object
        {
            get { return _id_object; }
            set { _id_object = value; NotifyPropertyChanged("id_object"); }
        }

        private DateTime _timestamp;
        public DateTime timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; NotifyPropertyChanged("timestamp"); }
        }

        private double _lat;
        public double lat
        {
            get { return _lat; }
            set { _lat = value; NotifyPropertyChanged("lat"); }
        }

        private double _lon;
        public double lon
        {
            get { return _lon; }
            set { _lon = value; NotifyPropertyChanged("lon"); }
        }

    }
}
