using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alarmee.WardManager.Contracts;

namespace Alarmee.Web.Models
{
    public class PlanDetailModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<CanvasItem> Rooms;
        public List<CanvasItem> Beds;

        public List<PumpListItem> Pumps;
        public List<AlertListItem> Alerts;

        public PlanDetailModel(WardStateInfo wardState)
        {
            Rooms = new List<CanvasItem>();
            Beds = new List<CanvasItem>();
            Pumps = new List<PumpListItem>();
            Alerts = new List<AlertListItem>();

            ColorConverter colorConverter = new ColorConverter();
            ImageConverter imageConvert = new ImageConverter();

            wardState.Rooms.ForEach(r => Rooms.Add(new CanvasItem() 
            { 
                Name = r.Name, 
                NamePosition = new Point(){ X = r.NamePosition.X, Y = r.NamePosition.Y },
                Vertices = r.Vertices,
                Color = colorConverter.StateToRoomColor(r.State)
            }));
            wardState.Beds.ForEach(b => Beds.Add(new CanvasItem() 
            { 
                Name = b.Name,
                NamePosition = b.NamePosition,
                Vertices = b.Vertices,
                Color = colorConverter.StateToColor(b.State)
            }));
            wardState.Pumps.ForEach(p => Pumps.Add(new PumpListItem()
            {
                Bed = p.Bed,
                RemainingTime = p.RemainingTime,
                Medicament = p.Medicament,
                Type = imageConvert.TypeToImage(p.Type),
                Progress = p.Progress,
                ProgressColor = colorConverter.StateToColor(p.State)
            }));
            wardState.Alerts.ForEach(a => Alerts.Add(new AlertListItem()
            {
                Bed = a.Bed,
                Message = a.Message,
                Medicament = a.Medicament,
                Type = imageConvert.TypeToWhiteImage(a.Type),
                Color = colorConverter.StateToColor(a.State)
            })); 
        }

        public class CanvasItem
        {
            public string Name { get; set; }
            public Point NamePosition { get; set; }
            public List<Point> Vertices { get; set; }
            public string Color { get; set; }

            public CanvasItem()
            {
                Vertices = new List<Point>();
            }
        }

        public class PumpListItem
        {
            public string Bed { get; set; }
            public string RemainingTime { get; set; }
            public string Medicament { get; set; }
            public string Type { set; get; }
            public string ProgressColor { set; get; }
            public int Progress { set; get; }
        }

        public class AlertListItem
        {
            public string Bed { get; set; }
            public string Message { get; set; }
            public string Medicament { get; set; }
            public string Type { set; get; }
            public string Color { get; set; }
        }
    }
}