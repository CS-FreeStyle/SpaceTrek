using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTrek.Helper
{
    public class EndpointHelper
    {
        //inser your url here
        public const string URL_BASE = "<your base url>";
        public static readonly string FacebookAppId = "<your facebook id>";
        public const string REGISTER = "<your link register>";

        public const string USER_NAME = "<your username>";
        public const string PASSWORD = "<your password>";

        

        public const string OBJECT_LIST = URL_BASE + "object/list?lat={0}&lon={1}&alt={2}";
        public const string OBJECT_DETAIL = URL_BASE + "object/get/{0}?lat={1}&lon={2}&alt={3}";

        public const string USER_UPDATE = URL_BASE + "user/update/{0}";

        public const string USER_CREATE_CHANNEL = URL_BASE + "channel/add/{0}";

        public const string USER_CREATE_EVENT = URL_BASE + "event/create";

        public const string USER_EVENT_INVITE = URL_BASE + "event/invite/{0}";

        public const string CHANNEL_UPLOAD = URL_BASE + "channel/upload/{0}";

        public const string GET_STREAM = URL_BASE + "channel/get/{0}";

        public const string GET_CHANNEL = URL_BASE + "object/get/{0}";

        //STATIC 
        public const string USER_GET_FRIENDS = "https://graph.facebook.com/me/friends?access_token={0}";

        public const string USER_PHOTO = "http://graph.facebook.com/{0}/picture";

        public const string OBJECT_CLOSEST = URL_BASE + "object/closest";

        public const string CHANNEL_DOWNLOAD = URL_BASE + "channel/get/{0}";
    }
}
