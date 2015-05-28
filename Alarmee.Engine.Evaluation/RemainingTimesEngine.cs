using System;
using System.Collections.Generic;
using System.Linq;
using Manager = Alarmee.Contracts.Manager.Monitoring;
using Plans = Alarmee.Contracts.DataAccess.Plans;
using Alarmee.Contracts.DataAccess.Pumps;

namespace Alarmee.Engine.Evaluation
{
	public class RemainingTimesEngine
    {
        private static string stateNotConfigured = "NotConfigured";
        private static string stateInactive = "Inactive";
        private static string stateRunning = "Running";
        private static string stateOk = "Ok";
        private static string statePreAlarm = "PreAlarm";
        private static string stateAlarm = "Alarm";

        public Manager.MonitoringInfo GetWardStateInfoWithError(string id)
        {
            Manager.MonitoringInfo monitoringInfo = new Manager.MonitoringInfo();
            monitoringInfo.Id = id;
            monitoringInfo.Name = "NaN";
            monitoringInfo.SuccessLoad = false;
            monitoringInfo.ErrorMessage = "Cannot load plan. <br /><br /> Please contact your IT administrator.";

            return monitoringInfo;
        }

        public Manager.MonitoringInfo GetWardStateInfo(Plans.Plan plan, Dictionary<string, List<PumpDto>> pumpDictionary)
        {
            Manager.MonitoringInfo monitoringInfo = new Manager.MonitoringInfo();
            monitoringInfo.Id = plan.Id;
            monitoringInfo.Name = plan.Name;
            monitoringInfo.SuccessLoad = true;
            monitoringInfo.ErrorMessage = "";
            Dictionary<int, List<Manager.Pump>> dicPump = new Dictionary<int, List<Manager.Pump>>();

            foreach (Plans.Room room in plan.Rooms)
            {
                Manager.Room roomInfo = new Manager.Room();
                roomInfo.Name = room.Name;
                roomInfo.NamePosition = new Manager.Vertex()
                {
                    X = room.NamePosition.X,
                    Y = room.NamePosition.Y
                };
                room.Vertices.ForEach(v => roomInfo.Vertices.Add(new Manager.Vertex()
                {
                    X = v.X,
                    Y = v.Y
                }));
                if (room.Beds.Count > 0)
                {
                    roomInfo.State = stateOk;
                }
                else
                {
                    roomInfo.State = stateNotConfigured;
                }

                foreach (Plans.Bed bed in room.Beds)
                {
                    Manager.Bed bedInfo = new Manager.Bed();
                    bedInfo.Name = bed.Name;
                    bedInfo.NamePosition = new Manager.Vertex()
                    {
                        X = bed.NamePosition.X,
                        Y = bed.NamePosition.Y
                    };
                    bed.Vertices.ForEach(v => bedInfo.Vertices.Add(new Manager.Vertex()
                    {
                        X = v.X,
                        Y = v.Y
                    }));
                    if (bed.IpAddresses.Count > 0)
                    {
                        bedInfo.State = stateInactive;
                    }
                    else
                    {
                        bedInfo.State = stateNotConfigured;
                    }

                    foreach (string ipAddress in bed.IpAddresses)
                    {
                        if (pumpDictionary.ContainsKey(ipAddress))
                        {
                            foreach (PumpDto pump in pumpDictionary[ipAddress])
                            {
                                if (pump.CurrentState == stateRunning)
                                {
                                    if (!dicPump.ContainsKey(pump.RemainingTime))
                                    {
                                        dicPump.Add(pump.RemainingTime, new List<Manager.Pump>());
                                    }
                                    dicPump[pump.RemainingTime].Add(new Manager.Pump()
                                    {
                                        Bed = bed.Name,
                                        Medicament = pump.Medicament,
                                        Progress = CalculateRemainingTimeProgress(pump),
                                        Type = pump.Type,
                                        RemainingTime = FormatRemainingTime(pump),
                                        State = pump.CurrentState
                                    });
                                    if (bedInfo.State != statePreAlarm && bedInfo.State != stateAlarm)
                                    {
                                        bedInfo.State = stateRunning;
                                    }
                                    if (roomInfo.State != statePreAlarm && roomInfo.State != stateAlarm)
                                    {
                                        roomInfo.State = stateRunning;
                                    }
                                }
                                else if (pump.CurrentState == statePreAlarm)
                                {
                                    if (!dicPump.ContainsKey(pump.RemainingTime))
                                    {
                                        dicPump.Add(pump.RemainingTime, new List<Manager.Pump>());
                                    }
                                    dicPump[pump.RemainingTime].Add(new Manager.Pump()
                                    {
                                        Bed = bed.Name,
                                        Medicament = pump.Medicament,
                                        Progress = CalculateRemainingTimeProgress(pump),
                                        Type = pump.Type,
                                        RemainingTime = FormatRemainingTime(pump),
                                        State = pump.CurrentState
                                    });
                                    monitoringInfo.Alerts.Add(new Manager.Alert()
                                    {
                                        Bed = bed.Name,
                                        Message = pump.AlertMessage,
                                        Medicament = pump.Medicament,
                                        Type = pump.Type,
                                        State = pump.CurrentState
                                    });
                                    if (bedInfo.State != stateAlarm)
                                    {
                                        bedInfo.State = statePreAlarm;
                                    }
                                    if (roomInfo.State != stateAlarm)
                                    {
                                        roomInfo.State = statePreAlarm;
                                    }
                                }
                                else if (pump.CurrentState == stateAlarm)
                                {
                                    monitoringInfo.Alerts.Insert(0, new Manager.Alert()
                                    {
                                        Bed = bed.Name,
                                        Message = pump.AlertMessage,
                                        Medicament = pump.Medicament,
                                        Type = pump.Type,
                                        State = pump.CurrentState
                                    });
                                    bedInfo.State = stateAlarm;
                                    roomInfo.State = stateAlarm;
                                }
                            }
                        }
                    }
                    monitoringInfo.Beds.Add(bedInfo);
                }
                monitoringInfo.Rooms.Add(roomInfo);
            }
            monitoringInfo.Pumps = dicPump.OrderBy(p => p.Key).SelectMany(p => p.Value).ToList();
            return monitoringInfo;
        }

        private double CalculateRemainingTimeProgress(PumpDto pump)
        {
            return Math.Round(((double)pump.RemainingTime * 100 / pump.TotalTime),1);
        }

		private string FormatRemainingTime(PumpDto pump)
        {
            if (pump.RemainingTime >= (60 * 60 * 24))
            {
                return string.Format("{0} day", pump.RemainingTime / (60 * 60 * 24));
            }
            if (pump.RemainingTime >= (60 * 60))
            {
                return string.Format("{0} hour", pump.RemainingTime / (60 * 60));
            }
            if (pump.RemainingTime >= 60)
            {
                return string.Format("{0} min", pump.RemainingTime / 60);
            }
			return string.Format("{0} sec", pump.RemainingTime);
		}
	}
}
