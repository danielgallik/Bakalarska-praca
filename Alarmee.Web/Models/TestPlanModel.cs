using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alarmee.WardManager.Contracts;

namespace Alarmee.Web.Models
{
    public class TestPlanModel
    {
        public List<Item> Rooms;
        public List<Item> Beds;

        public List<string> PumpsAsString;
        public List<string> AlertsAsString;

        public TestPlanModel(WardStateInfo wardState)
        {
            Rooms = new List<Item>();
            Beds = new List<Item>();
            PumpsAsString = new List<string>();
            AlertsAsString = new List<string>();

            wardState.Rooms.ForEach(r => Rooms.Add(new Item() { Name = r.Label, Color = stateToColor(r.State), X1 = 10, Y1 = 10, X2 = 290, Y2 = 390 }));
            wardState.Beds.ForEach(b => Beds.Add(new Item() { Name = b.Label, Color = stateToColor(b.State), X1 = 60, Y1 = 110, X2 = 150, Y2 = 250 }));
            wardState.Pumps.ForEach(p => PumpsAsString.Add(string.Format("{0} - {1}% Remaining time: {2} Medicament: {3} State:{4}", p.Bed, p.Progress, p.RemainingTime, p.Medicament, p.State)));
            wardState.Alerts.ForEach(a => AlertsAsString.Add(string.Format("{0} - {1} State:{2}", a.Bed, a.Label, a.State))); 
        }

        public class Item
        {
            public string Name { get; set; }
            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X2 { get; set; }
            public int Y2 { get; set; }
            public string Color { get; set; }
        }

        private string stateToColor(string state)
        {
            switch (state)
            {
                case "Running":
                    return "green";
                case "PreAlarm":
                    return "orange";
                case "Alarm":
                    return "red";
                default:
                    return "gray";
            }
        }
    }
}