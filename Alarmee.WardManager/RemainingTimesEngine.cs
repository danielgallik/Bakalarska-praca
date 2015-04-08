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

        public WardStateInfo getWardStateInfo(Plan plan, List<PumpDto> pumps)
        {
            Dictionary<string,List<PumpDto>> pumpIpAddresses = new Dictionary<string,List<PumpDto>>();
            foreach(PumpDto pump in pumps){
                if(!pumpIpAddresses.ContainsKey(pump.IpAddress)){
                    pumpIpAddresses[pump.IpAddress] = new List<PumpDto>();
                }
                pumpIpAddresses[pump.IpAddress].Add(pump);
            }


            WardStateInfo wardStateInfo = new WardStateInfo();
            foreach (Plan.Room room in plan.Rooms)
            {
                Room roomInfo = new Room();
                roomInfo.Name = room.Name;
                roomInfo.NamePosition = new Point()
                {
                    X = room.NamePosition.X,
                    Y = room.NamePosition.Y
                };
                room.Vertices.ForEach(v => roomInfo.Vertices.Add(new Point(){
                    X = v.X,
                    Y = v.Y
                }));
                roomInfo.State = stateInitial;

                foreach (Plan.Bed bed in room.Beds)
                {
                    Bed bedInfo = new Bed();
                    bedInfo.Name = bed.Name;
                    bedInfo.NamePosition = new Point()
                    {
                        X = bed.NamePosition.X,
                        Y = bed.NamePosition.Y
                    };
                    bed.Vertices.ForEach(v => bedInfo.Vertices.Add(new Point()
                    {
                        X = v.X,
                        Y = v.Y
                    }));
                    bedInfo.State = stateInitial;

                    foreach (string ipAddress in bed.IpAddresses)
                    {
                        if (pumpIpAddresses.ContainsKey(ipAddress))
                        {
                            foreach (PumpDto pump in pumpIpAddresses[ipAddress])
                            {
                                if (pump.CurrentState == stateRunning)
                                {
                                    wardStateInfo.Pumps.Add(new Pump()
                                    {
                                        Bed = bed.Name,
                                        Medicament = pump.Medicament,
                                        Progress = calculateRemainingTimeProgress(pump),
                                        RemainingTime = formatRemainingTime(pump),
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
                                    wardStateInfo.Pumps.Add(new Pump()
                                    {
                                        Bed = bed.Name,
                                        Medicament = pump.Medicament,
                                        Progress = calculateRemainingTimeProgress(pump),
                                        RemainingTime = formatRemainingTime(pump),
                                        State = pump.CurrentState
                                    });
                                    wardStateInfo.Alerts.Add(new Alert()
                                    {
                                        Bed = bed.Name,
                                        Label = "PreAlarm",
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
                                    wardStateInfo.Alerts.Add(new Alert()
                                    {
                                        Bed = bed.Name,
                                        Label = "Alarm",
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

        private decimal calculateRemainingTimeProgress(PumpDto pump)
        {
            return ((decimal)pump.RemainingTime / pump.TotalTime);
        }

		private string formatRemainingTime(PumpDto pump)
		{
			// TODO: needs to be impl. according spec.
			return string.Format("{0} s", pump.RemainingTime);
		}
	}
}
