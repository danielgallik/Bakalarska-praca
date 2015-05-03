using System.Collections.Generic;
using System.ServiceModel;
using PumpContract;

namespace Simulator
{
    [ServiceContract]
    public interface IAdminPumpDataAccess
    {
        [OperationContract]
        bool AddPump(string serialNumber, string type);

        [OperationContract]
        void ConnectToIpAddress(string serialNumber, string ipAddress);

        [OperationContract]
        bool TurnOn(string serialNumber);

        [OperationContract]
        bool TurnOff(string serialNumber);

        [OperationContract]
        bool SetInfusionParams(string serialNumber, int rate, int volume, string medicament);

        [OperationContract]
        bool StartInfusion(string serialNumber);

        [OperationContract]
        bool StopInfusion(string serialNumber);

        [OperationContract]
        bool AcknowledgeAlert(string serialNumber);

        [OperationContract]
        PumpDto GetPump(string serialNumber);

        [OperationContract]
        List<PumpDto> GetAllPumps();
    }
}
