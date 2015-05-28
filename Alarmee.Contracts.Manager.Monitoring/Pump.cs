using System.Runtime.Serialization;

namespace Alarmee.Contracts.Manager.Monitoring
{
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
}
