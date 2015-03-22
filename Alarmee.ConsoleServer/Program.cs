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

			Console.WriteLine("Alarmee Console Server started...");
			Console.ReadLine();

			serviceHost.Close();
			Console.WriteLine("Alarmee Console Server stopped...");
		}
	}
}
