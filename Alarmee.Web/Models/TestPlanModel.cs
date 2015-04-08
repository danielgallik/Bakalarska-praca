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

            wardState.Rooms.ForEach(r => Rooms.Add(new Item() 
            { 
                Name = r.Name, 
                NamePosition = new Point(){ X = r.NamePosition.X, Y = r.NamePosition.Y },
                Vertices = r.Vertices,
                Color = stateToColor(r.State)
            }));
            wardState.Beds.ForEach(b => Beds.Add(new Item() 
            { 
                Name = b.Name,
                NamePosition = b.NamePosition,
                Vertices = b.Vertices,
                Color = stateToColor(b.State)
            }));
            wardState.Pumps.ForEach(p => PumpsAsString.Add(string.Format("{0} - {1}% Remaining time: {2} Medicament: {3} State:{4}", p.Bed, (int)(p.Progress*100), p.RemainingTime, p.Medicament, p.State)));
            wardState.Alerts.ForEach(a => AlertsAsString.Add(string.Format("{0} - {1} State:{2}", a.Bed, a.Label, a.State))); 
        }

        public class Item
        {
            public string Name { get; set; }
            public Point NamePosition { get; set; }
            public List<Point> Vertices { get; set; }
            public string Color { get; set; }

            public Item()
            {
                Vertices = new List<Point>();
            }
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