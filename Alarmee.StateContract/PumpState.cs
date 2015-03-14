using System.Runtime.Serialization;

namespace Alarmee.StateContract
{
    [DataContract]
    public class PumpState
    {
        [DataMember]
        public string Medicament { get; set; }

        [DataMember]
        public string CurrentState { get; set; }

        [DataMember]
        public int Progres { get; set; }

        [DataMember]
        public string RemainingTime { get; set; }
    }
}
