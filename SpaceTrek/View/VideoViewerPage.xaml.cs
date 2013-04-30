using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using SpaceTrek.Service;
using SpaceTrek.Model;
using System.Windows.Media.Imaging;

namespace SpaceTrek.View
{
    public partial class VideoViewerPage : PhoneApplicationPage
    {
        DisplayStreamService vm = new DisplayStreamService();

        public VideoViewerPage()
        {
            InitializeComponent();
            InitTimer();
            vm.OnReadyToPlay += vm_OnReadyToPlay;
        }

        void vm_OnReadyToPlay(object sender, EventArgs e)
        {
            
            StartTimer();
            Loader.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void Replay()
        {
            vm.Replay();
            StartTimer();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object channel =null;
            if (PhoneApplicationService.Current.State.TryGetValue("CurrentSpaceChannel",out channel))
            {
                vm.InitCurrentSpaceChannel(channel as SpaceChannel);
            }
            else{
                SpaceChannel channel2 = new SpaceChannel();
                channel2.id_channel = 27;
                vm.InitCurrentSpaceChannel(channel2);
            }
            vm.Display();
        }

        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            StopTimer();
        }

        #region Timer
        private DispatcherTimer timer;

        private void InitTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                BitmapImage img = null;
                vm.LoadBitmap(ref img);
                GalleryViewer.Source = img;
            }
            catch (Exception ex)
            {
                StopTimer();
            }
        }

        private void InitTimer(double value)
        {
            timer.Interval = TimeSpan.FromMilliseconds(value);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void StartTimer()
        {
            timer.Start();
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        #endregion

        private void GalleryViewer_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Replay();
        }

        private void OnRectangleTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StopTimer();
            Replay();
        }
    }
}