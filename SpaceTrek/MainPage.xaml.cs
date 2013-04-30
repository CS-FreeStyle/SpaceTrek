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
using Microsoft.Phone.Tasks;
using System.IO;
using SpaceTrek.Service;
using SpaceTrek.Model;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace SpaceTrek
{
    public partial class MainPage : PhoneApplicationPage
    {
        User user = new User();



        // Constructor
        public MainPage()
        {
            InitializeComponent();

          //  PhotoChooser = new PhotoChooserTask();
          //  PhotoChooser.Completed += PhotoChooser_Completed;


          //  //SpaceObjectCollection vm = new SpaceObjectCollection();
          //  //vm.Load(false);


          //  SpaceObject spaceObj = new SpaceObject();
          //  spaceObj.id_object = 14;
          //  spaceObj.Load(false);

         
          //  user.id_user = 1;

          //  HomeViewModel vm = new HomeViewModel();
          //  vm.Load();

          // // user.Update(21.4, 31.4);
          //  user.OnEventCreationCompleted += user_OnEventCreationCompleted;
          //  user.OnFriendRequestCompleted += user_OnFriendRequestCompleted;




        
            

            //List<SpaceObject> spaceObjects = new List<SpaceObject>() 
            //{ 
            // new SpaceObject(){
            //  name = "musang",
            //   type = SpaceType.meteor,
            //    desc = "Musang 2",
            //     id_object = 1, 
            //      positions = new List<ObjectPosition>()
            //      {
            //         new ObjectPosition() { lat = 3.9 , lon = 2.9 , timestamp = DateTime.Now },
            //         new ObjectPosition() { lat = 3.9 , lon = 2.9 , timestamp = DateTime.Now }

            //      }
            // },
            //      new SpaceObject(){
            //  name = "musang",
            //   type = SpaceType.meteor,
            //    desc = "Musang 2",
            //     id_object = 1, 
            //      positions = new List<ObjectPosition>()
            //      {
            //         new ObjectPosition() { lat = 3.9 , lon = 2.9 , timestamp = DateTime.Now },
            //         new ObjectPosition() { lat = 3.9 , lon = 2.9 , timestamp = DateTime.Now }

            //      }
            //      }
             
            
            //};

            //string s = JsonConvert.SerializeObject(spaceObjects);
        }

        
           

        void PhotoChooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                InputStreamService service = new InputStreamService();
                service.SaveInputStream(e,0);

            }
        }

       
        PhotoChooserTask PhotoChooser;

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PhotoChooser.ShowCamera = true;
            PhotoChooser.Show();
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void OnCameraClicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Camera.xaml", UriKind.Relative));
        }

        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
        	NavigationService.Navigate(new Uri("/View/HomePage.xaml", UriKind.Relative));
        }
    }
}