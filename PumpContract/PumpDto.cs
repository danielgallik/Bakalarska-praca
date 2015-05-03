using System.Runtime.Serialization;

namespace PumpContract
{
    [DataContract]
    public class PumpDto
    {
        [DataMember]
        public string SerialNumber { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Medicament { get; set; }

        [DataMember]
        public string CurrentState { get; set; }

        [DataMember]
        public int TotalTime { get; set; }

        [DataMember]
        public int RemainingTime { get; set; }

        [DataMember]
        public string AlertMessage { get; set; }
    }
}
