using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.Contracts.Manager.Monitoring
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Vertex NamePosition { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public List<Vertex> Vertices { get; set; }

        public Room()
        {
            Vertices = new List<Vertex>();
        }
    }
}
