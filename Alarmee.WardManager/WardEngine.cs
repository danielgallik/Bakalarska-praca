using System.Collections.Generic;
using System.Linq;
using Alarmee.WardManager.Contracts;
using PumpContract;

namespace Alarmee.WardManager
{
    internal class WardEngine
    {
        public List<Room> GetRooms(List<PumpDto> pumps)
        {
            var result = new List<Room>();

            Room room = new Room 
            { 
                Id = 0,
                Label = "Room 1",
                State = "Running"
            };

            if (pumps.Any(x => x.CurrentState.Equals("PreAlarm")))
            {
                room.State = "PreAlarm";
            }

            if (pumps.Any(x => x.CurrentState.Equals("Alarm")))
            {
                room.State = "Alarm";
            }

            result.Add(room);

            return result;
        }

        public List<Bed> GetBeds(List<PumpDto> pumps)
        {
            var result = new List<Bed>();

            Bed bed = new Bed
            {
                Id = 0,
                Label = "001",
                State = "Running"
            };

            if (pumps.Any(x => x.CurrentState.Equals("PreAlarm")))
            {
                bed.State = "PreAlarm";
            }

            if (pumps.Any(x => x.CurrentState.Equals("Alarm")))
            {
                bed.State = "Alarm";
            }

            result.Add(bed);

            return result;
        }

        public List<Pump> GetPumps(List<PumpDto> pumps)
        {
            var result = new List<Pump>();

            pumps.Where(x => x.CurrentState.Equals("Running") || x.CurrentState.Equals("PreAlarm")).ToList().ForEach(x => result.Add(new Pump
            {
                Medicament = x.Medicament,
                Progress = calculateRemainingTimeProgress(x),
                RemainingTime = formatRemainingTime(x),
                Bed = "001",
                State = x.CurrentState
            }));

            return result;
        }

        public List<Alert> GetAlerts(List<PumpDto> pumps)
        {
            var result = new List<Alert>();

            pumps.Where(x => x.CurrentState.Equals("PreAlarm") || x.CurrentState.Equals("Alarm")).ToList().ForEach(x => result.Add(new Alert
            {
                Bed = "001",
                Label = "Pump alert",
                State = x.CurrentState
            }));

            return result;
        }

        private decimal calculateRemainingTimeProgress(PumpDto pump)
        {
            return ((decimal) pump.RemainingTime / pump.TotalTime);
        }

        private string formatRemainingTime(PumpDto pump)
        {
            // TODO: needs to be impl. according spec.
            return string.Format("{0} s", pump.RemainingTime);
        }
    }
}
