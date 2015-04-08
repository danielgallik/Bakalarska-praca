using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Alarmee.WardPlan.Contract
{
    public class PlanDataAccessClient : ClientBase<IPlanDataAccess>, IPlanDataAccess
    {
        public Plan getPlan(int id)
        {
            return Channel.getPlan(id);
        }
    }
}
