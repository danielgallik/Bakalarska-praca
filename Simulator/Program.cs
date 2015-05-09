using System;
using System.Configuration;
using System.ServiceModel;

namespace Simulator
{
    class Program
    {
        private static string ActivePump = "";
        private static string DefaultIpAddress = ConfigurationManager.AppSettings["DefaultIpAddress"];

        static void Main(string[] args)
        {
            var serviceHost = new ServiceHost(typeof(PumpDataAccess));
			serviceHost.Open();

			Console.WriteLine("Service started");
			
			writeHelp();

			string commandLine = string.Empty;

			while (true)
			{
				commandLine = Console.ReadLine().Trim();

				if (commandLine.StartsWith("add"))                                          // add new pump
				{
					string[] tokens = commandLine.Split(' ');

					if (tokens.Length < 2 )
					{
						Console.WriteLine("Invalid command syntax. Serial number expected");
						writeHelp();
						continue;
                    }
                    var adminClient = new AdminPumpDataAccessClient();
                    if (tokens.Length == 2)
                    {
                        adminClient.AddPump(tokens[1]);
                    }
                    else
                    {
                        adminClient.AddPump(tokens[1], tokens[2]);
                    }
                    adminClient.ConnectToIpAddress(tokens[1], DefaultIpAddress);
                    adminClient.Close();
                    Console.WriteLine("Pump added");
                }
                else if (commandLine.StartsWith("l") || commandLine.Equals("list"))         // list pumps
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    var allPumps = adminClient.GetAllPumps();
                    adminClient.Close();

                    Console.WriteLine("Available pumps:");
                    for (int i = 0; i < allPumps.Count; i++)
                    {
                        string pumpInfo = string.Format("Serial:{0} IP:{1} State:{2} Medicament:{3} Rem.Time:{4}", allPumps[i].SerialNumber, allPumps[i].IpAddress, allPumps[i].CurrentState, allPumps[i].Medicament, allPumps[i].RemainingTime);
                        if (allPumps[i].SerialNumber == ActivePump)
                            pumpInfo += " active";
                        Console.WriteLine(pumpInfo);
                    }
                }
                else if (commandLine.StartsWith("select"))                                  // select pump
                {
                    string[] tokens = commandLine.Split(' ');

                    if (tokens.Length != 2)
                    {
                        Console.WriteLine("Invalid command syntax. Pump number expected");
                        writeHelp();
                        continue;
                    }

                    var adminClient = new AdminPumpDataAccessClient();
                    var allPumps = adminClient.GetAllPumps();
                    adminClient.Close();
                    
                    ActivePump = "";
                    foreach (var pump in allPumps)
                    {
                        if (tokens[1] == pump.SerialNumber)
                        {
                            ActivePump = pump.SerialNumber;
                        }
                    }

                    if (ActivePump != "")
                    {
                        Console.WriteLine("Active pump " + ActivePump);
                        writeHelp();
                    }
                    else
                    {
                        Console.WriteLine("Serial number not exist");
                    }
                }
                else if (commandLine.Equals("on") && ActivePump != "")                      // turn on pump
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.TurnOn(ActivePump))
                    {
                        Console.WriteLine("Cannot turn on pump");
                    }
                    else
                    {
                        Console.WriteLine("Pump on");
                    }
                    adminClient.Close();
                }
                else if (commandLine.Equals("off") && ActivePump != "")                     // turn off pump
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.TurnOff(ActivePump))
                    {
                        Console.WriteLine("Cannot turn off pump");
                    }
                    else
                    {
                        Console.WriteLine("Pump off");
                    }
                    adminClient.Close();
                }
                else if (commandLine.StartsWith("set") && ActivePump != "")                 // set pump
                {
                    string[] tokens = commandLine.Split(' ');

                    if (tokens.Length < 4)
                    {
                        Console.WriteLine("Invalid command syntax. Rate, volume and medicament expected");
                        writeHelp();
                        continue;
                    }
                    string medicament = "";
                    for (int i = 3; i < tokens.Length; i++)
                    {
                        medicament += " " + tokens[i];
                    }
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.SetInfusionParams(ActivePump, Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]), medicament.Trim()))
                    {
                        Console.WriteLine("Cannot set pump");
                    }
                    else
                    {
                        Console.WriteLine("Pump set");
                    }
                    adminClient.Close();
                }
                else if (commandLine.Equals("start") && ActivePump != "")                   // start infusion
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.StartInfusion(ActivePump))
                    {
                        Console.WriteLine("Cannot start infusion");
                    }
                    else
                    {
                        Console.WriteLine("Pump start");
                    }
                    adminClient.Close();
                }
                else if (commandLine.Equals("stop") && ActivePump != "")                    // stop infusion
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.StopInfusion(ActivePump))
                    {
                        Console.WriteLine("Cannot stop infusion");
                    }
                    else
                    {
                        Console.WriteLine("Pump stop");
                    }
                    adminClient.Close();
                }
                else if (commandLine.Equals("accept") && ActivePump != "")                  // acknowledge alert
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.AcknowledgeAlert(ActivePump))
                    {
                        Console.WriteLine("Cannot acknowledge alert");
                    }
                    else
                    {
                        Console.WriteLine("Pump stop");
                    }
                    adminClient.Close();
                }
                else if (commandLine.StartsWith("connect") && ActivePump != "")                  // connect to other bed
                {
                    string[] tokens = commandLine.Split(' ');

                    if (tokens.Length != 2)
                    {
                        Console.WriteLine("Invalid command syntax. Ip address expected");
                        writeHelp();
                        continue;
                    }

                    var adminClient = new AdminPumpDataAccessClient();
                    adminClient.ConnectToIpAddress(ActivePump, tokens[1]);
                    adminClient.Close();
                    Console.WriteLine("Pump connected to " + tokens[1]);
                }
                else if ((commandLine.StartsWith("i") || commandLine.Equals("info")) && ActivePump != "")   // get info of selected pump
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    var pump = adminClient.GetPump(ActivePump);
                    adminClient.Close();

                    if (pump != null)
                        Console.WriteLine(string.Format("Serial:{0} IP:{1} State:{2} Medicament:{3} Rem.Time:{4}", pump.SerialNumber, pump.IpAddress, pump.CurrentState, pump.Medicament, pump.RemainingTime));
                    else
                        Console.WriteLine("Pump not found");
                }
                else if (commandLine.StartsWith("e") || commandLine.Equals("exit"))         // exit program
                {
                    Console.WriteLine("Bye bye...");
                    break;
                }
				else
				{
					writeHelp();
				}
			}

			serviceHost.Close();
			Console.WriteLine("Service stopped.");
            Console.ReadLine();
        }

        private static void writeHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("   add <serial_number> {infusion,injection}");
            Console.WriteLine("   list");
            Console.WriteLine("   select <serial_number>");
            if (ActivePump != "")
            {
                Console.WriteLine("   on");
                Console.WriteLine("   off");
                Console.WriteLine("   set <rate> <volume> <medicament>");
                Console.WriteLine("   start");
                Console.WriteLine("   stop");
                Console.WriteLine("   accept");
                Console.WriteLine("   connect");
                Console.WriteLine("   info");
            }
            Console.WriteLine("   exit");
        }
    }
}
