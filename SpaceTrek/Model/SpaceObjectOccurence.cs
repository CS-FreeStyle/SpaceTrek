using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrek.Model
{
    public class SpaceObjectOccurence : BaseLoader
    {

        private int _risetime;
        public int risetime
        {
            get { return _risetime; }
            set { _risetime = value; NotifyPropertyChanged("risetime"); }
        }

        private string _date;
        public string date
        {
            get { return _date; }
            set { _date = value; NotifyPropertyChanged("date"); }
        }

        private int _timeleft;
        public int timeleft
        {
            get { return _timeleft; }
            set { _timeleft = value; NotifyPropertyChanged("timeleft"); SetTime(value); }
        }

        private int _days;
        public int days
        {
            get { return _days; }
            set { _days = value; NotifyPropertyChanged("days"); }
        }

        private int _hours;
        public int hours
        {
            get { return _hours; }
            set { _hours = value; NotifyPropertyChanged("hours"); }
        }

        private int _minutes;
        public int minutes
        {
            get { return _minutes; }
            set { _minutes = value; NotifyPropertyChanged("minutes"); }
        }

        private int _seconds;
        public int seconds
        {
            get { return _seconds; }
            set { _seconds = value; NotifyPropertyChanged("seconds"); }
        }

        public void SetTime(int time)
        {
            int i = Math.Abs(time);
            DateTime t = new DateTime().AddSeconds(i);
            this.days = t.Day;
            this.hours = t.Hour;
            this.minutes = t.Minute;
            this.seconds = t.Second;
        }
    }
}
