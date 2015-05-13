﻿using Alarmee.WardManager.Contracts;
using Alarmee.WardPlan.Contract;
using PumpContract;
using System.Collections.Generic;

namespace Alarmee.WardManager
{
	public class WardManager : IWardManager
	{
        public Dictionary<string, string> GetWardPlans()
        {
            try
            {
                var client = new PlanDataAccessClient();
                var planList = client.GetPlanList();
                client.Close();

                return planList;
            }
            catch
            {
                return null;
            }
        }

		public WardStateInfo GetWardState(string id)
        {
            ArrayConverter arrayConverter = new ArrayConverter();
            RemainingTimesEngine remainingTimesEngine = new RemainingTimesEngine();

            try
            {
                var client = new PlanDataAccessClient();
                var plan = client.GetPlan(id);
                client.Close();
                
                try
                {
                    var pumpDataAccessClient = new PumpDataAccessClient();
                    var pumps = pumpDataAccessClient.GetAllPumps();
                    pumpDataAccessClient.Close();

                    Dictionary<string, List<PumpDto>> pumpDictionary = arrayConverter.PumpListToDictionary(pumps);

                    return remainingTimesEngine.GetWardStateInfo(plan, pumpDictionary);
                }
                catch
                {
                    return remainingTimesEngine.GetWardStateInfo(plan, new Dictionary<string, List<PumpDto>>()); ;
                }
            }
            catch
            {
                return remainingTimesEngine.GetWardStateInfoWithError(id);
            }
		}
	}
}
