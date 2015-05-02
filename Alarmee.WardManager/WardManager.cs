using Alarmee.WardManager.Contracts;
using Alarmee.WardPlan.Contract;
using PumpContract;
using System.Collections.Generic;

namespace Alarmee.WardManager
{
	public class WardManager : IWardManager
	{
        public Dictionary<string, string> getWardPlan()
        {
            var client = new PlanDataAccessClient();
            var planList = client.getPlanList();
            client.Close();

            return planList;
        }

		public WardStateInfo GetWardState(string id)
		{
			// the manager contains no business logic, it is only orchestrating other components;
			// calling data access, passing obtained data to the engine for further processing and returning final ward state

			// get data from data access service
			var pumpDataAccessClient = new PumpDataAccessClient();
			var pumps = pumpDataAccessClient.GetAllPumps();
			pumpDataAccessClient.Close();

            var client = new PlanDataAccessClient();
            var plan = client.getPlan(id);
            client.Close();

			// transform the data
            var remainingTimesEngine = new RemainingTimesEngine();

            var wardState = remainingTimesEngine.getWardStateInfo(plan, pumps);

			return wardState;
		}
	}
}
