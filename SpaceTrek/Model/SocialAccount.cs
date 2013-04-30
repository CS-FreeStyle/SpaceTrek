using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrek.Model
{
    
        public class SocialAccount
        {
            public string social_id { get; set; }
            public string access_token { get; set; }
            public string user_name { get; set; }
            public string access_secret { get; set; }
            
            public SocialAccount() { }

            public SocialAccount(string socialId, string accessToken, string userName, string accessSecret)
            {
                social_id = socialId;
                access_token = accessToken;
                user_name = userName;
                access_secret = accessSecret;
            }


        }
    
}
