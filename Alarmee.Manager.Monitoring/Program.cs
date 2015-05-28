using System;
using System.ServiceModel;
using Alarmee.DataAccess.Plans;
using Alarmee.DataAccess.Pumps;
using Alarmee.Engine.Evaluation;

namespace Alarmee.Manager.Monitoring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Alarmee Manager Monitoring";
            var serviceWardManagerHost = new ServiceHost(typeof(MonitoringManager));
            serviceWardManagerHost.Open();
            var servicePlanDataAccessHost = new ServiceHost(typeof(PlanDataAccess));
            servicePlanDataAccessHost.Open();
            var servicePumpDataAccessHost = new ServiceHost(typeof(PumpDataAccess));
            servicePumpDataAccessHost.Open();

            Console.WriteLine("Alarmee Console Server started...");
            Console.ReadLine();

            servicePumpDataAccessHost.Close();
            servicePlanDataAccessHost.Close();
            serviceWardManagerHost.Close();
            Console.WriteLine("Alarmee Console Server stopped...");
        }
    }
}
