using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using PumpContract;

namespace Simulator
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class PumpDataAccess : IPumpDataAccess, IAdminPumpDataAccess
    {
        private static Dictionary<string,Pump> myPumps;
        
        static PumpDataAccess()
        {
            myPumps = new Dictionary<string, Pump>();
        }

        public bool AddPump(string serialNumber)
        {
            if (myPumps.ContainsKey(serialNumber))
                return false;
            myPumps.Add(serialNumber, new Pump(serialNumber));
            return true;
        }

        public bool TurnOn(string serialNumber)
        {
            Pump pump;
            if (myPumps.TryGetValue(serialNumber, out pump))
            {
                return pump.TurnOn();
            }
            return false;
        }

        public bool TurnOff(string serialNumber)
        {
            Pump pump;
            if (myPumps.TryGetValue(serialNumber, out pump))
            {
                return pump.TurnOff();
            }
            return false;
        }

        public bool SetInfusionParams(string serialNumber, int rate, int volume, string medicament)
        {
            Pump pump;
            if (myPumps.TryGetValue(serialNumber, out pump))
            {
                return pump.SetInfusionParams(rate, volume, medicament);
            }
            return false;
        }

        public bool StartInfusion(string serialNumber)
        {
            Pump pump;
            if (myPumps.TryGetValue(serialNumber, out pump))
            {
                return pump.StartInfusion();
            }
            return false;
        }

        public bool StopInfusion(string serialNumber)
        {
            Pump pump;
            if (myPumps.TryGetValue(serialNumber, out pump))
            {
                return pump.StopInfusion();
            }
            return false;
        }

        public bool AcknowledgeAlert(string serialNumber)
        {
            Pump pump;
            if (myPumps.TryGetValue(serialNumber, out pump))
            {
                return pump.AcknowledgeAlert();
            }
            return false;
        }

        public PumpDto GetPump(string serialNumber)
        {
            Pump pump;
            if (myPumps.TryGetValue(serialNumber, out pump))
            {
                PumpDto pumpDto = new PumpDto();
                pumpDto.SerialNumber = pump.SerialNumber;
                pumpDto.Medicament = pump.Medicament;
                pumpDto.CurrentState = pump.CurrentState;
                pumpDto.TotalTime = pump.TotalTime;
                pumpDto.RemainingTime = pump.RemainingTime;
                return pumpDto;
            }
            return null;
        }

        public List<PumpDto> GetAllPumps()
        {
            return myPumps.Select(x => new PumpDto
            {
                SerialNumber = x.Value.SerialNumber,
                Medicament = x.Value.Medicament,
                CurrentState = x.Value.CurrentState,
                TotalTime = x.Value.TotalTime,
                RemainingTime = x.Value.RemainingTime
            }).ToList();
        }
    }
}
