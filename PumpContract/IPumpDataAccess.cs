using System.Collections.Generic;
using System.ServiceModel;

namespace PumpContract
{
    [ServiceContract]
    public interface IPumpDataAccess
    {
        [OperationContract]
        List<PumpDto> GetAllPumps();
    }
}
