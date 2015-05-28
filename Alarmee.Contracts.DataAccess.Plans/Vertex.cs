using System.Runtime.Serialization;

namespace Alarmee.Contracts.DataAccess.Plans
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
