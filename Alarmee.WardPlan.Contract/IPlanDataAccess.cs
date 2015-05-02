using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.WardPlan.Contract
{
    [ServiceContract]
    public interface IPlanDataAccess
    {
        [OperationContract]
        Dictionary<string, string> getPlanList();

        [OperationContract]
        Plan getPlan(string id);
    }
}
