using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PumpContract;

namespace Alarmee.WardManager
{
    internal class ArrayConverter
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
