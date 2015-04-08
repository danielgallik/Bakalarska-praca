using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.WardPlan.Contract
{
    [ServiceContract]
    public interface IPlanDataAccess
    {
        [OperationContract]
        Plan getPlan(int id);
    }
}
