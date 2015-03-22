using System.Runtime.Serialization;

namespace Alarmee.WardManager.Contracts
{
	[DataContract]
	public class RemainingTimeInfo
	{
		[DataMember]
		public string Medicament { get; set; }

		[DataMember]
		public int Progress { get; set; }

		[DataMember]
		public string RemainingTime { get; set; }
	}
}
