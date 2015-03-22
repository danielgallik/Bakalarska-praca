using System.ServiceModel;

namespace Alarmee.WardManager.Contracts
{
	[ServiceContract]
	public interface IWardManager
	{
		[OperationContract]
		WardStateInfo GetWardState();
	}
}
