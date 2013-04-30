using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTrek.Model
{
    public class SpaceChannel : BaseObject
    {

        private string _user;
        public string user
        {
            get { return _user; }
            set { _user = value; NotifyPropertyChanged("user"); }
        }


        private int _id_channel;
        public int id_channel
        {
            get { return _id_channel; }
            set { _id_channel = value; NotifyPropertyChanged("id_channel"); thumbnail = "http://119.81.24.210/parisvanjava/spacetrek/stream/"+value+"/1.jpg"; }
        }

        private int _id_object;
        public int id_object
        {
            get { return _id_object; }
            set { _id_object = value; NotifyPropertyChanged("id_object"); }
        }

        private int _channel_number;
        public int channel_number
        {
            get { return _channel_number; }
            set { _channel_number = value; NotifyPropertyChanged("channel_number"); }
        }

        private string _thumbnail;
        public string thumbnail
        {
            get { return _thumbnail; }
            set { _thumbnail = value; NotifyPropertyChanged("thumbnail"); }
        }

        private DateTime _timestamp;
        public DateTime timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; NotifyPropertyChanged("timestamp"); }
        }

    }
}
