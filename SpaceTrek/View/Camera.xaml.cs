using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SpaceTrek.Service;
using System.Windows.Controls.Primitives;

namespace SpaceTrek.View
{
    public partial class Camera : PhoneApplicationPage
    {

        private InputStreamService vm;

        public Camera()
        {
            InitializeComponent();
            vm = new InputStreamService();
           // vm.InitCamera(viewfinderBrush);
         //   vm.InitSpaceChannel(new Model.SpaceChannel() { id_channel = 27 });
            vm.OnChannelCreationCompleted += vm_OnChannelCreationCompleted;
        }

        

        void vm_OnChannelCreationCompleted(object sender, EventArgs e)
        {
            Loader.Visibility = System.Windows.Visibility.Collapsed;
        }

        string action;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SpaceObject obj = null;
            
            if (PhoneApplicationService.Current.State["CurrentSpaceObject"] != null)
            {
                obj = PhoneApplicationService.Current.State["CurrentSpaceObject"] as SpaceObject;
            }
            else
            {

                obj = new SpaceObject() { id_object = 1 };
            }
            vm.InitCamera(viewfinderBrush, obj);
            if (NavigationContext.QueryString.TryGetValue("action", out action))
            {
                if (action == "delete")
                    NavigationService.RemoveBackEntry();
            }
            
        }

        protected override void OnOrientationChanged(OrientationChangedEventArgs e)
        {
            

            base.OnOrientationChanged(e);
            //vm.OnOrientationChanged(e);
            
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            vm.DisposeCamera();
        }

        private void ShutterButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            if (button.IsChecked.Value)
            {
                vm.StartCapture();
            }
            else
                vm.StopCapture();
        }

        private void OnCameraTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!vm.IsActive)
            {
                vm.StartCapture();
            }
            else {
                vm.StopCapture();
            }
        }
    }
}