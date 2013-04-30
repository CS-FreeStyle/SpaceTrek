using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTrek.Model
{
    public class Calendar : BaseObject
    {

        private int _id_calendar;
        public int id_calendar
        {
            get { return _id_calendar; }
            set { _id_calendar = value; NotifyPropertyChanged("id_calendar"); }
        }

        private int _id_object;
        public int id_object
        {
            get { return _id_object; }
            set { _id_object = value; NotifyPropertyChanged("id_object"); }
        }

        private DateTime _date;
        public DateTime date
        {
            get { return _date; }
            set { _date = value; NotifyPropertyChanged("date"); }
        }

    }
}
