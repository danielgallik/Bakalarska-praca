using System.Collections.Generic;
using System.Linq;
using Alarmee.WardManager.Contracts;
using Alarmee.WardPlan.Contract;
using PumpContract;

namespace Alarmee.WardManager
{
	internal class RemainingTimesEngine
    {
        static string stateInitial = "Initial";
        static string stateRunning = "Running";
        static string statePreAlarm = "PreAlarm";
        static string stateAlarm = "Alarm";

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
                roomInfo.State = stateInitial;

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
                    bedInfo.State = stateInitial;

                    foreach (string ipAddress in bed.IpAddresses)
                    {
                        if (pumpDictionary.ContainsKey(ipAddress))
                        {
                            foreach (PumpDto pump in pumpDictionary[ipAddress])
                            {
                                if (pump.CurrentState == stateRunning)
                                {
                                    wardStateInfo.Pumps.Add(new WardStateInfo.Pump()
                                    {
                                        Bed = bed.Name,
                                        Medicament = pump.Medicament,
                                        Progress = CalculateRemainingTimeProgress(pump),
                                        Type = pump.Type,
                                        RemainingTime = FormatRemainingTime(pump),
                                        State = pump.CurrentState
                                    });
                                    if (bedInfo.State == stateInitial)
                                    {
                                        bedInfo.State = stateRunning;
                                    }
                                    if (roomInfo.State == stateInitial)
                                    {
                                        roomInfo.State = stateRunning;
                                    }
                                }
                                else if (pump.CurrentState == statePreAlarm)
                                {
                                    wardStateInfo.Pumps.Add(new WardStateInfo.Pump()
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
                                    wardStateInfo.Alerts.Add(new WardStateInfo.Alert()
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
            return wardStateInfo;
        }

        private int CalculateRemainingTimeProgress(PumpDto pump)
        {
            return (pump.RemainingTime * 100 / pump.TotalTime);
        }

		private string FormatRemainingTime(PumpDto pump)
        {
            if (pump.RemainingTime >= 3600)
            {
                return string.Format("{0} hour", pump.RemainingTime / 3600);
            }
            if (pump.RemainingTime >= 60)
            {
                return string.Format("{0} min", pump.RemainingTime / 60);
            }
			return string.Format("{0} sec", pump.RemainingTime);
		}
	}
}
