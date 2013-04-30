using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SpaceTrek.Helper;
using Facebook;
using SpaceTrek.Model;
using Newtonsoft.Json;

namespace SpaceTrek.View
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        private Facebook.FacebookClient client = new Facebook.FacebookClient();
        private const string ExtendedPermissions = "user_events,user_location,user_about_me,create_event,read_friendlists";
        private AuthHelper authHelper = new AuthHelper();

        public RegisterPage()
        {
            InitializeComponent();
            this.Loaded += RegisterPage_Loaded;
            client.GetCompleted += client_GetCompleted;


            StackPanelHeader.Visibility = System.Windows.Visibility.Collapsed;
            ContentPanel.Visibility = System.Windows.Visibility.Collapsed;
            FacebookWebBrowser.Visibility = System.Windows.Visibility.Visible;
            authHelper.OnRegisterCompleted += authHelper_OnRegisterCompleted;
            authHelper.OnRegisterError += authHelper_OnRegisterError;

        }

        void authHelper_OnRegisterError(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {

                MessageBox.Show("Register failed");

                //remove login
                NavigationService.RemoveBackEntry();

                NavigationService.GoBack();

            });
        }

        void authHelper_OnRegisterCompleted(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() => {

                MessageBox.Show("Register success");

                //remove login
                NavigationService.RemoveBackEntry();

                NavigationService.GoBack();
            
            });
        }

        void client_GetCompleted(object sender, FacebookApiEventArgs e)
        {
            if (e.Error != null)
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                return;
            }
            else
            {
                //SocialAccount account = new SocialAccount(
                //var result = (IDictionary<string, object>)e.GetResultData();
                //var id = (string)result["id"];
                FBUser userAccount = JsonConvert.DeserializeObject<FBUser>(e.GetResultData().ToString());
                
                SocialAccount socialAccount = new SocialAccount(userAccount.id,client.AccessToken,userAccount.username,null);
                SettingHelper.SetKeyValue<SocialAccount>("SocialAccount",socialAccount);

                Dispatcher.BeginInvoke(() =>
                {

                    FacebookWebBrowser.Visibility = System.Windows.Visibility.Collapsed;
                    StackPanelHeader.Visibility = System.Windows.Visibility.Visible;
                    ContentPanel.Visibility = System.Windows.Visibility.Visible;
                });
            }
           
        }



        

        void RegisterPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void FacebookBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(EndpointHelper.FacebookAppId, ExtendedPermissions);
            FacebookWebBrowser.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = EndpointHelper.FacebookAppId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return client.GetLoginUrl(parameters);
        }

        private void FacebookBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!client.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                var accessToken = oauthResult.AccessToken;
                FacebookWebBrowser.Visibility = System.Windows.Visibility.Collapsed;
                LoginSucceded(accessToken);
            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void LoginSucceded(string accessToken)
        {
            client.AccessToken = accessToken;
           
            client.GetAsync("me");
        }

        private void OnRegisterClicked(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TextBoxEmail.Text.Length == 0)
            {
                //Not valid
            }
            else
            {
                SocialAccount account = SettingHelper.GetKeyValue<SocialAccount>("SocialAccount");
                authHelper.Register(account.user_name,TextBoxEmail.Text, account.social_id);
            }
        }
    }
}