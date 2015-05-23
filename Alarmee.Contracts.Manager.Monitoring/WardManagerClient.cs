using System.ServiceModel;
using System.Collections.Generic;

namespace Alarmee.Contracts.Manager.Monitoring
{
	public class WardManagerClient : ClientBase<IWardManager>, IWardManager
    {
        public Dictionary<string, string> GetWardPlans()
        {
            return Channel.GetWardPlans();
        }

        public WardStateInfo GetWardState(string id)
        {
            return Channel.GetWardState(id);
        }
	}
}
