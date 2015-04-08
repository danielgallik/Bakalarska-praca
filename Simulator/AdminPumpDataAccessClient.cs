using System.Collections.Generic;
using System.ServiceModel;
using PumpContract;

namespace Simulator
{
    public class AdminPumpDataAccessClient : ClientBase<IAdminPumpDataAccess>, IAdminPumpDataAccess
    {
        public bool AddPump(string serialNumber)
        {
            return Channel.AddPump(serialNumber);
        }

        public void ConnectToIpAddress(string serialNumber, string ipAddress)
        {
            Channel.ConnectToIpAddress(serialNumber, ipAddress);
        }

        public bool TurnOn(string serialNumber)
        {
            return Channel.TurnOn(serialNumber);
        }

        public bool TurnOff(string serialNumber)
        {
            return Channel.TurnOff(serialNumber);
        }

        public bool SetInfusionParams(string serialNumber, int rate, int volume, string medicament)
        {
            return Channel.SetInfusionParams(serialNumber, rate, volume, medicament);
        }

        public bool StartInfusion(string serialNumber)
        {
            return Channel.StartInfusion(serialNumber);
        }

        public bool StopInfusion(string serialNumber)
        {
            return Channel.StopInfusion(serialNumber);
        }

        public bool AcknowledgeAlert(string serialNumber)
        {
            return Channel.AcknowledgeAlert(serialNumber);
        }

        public PumpDto GetPump(string serialNumber)
        {
            return Channel.GetPump(serialNumber);
        }

        public List<PumpDto> GetAllPumps()
        {
            return Channel.GetAllPumps();
        }
    }
}
