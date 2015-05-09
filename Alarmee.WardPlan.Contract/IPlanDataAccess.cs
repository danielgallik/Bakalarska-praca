using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.WardPlan.Contract
{
    [ServiceContract]
    public interface IPlanDataAccess
    {
        [OperationContract]
        Dictionary<string, string> GetPlanList();

        [OperationContract]
        Plan GetPlan(string id);
    }
}
