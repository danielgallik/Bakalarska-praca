using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.Contracts.DataAccess.Plans
{
    public class PlanDataAccessClient : ClientBase<IPlanDataAccess>, IPlanDataAccess
    {
        public Dictionary<string, string> GetPlanList()
        {
            return Channel.GetPlanList();
        }

        public Plan GetPlan(string id)
        {
            return Channel.GetPlan(id);
        }
    }
}
