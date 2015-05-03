using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.WardManager.Contracts
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Point NamePosition { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public List<Point> Vertices { get; set; }

        public Room()
        {
            Vertices = new List<Point>();
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
        public string State { get; set; }

        [DataMember]
        public List<Point> Vertices { get; set; }

        public Bed()
        {
            Vertices = new List<Point>();
        }
    }

    [DataContract]
    public class Pump
    {
        [DataMember]
        public string Bed { get; set; }

        [DataMember]
        public string RemainingTime { get; set; }

        [DataMember]
        public string Medicament { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Progress { get; set; }

        [DataMember]
        public string State { get; set; }
    }

    [DataContract]
    public class Alert
    {
        [DataMember]
        public string Bed { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Medicament { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string State { get; set; }
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
