using Alarmee.WardManager.Contracts;
using PumpContract;

namespace Alarmee.WardManager
{
	public class WardManager : IWardManager
	{
		public WardStateInfo GetWardState()
		{
			// the manager contains no business logic, it is only orchestrating other components;
			// calling data access, passing obtained data to the engine for further processing and returning final ward state

			// get data from data access service
			var pumpDataAccessClient = new PumpDataAccessClient();
			var pumps = pumpDataAccessClient.GetAllPumps();
			pumpDataAccessClient.Close();

			// transform the data
			var remainingTimesEngine = new RemainingTimesEngine();
			var wardEngine = new WardEngine();

			var wardState = new WardStateInfo
			{
				RemainingTimes = remainingTimesEngine.GetRemainingTimes(pumps),
                Rooms = wardEngine.GetRooms(pumps),
                Beds = wardEngine.GetBeds(pumps),
                Pumps = wardEngine.GetPumps(pumps),
                Alerts = wardEngine.GetAlerts(pumps)
			};

			return wardState;
		}
	}
}
