using System.ServiceModel;
using System.Collections.Generic;

namespace Alarmee.WardManager.Contracts
{
	[ServiceContract]
	public interface IWardManager
    {
        [OperationContract]
        Dictionary<string, string> GetWardPlans();

        [OperationContract]
        WardStateInfo GetWardState(string id);
	}
}
