using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.Contracts.DataAccess.Pumps
{
    public class PumpDataAccessClient : ClientBase<IPumpDataAccess>, IPumpDataAccess
    {
        public List<PumpDto> GetAllPumps()
        {
            return Channel.GetAllPumps();
        }
    }
}
