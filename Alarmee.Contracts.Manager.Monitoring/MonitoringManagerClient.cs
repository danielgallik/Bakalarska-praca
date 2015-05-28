using System.ServiceModel;
using System.Collections.Generic;

namespace Alarmee.Contracts.Manager.Monitoring
{
	public class MonitoringManagerClient : ClientBase<IMonitoringManager>, IMonitoringManager
    {
        public Dictionary<string, string> GetWardPlans()
        {
            return Channel.GetWardPlans();
        }

        public MonitoringInfo GetWardState(string id)
        {
            return Channel.GetWardState(id);
        }
	}
}
