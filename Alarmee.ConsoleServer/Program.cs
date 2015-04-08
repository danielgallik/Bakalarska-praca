using System;
using System.ServiceModel;

namespace Alarmee.ConsoleServer
{
	class Program
	{
		static void Main(string[] args)
		{
			var serviceHost = new ServiceHost(typeof(WardManager.WardManager));
			serviceHost.Open();
            var service2Host = new ServiceHost(typeof(WardPlan.PlanDataAccess));
            service2Host.Open();

			Console.WriteLine("Alarmee Console Server started...");
			Console.ReadLine();

            service2Host.Close();
            serviceHost.Close();
			Console.WriteLine("Alarmee Console Server stopped...");
		}
	}
}
