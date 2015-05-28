using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.Contracts.DataAccess.Plans
{
    [DataContract]
    public class Plan
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Room> Rooms { get; set; }

        public Plan()
        {
            Rooms = new List<Room>();
        }
    }
}
