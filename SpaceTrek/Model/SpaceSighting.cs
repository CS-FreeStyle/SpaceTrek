using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SpaceTrek.Model
{
    public class SpaceSighting
    {
        public int Id { get; set; }

        [DataMember(Name = "id_object")]
        public int Idobject { get; set; }
        
        [DataMember(Name = "name")]
        public string Name { get; set; }
        
        [DataMember(Name="duration")]
        public string Duration { get; set; }
    }
}
