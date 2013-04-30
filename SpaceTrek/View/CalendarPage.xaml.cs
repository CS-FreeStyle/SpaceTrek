using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SpaceTrek.Model;
using WPControls;
using System.Windows.Media;

namespace SpaceTrek
{
    public partial class CalendarPage : PhoneApplicationPage
    {
        public CalendarPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //List<CalendarItem> items = new List<CalendarItem>();


            
            
         //  ZCalendar.DatesSource = e.
        }

        private void ZCalendar_SelectionChanged_1(object sender, WPControls.SelectionChangedEventArgs e)
        {
            try
            {
                DateTime dt = e.SelectedDate;
                List<SpaceObject> spaces = new List<SpaceObject>();
                foreach (var x in App.MasterSpaceObjects.SpaceItems)
                {
                    foreach (var y in x.occurences)
                    {
                        DateTime comp = DateTime.Parse(y.date);
                        if (comp.Date == dt.Date)
                        {
                            spaces.Add(x);
                            break;
                        }
                    }
                }

                lbSpaceObj.ItemsSource = spaces;
            }
            catch { 
            
            }
        }

       
    }
}