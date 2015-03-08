﻿using System;
using System.ServiceModel;

namespace Simulator
{
    class Program
    {
        private static string activePump = "";

        static void Main(string[] args)
        {
            var serviceHost = new ServiceHost(typeof(PumpDataAccess));
			serviceHost.Open();

			Console.WriteLine("Service started.");
			
			writeHelp();

			string commandLine = string.Empty;

			while (true)
			{
				commandLine = Console.ReadLine().Trim();

				if (commandLine.StartsWith("add"))                                          // add new pump
				{
					string[] tokens = commandLine.Split(' ');

					if (tokens.Length != 2)
					{
						Console.WriteLine("Invalid command syntax. Serial number expected.");
						writeHelp();
						continue;
                    }

                    var adminClient = new AdminPumpDataAccessClient();
					adminClient.AddPump(tokens[1]);
					adminClient.Close();
                }
                else if (commandLine.StartsWith("l") || commandLine.Equals("list"))         // list pumps
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    var allPumps = adminClient.GetAllPumps();
                    adminClient.Close();

                    Console.WriteLine("Available pumps:");
                    for (int i = 0; i < allPumps.Count; i++)
                    {
                        string pumpInfo = i + " " + allPumps[i].SerialNumber + " " + allPumps[i].CurrentState + " " + allPumps[i].Medicament + " " + allPumps[i].TotalTime + " " + allPumps[i].RemainingTime;
                        if (allPumps[i].SerialNumber == activePump)
                            pumpInfo += " active";
                        Console.WriteLine(pumpInfo);
                    }
                }
                else if (commandLine.StartsWith("select"))                                  // select pump
                {
                    string[] tokens = commandLine.Split(' ');

                    if (tokens.Length != 2)
                    {
                        Console.WriteLine("Invalid command syntax. Pump number expected.");
                        writeHelp();
                        continue;
                    }

                    var adminClient = new AdminPumpDataAccessClient();
                    var allPumps = adminClient.GetAllPumps();
                    adminClient.Close();
                    
                    activePump = "";
                    int pumpNumber = Convert.ToInt16(tokens[1]);

                    if (pumpNumber >= 0 && pumpNumber < allPumps.Count)
                    {
                        activePump = allPumps[pumpNumber].SerialNumber;
                        Console.WriteLine("Active pump " + pumpNumber);
                        writeHelp();
                    }
                    else
                        Console.WriteLine("Pump number is out of range.");
                }
                else if (commandLine.Equals("on") && activePump != "")                      // turn on pump
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.TurnOn(activePump))
                        Console.WriteLine("Cannot turn on pump.");
                    adminClient.Close();
                }
                else if (commandLine.Equals("off") && activePump != "")                     // turn off pump
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.TurnOff(activePump))
                        Console.WriteLine("Cannot turn off pump.");
                    adminClient.Close();
                }
                else if (commandLine.StartsWith("set") && activePump != "")                 // set pump
                {
                    string[] tokens = commandLine.Split(' ');

                    if (tokens.Length != 4)
                    {
                        Console.WriteLine("Invalid command syntax. Rate, volume and medicament expected.");
                        writeHelp();
                        continue;
                    }

                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.SetInfusionParams(activePump, Convert.ToInt32(tokens[1]), Convert.ToInt32(tokens[2]), tokens[3]))
                        Console.WriteLine("Cannot set pump.");
                    adminClient.Close();
                }
                else if (commandLine.Equals("start") && activePump != "")                   // start infusion
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.StartInfusion(activePump))
                        Console.WriteLine("Cannot start infusion.");
                    adminClient.Close();
                }
                else if (commandLine.Equals("stop") && activePump != "")                    // stop infusion
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.StopInfusion(activePump))
                        Console.WriteLine("Cannot stop infusion.");
                    adminClient.Close();
                }
                else if (commandLine.Equals("accept") && activePump != "")                  // acknowledge alert
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    if (!adminClient.AcknowledgeAlert(activePump))
                        Console.WriteLine("Cannot acknowledge alert.");
                    adminClient.Close();
                }
                else if ((commandLine.StartsWith("i") || commandLine.Equals("info")) && activePump != "")   // get info of selected pump
                {
                    var adminClient = new AdminPumpDataAccessClient();
                    var pump = adminClient.GetPump(activePump);
                    adminClient.Close();

                    if (pump != null)
                        Console.WriteLine(pump.SerialNumber + " " + pump.CurrentState + " " + pump.Medicament + " " + pump.TotalTime + " " + pump.RemainingTime);
                    else
                        Console.WriteLine("Pump not found.");
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
			/*Console.WriteLine("Service stopped.");
            Pump pump = new Pump("51351032");
            pump.TurnOn();
            pump.SetInfusionParams(500, 15, "Morfium");
            pump.StartInfusion();
            Console.ReadLine();*/
        }

        private static void writeHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("   add <serial_number>");
            Console.WriteLine("   list");
            Console.WriteLine("   select <pump_number>");
            if (activePump != "")
            {
                Console.WriteLine("   on");
                Console.WriteLine("   off");
                Console.WriteLine("   set <rate> <volume> <medicament>");
                Console.WriteLine("   start");
                Console.WriteLine("   stop");
                Console.WriteLine("   accept");
                Console.WriteLine("   info");
            }
            Console.WriteLine("   exit");
        }
    }
}