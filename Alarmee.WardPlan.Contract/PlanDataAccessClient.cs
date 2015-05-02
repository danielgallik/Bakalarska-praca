using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.WardPlan.Contract
{
    public class PlanDataAccessClient : ClientBase<IPlanDataAccess>, IPlanDataAccess
    {
        public Dictionary<string, string> getPlanList()
        {
            return Channel.getPlanList();
        }

        public Plan getPlan(string id)
        {
            return Channel.getPlan(id);
        }
    }
}
