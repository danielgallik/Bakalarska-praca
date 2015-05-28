using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.Contracts.DataAccess.Plans
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Vertex NamePosition { get; set; }

        [DataMember]
        public List<Vertex> Vertices { get; set; }

        [DataMember]
        public List<Bed> Beds { get; set; }

        public Room()
        {
            Vertices = new List<Vertex>();
            Beds = new List<Bed>();
        }
    }
}
