using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrek.Model
{
    public class FBUser
    {
        public string name { get; set; }
        public string id { get; set; }
        public string image { get; set; }
        public string username { get; set; }
    }

    public class UserParisVanJavaArgs
    {
            public int id { get; set; }
        
    }

    public class EventUserEventArgs : System.EventArgs
    {
        public string EventId { get; set; }

        public EventUserEventArgs()
        { 
            
        }

        public EventUserEventArgs(string id)
        {
            this.EventId = id;
        }
    }


    public class StreamUserEventArgs : System.EventArgs
    {
        public string sequence { get; set; }

        public StreamUserEventArgs()
        {

        }

        public StreamUserEventArgs(string id)
        {
            this.sequence = id;
        }
    }


    public class ChannelUserEventArgs : System.EventArgs
    {
        public int EventId { get; set; }

        public ChannelUserEventArgs()
        {

        }

        public ChannelUserEventArgs(string id)
        {
            this.EventId = Int32.Parse( id );
        }
    }

    public class FacebookUserEventArgs : System.EventArgs
    {
        public List<FBUser> Users { get; set; }

        public FacebookUserEventArgs()
        {
            Users = new List<FBUser>();

        }

        public FacebookUserEventArgs(FBRootObject rootObject)
        {
            Users = new List<FBUser>(rootObject.data);

        }

    }

    public class Paging
    {
        public string next { get; set; }
    }

    public class FBRootObject
    {
        public List<FBUser> data { get; set; }
        public Paging paging { get; set; }
    }

    public class EventRootObject
    {

            public string status { get; set; }
            public string event_id { get; set; }

    }

    public class ChannelRootObject
    {
            public string status { get; set; }
            public string id { get; set; }
    }

   

    public class StreamDataRootObject
    {
        public string name { get; set; }
        public string user { get; set; }
        public string date { get; set; }
        public List<ObjectStream> stream { get; set; }
    }
}
