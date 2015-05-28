using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.Contracts.DataAccess.Plans
{
    [DataContract]
    public class Bed
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Vertex NamePosition { get; set; }

        [DataMember]
        public List<string> IpAddresses { get; set; }

        [DataMember]
        public List<Vertex> Vertices { get; set; }

        public Bed()
        {
            IpAddresses = new List<string>();
            Vertices = new List<Vertex>();
        }
    }
}
