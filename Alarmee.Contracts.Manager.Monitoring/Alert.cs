using System.Runtime.Serialization;

namespace Alarmee.Contracts.Manager.Monitoring
{
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
}
