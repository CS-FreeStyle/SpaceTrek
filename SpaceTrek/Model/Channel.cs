using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTrek.Model
{
    public class Channel
    {
        public int Id { get; set; }


        [DataMember(Name = "uri")]
        public string Uri { get; set; }


    }
}
