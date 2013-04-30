using Newtonsoft.Json;
using SpaceTrek.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrek.Helper
{
    public class AuthHelper : BaseLoader
    {
        public event EventHandler<EventArgs> OnRegisterCompleted;
        public event EventHandler<EventArgs> OnLoginCompleted;
        public event EventHandler<EventArgs> OnLoginError;
        public event EventHandler<EventArgs> OnRegisterError;

        private User user;

        public AuthHelper()
        {
            this.OnUploadRequestCompleted += AuthHelper_OnUploadRequestCompleted;
            this.OnDownloadRequestCompleted += AuthHelper_OnDownloadRequestCompleted;
        }

        void AuthHelper_OnDownloadRequestCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e)
        {
            try
            {
                string result = e.Result;
                FBUser user = JsonConvert.DeserializeObject<FBUser>(result);

                User userAccout = new User() { id_user = Int32.Parse(user.id), name = user.name };

                App.CurrentUser = userAccout;

                SettingHelper.SetKeyValue<User>("User", userAccout);

                if (OnLoginCompleted != null)
                    OnLoginCompleted(this, e);
                
            }
            catch (Exception ex)
            {
               
                if (OnRegisterError != null)
                    OnRegisterError(this, e);
            }
        }

        void AuthHelper_OnUploadRequestCompleted(object sender, System.Net.UploadStringCompletedEventArgs e)
        {
            try
            {
                string result = e.Result;
                
                    UserParisVanJavaArgs userId = JsonConvert.DeserializeObject<UserParisVanJavaArgs>(result);

                    User user = new User();
                    user.id_user = userId.id;

                    SettingHelper.SetKeyValue<User>("User", user);

                    if (OnRegisterCompleted != null)
                        OnRegisterCompleted(this, e);
                
               
            }
            catch (Exception ex)
            {
                if (OnRegisterError != null)
                    OnRegisterError(this, e);
              
            }
        }

        public AuthHelper(User user)
        {
            this.user = user;
            this.OnUploadRequestCompleted += AuthHelper_OnUploadRequestCompleted;
        }

        public void Register()
        { 
        
        }

        public void Login()
        { }

        public void Login(string social_id)
        {
            IsBusy = true;

            string postData = "source=facebook&username=" + social_id;

            Uri uri = new Uri(EndpointHelper.REGISTER+ "?" + postData + "&tmp=" + Guid.NewGuid().ToString());

            
            ConstructRequest(uri ,"GET",null,"login",EndpointHelper.USER_NAME,EndpointHelper.PASSWORD);
        }

        public void Register(string username,string email,string social_id)
        {
            IsBusy = true;
            Uri uri = new Uri(EndpointHelper.REGISTER);

            
            string  postData = "source=facebook&username=" + username + "&email=" + email + "&social_id=" + social_id;

            ConstructRequest(uri, "POST", postData, "register",EndpointHelper.USER_NAME,EndpointHelper.PASSWORD);
        }
    }
}
