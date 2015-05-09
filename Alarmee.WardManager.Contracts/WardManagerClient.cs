using System.ServiceModel;
using System.Collections.Generic;

namespace Alarmee.WardManager.Contracts
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
