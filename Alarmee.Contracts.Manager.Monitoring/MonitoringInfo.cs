using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.Contracts.Manager.Monitoring
{
	[DataContract]
	public class MonitoringInfo
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

		public MonitoringInfo()
		{
            Beds = new List<Bed>();
            Rooms = new List<Room>();
            Pumps = new List<Pump>();
            Alerts = new List<Alert>();
		}
	}
}
