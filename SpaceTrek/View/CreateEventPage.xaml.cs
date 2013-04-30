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
using SpaceTrek.Helper;

namespace SpaceTrek.View
{
    public partial class CreateEventPage : PhoneApplicationPage
    {

        //CREATE LOADER

        public CreateEventPage()
        {
            InitializeComponent();
            App.CurrentUser.OnEventCreationCompleted += CurrentUser_OnEventCreationCompleted;
            App.CurrentUser.OnFriendRequestCompleted += CurrentUser_OnFriendRequestCompleted;
            App.CurrentUser.OnUserInvitationCompleted += CurrentUser_OnUserInvitationCompleted;
        }

        void CurrentUser_OnUserInvitationCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    if ( (e as UploadStringCompletedEventArgs).Result.ToString() == "success")
                    {
                        if (FriendList.SelectedItems.Count > 1)
                            MessageBox.Show("Your invitations are succesfully sent");
                        else
                            MessageBox.Show("Your invitation is succesfully sent");
                        NavigationService.GoBack();
                    }
                });
        }
        List<FBUser> users = new List<FBUser>();
        void CurrentUser_OnFriendRequestCompleted(object sender, Model.FacebookUserEventArgs e)
        {
           // List<FBUser> users = e.Users;
            users = new List<FBUser>();
           
            foreach (var x in e.Users)
            { 
                x.image = "https://graph.facebook.com/"+ x.id+"/picture";
                users.Add(x);
            }

            FriendList.ItemsSource = users.OrderBy(item => item.name) ;
            //App.CurrentUser.InviteFriends(eventId, User.ParseInvitation(users));
        }

        string eventId = String.Empty;

       
        void CurrentUser_OnEventCreationCompleted(object sender, Model.EventUserEventArgs e)
        {
            eventId = e.EventId;

            //FADE OUT LOADER CREATING EVENT, CHANGE TO FRIEND LOADER

            if (MessageBox.Show("The event successfully created. Tap ok to invite your friend", "Great", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.CurrentUser.GetFriends();

            }
            else
                NavigationService.GoBack();



         
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (PhoneApplicationService.Current.State["SelectedObject"] != null)
            {
                SpaceObject obj = PhoneApplicationService.Current.State["SelectedObject"] as SpaceObject;

               
                
                CreateEvent(obj);
            }
            string action = null;
            if (NavigationContext.QueryString.TryGetValue("action",out action))
            {
                if (action == "delete")
                    NavigationService.RemoveBackEntry();
            }
        }

        protected void CreateEvent(SpaceObject obj)
        {
            App.CurrentUser.CreateEvent(obj, SettingHelper.GetKeyValue<SocialAccount>("SocialAccount").access_token);
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            var friends = FriendList.SelectedItems;

            List<FBUser> user_to = new List<FBUser>();

            foreach (var x in friends)
            {
                FBUser user = x as FBUser;
                user_to.Add(user);
            }


           
           App.CurrentUser.InviteFriends(eventId, user_to, SettingHelper.GetKeyValue<SocialAccount>("SocialAccount").access_token);
        }
    }
}