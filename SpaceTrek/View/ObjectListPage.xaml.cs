using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using Phone.Controls;

namespace SpaceTrek
{
    public partial class ObjectListPage : PhoneApplicationPage
    {
        SpaceObjectCollection vm;
        GeoCoordinateWatcher watcher;
        PickerBoxDialog dialog;

        public ObjectListPage()
        {
            InitializeComponent();
            vm = new SpaceObjectCollection();
            DataContext = vm;

            dialog = new PickerBoxDialog();
            dialog.ItemSource = new List<String>() { "All", "Meteor", "Satellite", "Station", "Others" };
            dialog.Closed += dialog_Closed;
        }

        void dialog_Closed(object sender, EventArgs e)
        {
            try
            {
                string category = dialog.SelectedItem.ToString();
                vm.Filter(category);
                vm.Order();
            }
            catch
            {
                string category = "All";
                vm.Filter(category);
                vm.Order();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (watcher == null)
            {
                watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                watcher.MovementThreshold = 20;
                watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
                watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            }

            watcher.Start();

            if (!vm.IsLoad)
            {
                vm.Load(false, App.lat, App.lon, App.alt);
                if (App.MasterSpaceObjects.IsLoad)
                {
                    vm.Load(App.MasterSpaceObjects);
                }
                else
                {
                    vm.Load(false, App.lat, App.lon, App.alt);
                }

            }

        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            App.lat = e.Position.Location.Latitude;
            App.lon = e.Position.Location.Longitude;
            App.alt = e.Position.Location.Altitude;
        }

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Point point = new Point(0, 0);
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    if (watcher.Permission == GeoPositionPermission.Denied)
                    {

                        MessageBox.Show("you turn off real-time location service functionality, application will show data from your last position", "ATTENTION", MessageBoxButton.OK);

                        //point = SettingHelper.GetKeyValue<Point>("DefaultLocation");

                        //App.Latitude = point.X;
                        //App.Longitude = point.Y;


                        //LOAD DATA
                        vm.Load(true, App.lat, App.lon, App.alt);
                    }

                    break;

                case GeoPositionStatus.Initializing:
                    break;

                case GeoPositionStatus.NoData:

                    //LOAD DATA
                    break;

                case GeoPositionStatus.Ready:
                    App.lat = watcher.Position.Location.Latitude;
                    App.lon = watcher.Position.Location.Longitude;
                    App.alt = watcher.Position.Location.Altitude;



                    //LOAD DATA
                    vm.Load(true, App.lat, App.lon, App.alt);
                    //watcher.Stop();


                    break;
            }
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex;

            if (index != -1)
            {
                PhoneApplicationService.Current.State["SelectedObject"] = vm.SpaceItems[index];
                NavigationService.Navigate(new Uri("/View/ObjectDetailPage.xaml", UriKind.Relative));
                (sender as ListBox).SelectedIndex = -1;
            }
        }

        private void ApplicationBarIconButton_Click_1(object sender, System.EventArgs e)
        {
            dialog.Show();
        }
    }
}