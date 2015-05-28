using System.Runtime.Serialization;

namespace Alarmee.Contracts.Manager.Monitoring
{
    [DataContract]
    public class Vertex
    {
        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }
    }
}
