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
    public partial class LoginPage : PhoneApplicationPage
    {
        private Facebook.FacebookClient client = new Facebook.FacebookClient();
        private const string ExtendedPermissions = "user_events,user_location,user_about_me,create_event,read_friendlists";
        private AuthHelper authHelper = new AuthHelper();

        public LoginPage()
        {
            InitializeComponent();

            client.GetCompleted += client_GetCompleted;
            authHelper.OnLoginCompleted += authHelper_OnLoginCompleted;
            authHelper.OnLoginError += authHelper_OnLoginError;
        }

        void authHelper_OnLoginError(object sender, EventArgs e)
        {
            MessageBox.Show("Something's wrong, please try again in a minutes","Sorry",MessageBoxButton.OK);
        }

        string to_page;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.TryGetValue("page", out to_page))
            { 
                //donothing
            }

        }

        void authHelper_OnLoginCompleted(object sender, EventArgs e)
        {
          

            Dispatcher.BeginInvoke(() =>
                {
                    FacebookWebBrowser.Visibility = Visibility.Collapsed;

                      //CHANGE TO TOAST HELPER

                    FacebookWebBrowser.Navigated -= FacebookBrowser_Navigated;
                    FacebookWebBrowser.Navigate(new Uri("https://www.facebook.com/logout.php?next=https://www.facebook.com/connect/login_success.html&access_token=" + client.AccessToken));

                    MessageBox.Show("You successfully sign-in to SpaceTrek", "Great,", MessageBoxButton.OK);

                    if (to_page == "create_event")
                        NavigationService.Navigate(new Uri("/View/CreateEventPage.xaml?action=delete", UriKind.Relative));
                    else if (to_page == "record_event")
                        NavigationService.Navigate(new Uri("/View/Camera.xaml?action=delete", UriKind.Relative));

                });

        }

        private void OnLoginClicked(object sender, System.Windows.Input.GestureEventArgs e)
        {
            FacebookWebBrowser.Visibility = Visibility.Visible;
            StackPanelHeader.Visibility = Visibility.Collapsed;
            ContentPanel.Visibility = System.Windows.Visibility.Collapsed;

            var loginUrl = GetFacebookLoginUrl(EndpointHelper.FacebookAppId, ExtendedPermissions);
            FacebookWebBrowser.Navigate(loginUrl);
        }

        private void OnRegisterClicked(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/RegisterPage.xaml", UriKind.Relative));
        }

        private void FacebookBrowser_Loaded(object sender, RoutedEventArgs e)
        {

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

                SocialAccount socialAccount = new SocialAccount(userAccount.id, client.AccessToken, userAccount.username, null);
                SettingHelper.SetKeyValue<SocialAccount>("SocialAccount", socialAccount);

                Dispatcher.BeginInvoke(() =>
                {

                    FacebookWebBrowser.Visibility = System.Windows.Visibility.Collapsed;
                    StackPanelHeader.Visibility = System.Windows.Visibility.Visible;
                    ContentPanel.Visibility = System.Windows.Visibility.Visible;
                });

                //login
                authHelper.Login(socialAccount.social_id);
            }

        }

    }
}