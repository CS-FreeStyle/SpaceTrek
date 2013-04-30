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
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using SpaceTrek.Helper;

namespace SpaceTrek
{
    public partial class HomePage : PhoneApplicationPage
    {
        HomeViewModel vm;

     



        public HomePage()
        {
            InitializeComponent();
            vm = new HomeViewModel();
            DataContext = vm;


        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (vm.NearestSpaceObject == null)
                vm.Load();


            if (SettingHelper.GetKeyValue<User>("User") == null)
                btnAllLogout.Visibility = Visibility.Collapsed;
            else
                btnAllLogout.Visibility = Visibility.Visible;
        }

       

        private void btnAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(new Uri("/View/ObjectListPage.xaml", UriKind.Relative));
        }

        private void OnClicked(object sender, System.Windows.Input.GestureEventArgs e)
        {
                PhoneApplicationService.Current.State["SelectedObject"] = vm.NearestSpaceObject;
                NavigationService.Navigate(new Uri("/View/ObjectDetailPage.xaml", UriKind.Relative));
        }

        private void OnScheduleClicked(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
            NavigationService.Navigate(new Uri("/View/CalendarPage.xaml", UriKind.Relative));
        }

        private void btnAll_Copy_Click(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.State["SelectedObject"] = vm.NearestSpaceObject;
            User user = SettingHelper.GetKeyValue<User>("User");

            if (user != null)
            {

                NavigationService.Navigate(new Uri("/View/CreateEventPage.xaml", UriKind.Relative));
       
            }
            else
            {
                NavigationService.Navigate(new Uri("/View/LoginPage.xaml?page=create_event", UriKind.Relative));
            }
        }

        private void OnLogoutClicked(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try{
                

                WebBrowser browser = new WebBrowser();
                browser.Navigate(new Uri(String.Format("https://www.facebook.com/logout.php?next={0}&access_token={1}", null,SettingHelper.GetKeyValue<SocialAccount>("SocialAccount").access_token )));

                browser.Navigated += browser_Navigated;
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something is wrong, we've failed to process your request");
            }
        }

        void browser_Navigated(object sender, NavigationEventArgs e)
        {
            (sender as WebBrowser).Navigated -= browser_Navigated;
            
            MessageBox.Show("You successfully sign-out");

            SettingHelper.SetKeyValue<SocialAccount>("SocialAccount", null);
            SettingHelper.SetKeyValue<User>("User", null);

            if (SettingHelper.GetKeyValue<User>("User") == null)
                btnAllLogout.Visibility = Visibility.Collapsed;
            else
                btnAllLogout.Visibility = Visibility.Visible;
        }
    }
}