using Newtonsoft.Json;
using SpaceTrek.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;

namespace SpaceTrek.Model
{
    public class User : BaseLoader
    {

        public User()
        {
            this.OnDownloadRequestCompleted += User_OnDownloadRequestCompleted;
            this.OnUploadRequestCompleted += User_OnUploadRequestCompleted;
        }

        public event EventHandler<FacebookUserEventArgs> OnFriendRequestCompleted;
        public event EventHandler<EventUserEventArgs> OnEventCreationCompleted;
        public event EventHandler<EventArgs> OnUserInvitationCompleted;
        public event EventHandler<EventArgs> OnError;

        void User_OnDownloadRequestCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                string operation = e.UserState.ToString();

                //update position
                if (operation == "get_friends")
                {

                    if (e.Error == null)
                    {
                        FBRootObject fbRootObject = JsonConvert.DeserializeObject<FBRootObject>(e.Result);

                        if (OnFriendRequestCompleted != null)
                            OnFriendRequestCompleted(this, new FacebookUserEventArgs(fbRootObject));
                    }
                    else
                    {
                        MessageBox.Show("We've failed to load your friend");
                    }

                    //if (result.Result.Equals("{\"status\":\"Saved successfully\"}"))
                    //{
                    //    IsError = false;
                    //    Message = String.Empty;
                    //}
                    //else
                    //{
                    //    IsError = true;
                    //    Message = "We've failed to load your request";
                    //}
                }
               
                   
                
            }
            catch (Exception ex)
            {
                IsError = false;
                Message = "We've failed to load your request";
                if (OnError != null)
                    OnError(this, e);
            }
        }

        void User_OnUploadRequestCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                string operation = e.UserState.ToString();
                
                //update position
                if (operation == "update_pos")
                {
                    UploadStringCompletedEventArgs result = e as UploadStringCompletedEventArgs;
                    if (result.Result.Equals("{\"status\":\"Saved successfully\"}"))
                    {
                        IsError = false;
                        Message = String.Empty;
                    }
                    else
                    {
                        IsError = true;
                        Message = "We've failed to load your request";
                    }
                }
                else if (operation == "create_event")
                {
                    UploadStringCompletedEventArgs result = e as UploadStringCompletedEventArgs;
                    if (e.Error == null)
                    {
                        IsError = false;
                        Message = String.Empty;

                        EventRootObject eventResult = JsonConvert.DeserializeObject<EventRootObject>(e.Result);

                        if (OnEventCreationCompleted != null)
                            OnEventCreationCompleted(sender, new EventUserEventArgs(eventResult.event_id));
                    }
                    else
                    {
                        IsError = true;
                        Message = "We've failed to load your request";
                        if (OnError != null)
                            OnError(this, e);
                    }
                }
                else if (operation == "invite_friends")
                {
                    UploadStringCompletedEventArgs result = e as UploadStringCompletedEventArgs;
                    if (result.Result.Equals("success"))
                    {
                        IsError = false;
                        Message = String.Empty;
                        if (OnUserInvitationCompleted != null)
                            OnUserInvitationCompleted(sender, e);
                    }
                    else
                    {
                        IsError = true;
                        Message = "We've failed to process invitation per your request";
                        if (OnError != null)
                            OnError(this, e);
                    }
                }
            }
            catch (Exception ex)
            {
                IsError = false;
                Message = "We've failed to load your request";
                if (OnError != null)
                    OnError(this, e);
            }
        }

       

        private int _id_user;
        public int id_user
        {
            get { return _id_user; }
            set { _id_user = value; NotifyPropertyChanged("id_user"); }
        }

        private String _name;
        public String name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("name"); }
        }

        private double _lat;
        public double lat
        {
            get { return _lat; }
            set { _lat = value; NotifyPropertyChanged("lat"); }
        }

        private double _lon;
        public double lon
        {
            get { return _lon; }
            set { _lon = value; NotifyPropertyChanged("lon"); }
        }

        public void Update(double lat, double lon)
        {
            Uri uri = new Uri(String.Format(EndpointHelper.USER_UPDATE, id_user));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("lat", lat);
            parameters.Add("long", lon);

            string postData = ConstructQueryString(parameters);

            ConstructRequest(uri, "POST", postData, "update_pos");
        }

        public void CreateEvent(SpaceObject spaceObject,string access_token=null)
        {
            CreateEvent(spaceObject.id_object,access_token);
        }

        public void CreateEvent(int id_object,string access_token =null)
        {
            Uri uri = new Uri(String.Format(EndpointHelper.USER_CREATE_EVENT, id_user));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id_user", id_user);
            parameters.Add("id_object", id_object);
            parameters.Add("token", access_token);

            string postData = ConstructQueryString(parameters);

            ConstructRequest(uri, "POST", postData, "create_event");
        }

        public void GetFriends()
        {
            SocialAccount account = SettingHelper.GetKeyValue<SocialAccount>("SocialAccount");
            Uri uri = new Uri( String.Format(EndpointHelper.USER_GET_FRIENDS,account.access_token));

            ConstructRequest(uri, "GET", null, "get_friends");
        }


        private string ConstructFacebookFriends(List<FBUser> friends)
        {
            string result = String.Empty;
            


            foreach (var x in friends)
            {
                result += x.id + ",";
            }
            result = result.Remove(result.Length - 1);



            //foreach (var x in App.InvitationLists )
            //{
            //    result += x;
            //}
            //result = result.Remove(result.Length - 1);



            return result;
        }

        public void InviteFriends(string eventId,List<FBUser> friends,string token = null)
        {
            Uri uri = new Uri(String.Format(EndpointHelper.USER_EVENT_INVITE,eventId));
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id_users", ConstructFacebookFriends(friends));
            if (token != null)
                parameters.Add("token", token);


            string postData = ConstructQueryString(parameters);

            ConstructRequest(uri, "POST", postData, "invite_friends");

        }

       
    }
}
