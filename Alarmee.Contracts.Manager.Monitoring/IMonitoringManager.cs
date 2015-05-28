using System.ServiceModel;
using System.Collections.Generic;

namespace Alarmee.Contracts.Manager.Monitoring
{
	[ServiceContract]
	public interface IMonitoringManager
    {
        [OperationContract]
        Dictionary<string, string> GetWardPlans();

        [OperationContract]
        MonitoringInfo GetWardState(string id);
	}
}
