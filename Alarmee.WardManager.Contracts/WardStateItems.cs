using System.Runtime.Serialization;

namespace Alarmee.WardManager.Contracts
{
    [DataContract]
    public class Room
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public string State { get; set; }
    }

    [DataContract]
    public class Bed
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public string State { get; set; }
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
        public decimal Progress { get; set; }

        [DataMember]
        public string State { get; set; }
    }

    [DataContract]
    public class Alert
    {
        [DataMember]
        public string Bed { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public string State { get; set; }
    }
}
