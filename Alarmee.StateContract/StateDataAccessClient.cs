using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.StateContract
{
    class StateDataAccessClient : ClientBase<IStateDataAccess>, IStateDataAccess
    {
        public List<PumpState> GetAllPumps()
        {
            return Channel.GetAllPumps();
        }
    }
}
