using System.ServiceModel;

namespace Alarmee.WardManager.Contracts
{
	public class WardManagerClient : ClientBase<IWardManager>, IWardManager
	{
		public WardStateInfo GetWardState()
		{
			return Channel.GetWardState();
		}
	}
}
