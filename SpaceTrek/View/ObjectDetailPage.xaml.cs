using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SpaceTrek.Model;
using SpaceTrek.Helper;

namespace SpaceTrek
{
    public partial class ObjectDetailPage : PhoneApplicationPage
    {
        ObjectDetailViewModel vm;
        public ObjectDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            vm = new ObjectDetailViewModel();
            string query = null;
            if (NavigationContext.QueryString.TryGetValue("id_object", out query))
            {
                vm.spaceObject = new SpaceObject();
                vm.spaceObject.id_object = Int32.Parse(query);
                vm.spaceObject.Load(false);
                DataContext = vm;
            }
            else
            {
                vm.spaceObject = PhoneApplicationService.Current.State["SelectedObject"] as SpaceObject;
                vm.Load();
                DataContext = vm;
               
            }
            base.OnNavigatedTo(e);
        }

        private void ListBox_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            int index = (sender as ListBox).SelectedIndex;
            if(index!=-1)
            {
                PhoneApplicationService.Current.State["CurrentSpaceChannel"] = vm.spaceObject.channels[index];
                NavigationService.Navigate(new Uri("/View/VideoViewerpage.xaml", UriKind.Relative));
                (sender as ListBox).SelectedIndex = -1;
            }
        }

        private void OnRecordTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneApplicationService.Current.State["CurrentSpaceObject"] = vm.spaceObject;

            User user = SettingHelper.GetKeyValue<User>("User");

            if (user != null)
            {

                NavigationService.Navigate(new Uri("/View/Camera.xaml", UriKind.Relative));
            }
            else {
                NavigationService.Navigate(new Uri("/View/LoginPage.xaml?page=record_event", UriKind.Relative));
            }
        }

        private void abRefresh_Click(object sender, System.EventArgs e)
        {
            vm.Load();
        }

        private void OnEventTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneApplicationService.Current.State["SelectedObject"] = vm.spaceObject;
            User user = SettingHelper.GetKeyValue<User>("User");

            if (user != null)
            {

                NavigationService.Navigate(new Uri("/View/CreateEventPage.xaml", UriKind.Relative));

            }
            else
            {
                NavigationService.Navigate(new Uri("/View/LoginPage.xaml?page=create_event", UriKind.Relative));
            }

            

            NavigationService.Navigate(new Uri("/View/CreateEventPage.xaml", UriKind.Relative));
        }
    }
}