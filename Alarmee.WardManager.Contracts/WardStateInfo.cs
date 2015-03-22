using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Alarmee.WardManager.Contracts
{
	[DataContract]
	public class WardStateInfo
	{
		[DataMember]
		public IList<RemainingTimeInfo> RemainingTimes { get; set; }

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
            RemainingTimes = new List<RemainingTimeInfo>();
            Beds = new List<Bed>();
            Rooms = new List<Room>();
            Pumps = new List<Pump>();
            Alerts = new List<Alert>();
		}
	}
}
