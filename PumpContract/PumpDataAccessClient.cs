using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace PumpContract
{
    public class PumpDataAccessClient : ClientBase<IPumpDataAccess>, IPumpDataAccess
    {
        public List<PumpDto> GetAllPumps()
        {
            return Channel.GetAllPumps();
        }
    }
}
