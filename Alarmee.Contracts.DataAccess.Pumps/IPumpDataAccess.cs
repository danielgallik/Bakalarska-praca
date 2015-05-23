using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.Contracts.DataAccess.Pumps
{
    [ServiceContract]
    public interface IPumpDataAccess
    {
        [OperationContract]
        List<PumpDto> GetAllPumps();
    }
}
