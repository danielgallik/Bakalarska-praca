using System.Collections.Generic;
using Alarmee.Contracts.DataAccess.Pumps;

namespace Alarmee.Engine.Evaluation
{
    public class ArrayConverter
    {
        public Dictionary<string, List<PumpDto>> PumpListToDictionary(List<PumpDto> pumpList)
        {
            Dictionary<string, List<PumpDto>> pumpIpAddresses = new Dictionary<string, List<PumpDto>>();
            foreach (PumpDto pump in pumpList)
            {
                if (!pumpIpAddresses.ContainsKey(pump.IpAddress))
                {
                    pumpIpAddresses[pump.IpAddress] = new List<PumpDto>();
                }
                pumpIpAddresses[pump.IpAddress].Add(pump);
            }
            return pumpIpAddresses;
        }
    }
}
