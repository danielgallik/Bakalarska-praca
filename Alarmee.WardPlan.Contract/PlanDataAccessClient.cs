using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.WardPlan.Contract
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
