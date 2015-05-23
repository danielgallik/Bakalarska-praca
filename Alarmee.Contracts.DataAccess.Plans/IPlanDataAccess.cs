using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.Contracts.DataAccess.Plans
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
