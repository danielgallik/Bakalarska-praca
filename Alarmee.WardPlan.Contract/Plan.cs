using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.WardPlan.Contract
{
    [DataContract]
    public class Plan
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Room> Rooms { get; set; }

        public Plan()
        {
            Rooms = new List<Room>();
        }



        [DataContract]
        public class Room
        {
            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public Point NamePosition { get; set; }

            [DataMember]
            public List<Point> Vertices { get; set; }

            [DataMember]
            public List<Bed> Beds { get; set; }

            public Room()
            {
                Vertices = new List<Point>();
                Beds = new List<Bed>();
            }
        }

        [DataContract]
        public class Bed
        {
            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public Point NamePosition { get; set; }

            [DataMember]
            public List<string> IpAddresses { get; set; }

            [DataMember]
            public List<Point> Vertices { get; set; }

            public Bed()
            {
                IpAddresses = new List<string>();
                Vertices = new List<Point>();
            }
        }

        [DataContract]
        public class Point
        {
            [DataMember]
            public int X { get; set; }

            [DataMember]
            public int Y { get; set; }
        }
    }
}
