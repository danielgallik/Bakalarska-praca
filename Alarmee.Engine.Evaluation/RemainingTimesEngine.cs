using System;
using System.Collections.Generic;
using System.Linq;
using Alarmee.Contracts.Manager.Monitoring;
using Alarmee.Contracts.DataAccess.Plans;
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

        public WardStateInfo GetWardStateInfoWithError(string id)
        {
            WardStateInfo wardStateInfo = new WardStateInfo();
            wardStateInfo.Id = id;
            wardStateInfo.Name = "NaN";
            wardStateInfo.SuccessLoad = false;
            wardStateInfo.ErrorMessage = "Cannot load plan. <br /><br /> Please contact your IT administrator.";

            return wardStateInfo;
        }

        public WardStateInfo GetWardStateInfo(Plan plan, Dictionary<string, List<PumpDto>> pumpDictionary)
        {            
            WardStateInfo wardStateInfo = new WardStateInfo();
            wardStateInfo.Id = plan.Id;
            wardStateInfo.Name = plan.Name;
            wardStateInfo.SuccessLoad = true;
            wardStateInfo.ErrorMessage = "";
            Dictionary<int, List<WardStateInfo.Pump>> dicPump = new Dictionary<int, List<WardStateInfo.Pump>>();

            foreach (Plan.Room room in plan.Rooms)
            {
                WardStateInfo.Room roomInfo = new WardStateInfo.Room();
                roomInfo.Name = room.Name;
                roomInfo.NamePosition = new WardStateInfo.Point()
                {
                    X = room.NamePosition.X,
                    Y = room.NamePosition.Y
                };
                room.Vertices.ForEach(v => roomInfo.Vertices.Add(new WardStateInfo.Point()
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

                foreach (Plan.Bed bed in room.Beds)
                {
                    WardStateInfo.Bed bedInfo = new WardStateInfo.Bed();
                    bedInfo.Name = bed.Name;
                    bedInfo.NamePosition = new WardStateInfo.Point()
                    {
                        X = bed.NamePosition.X,
                        Y = bed.NamePosition.Y
                    };
                    bed.Vertices.ForEach(v => bedInfo.Vertices.Add(new WardStateInfo.Point()
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
                                        dicPump.Add(pump.RemainingTime, new List<WardStateInfo.Pump>());
                                    }
                                    dicPump[pump.RemainingTime].Add(new WardStateInfo.Pump()
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
                                        dicPump.Add(pump.RemainingTime, new List<WardStateInfo.Pump>());
                                    }
                                    dicPump[pump.RemainingTime].Add(new WardStateInfo.Pump()
                                    {
                                        Bed = bed.Name,
                                        Medicament = pump.Medicament,
                                        Progress = CalculateRemainingTimeProgress(pump),
                                        Type = pump.Type,
                                        RemainingTime = FormatRemainingTime(pump),
                                        State = pump.CurrentState
                                    });
                                    wardStateInfo.Alerts.Add(new WardStateInfo.Alert()
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
                                    wardStateInfo.Alerts.Insert(0, new WardStateInfo.Alert()
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
                    wardStateInfo.Beds.Add(bedInfo);
                }
                wardStateInfo.Rooms.Add(roomInfo);
            }
            wardStateInfo.Pumps = dicPump.OrderBy(p => p.Key).SelectMany(p => p.Value).ToList();
            return wardStateInfo;
        }

        private double CalculateRemainingTimeProgress(PumpDto pump)
        {
            return ((double)pump.RemainingTime * 100 / pump.TotalTime);
        }

		private string FormatRemainingTime(PumpDto pump)
        {
            if (pump.RemainingTime >= 3600)
            {
                return string.Format("{0} hour", Math.Round((double)pump.RemainingTime / 3600, 2));
            }
            if (pump.RemainingTime >= 60)
            {
                return string.Format("{0} min", Math.Round((double)pump.RemainingTime / 60, 2));
            }
			return string.Format("{0} sec", pump.RemainingTime);
		}
	}
}
