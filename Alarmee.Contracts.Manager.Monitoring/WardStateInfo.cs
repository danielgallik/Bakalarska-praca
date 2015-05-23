using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.Contracts.Manager.Monitoring
{
	[DataContract]
	public class WardStateInfo
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool SuccessLoad { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public List<Bed> Beds { get; set; }

        [DataMember]
        public List<Room> Rooms { get; set; }

        [DataMember]
        public List<Pump> Pumps { get; set; }

        [DataMember]
        public List<Alert> Alerts { get; set; }

		public WardStateInfo()
		{
            Beds = new List<Bed>();
            Rooms = new List<Room>();
            Pumps = new List<Pump>();
            Alerts = new List<Alert>();
		}

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
            public double Progress { get; set; }

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
}
