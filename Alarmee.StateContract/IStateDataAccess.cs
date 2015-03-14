using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.StateContract
{
    [ServiceContract]
    public interface IStateDataAccess
    {
        [OperationContract]
        List<PumpState> GetAllPumps();
    }
}
