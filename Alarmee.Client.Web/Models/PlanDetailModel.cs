using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Manager = Alarmee.Contracts.Manager.Monitoring;

namespace Alarmee.Client.Web.Models
{
    public class PlanDetailModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool SuccessLoad { get; set; }
        public string ErrorMessage { get; set; }

        public List<CanvasRoom> Rooms;
        public List<CanvasBed> Beds;

        public List<PumpListItem> Pumps;
        public List<AlertListItem> Alerts;

        public PlanDetailModel()
        {
            Rooms = new List<CanvasRoom>();
            Beds = new List<CanvasBed>();
            Pumps = new List<PumpListItem>();
            Alerts = new List<AlertListItem>();
        }

        public void SetError(string id, string message)
        {
            Id = id;
            Name = "NaN";
            SuccessLoad = false;
            ErrorMessage = message;
        }

        public void SetPlan(Manager.MonitoringInfo monitoringInfo)
        {
            Id = monitoringInfo.Id;
            Name = monitoringInfo.Name;
            SuccessLoad = true;
            ErrorMessage = "";

            foreach (Manager.Room room in monitoringInfo.Rooms)
            {
                Vertex namePosition = new Vertex(room.NamePosition.X, room.NamePosition.Y);
                List<Vertex> vertices = new List<Vertex>();
                room.Vertices.ForEach(v => vertices.Add(new Vertex(v.X, v.Y)));
                CanvasRoom canvasRoom = new CanvasRoom(room.Name, namePosition, vertices, room.State);
                Rooms.Add(canvasRoom);
            }
            foreach (Manager.Bed bed in monitoringInfo.Beds)
            {
                Vertex namePosition = new Vertex(bed.NamePosition.X, bed.NamePosition.Y);
                List<Vertex> vertices = new List<Vertex>();
                bed.Vertices.ForEach(v => vertices.Add(new Vertex(v.X, v.Y)));
                CanvasBed canvasBed = new CanvasBed(bed.Name, namePosition, vertices, bed.State);
                Beds.Add(canvasBed);
            }
            monitoringInfo.Pumps.ForEach(p => Pumps.Add(new PumpListItem(p.Bed, p.RemainingTime, p.Medicament, p.Type, p.State, p.Progress)));
            monitoringInfo.Alerts.ForEach(a => Alerts.Add(new AlertListItem(a.Bed, a.Message, a.Medicament, a.Type, a.State)));
        }
    }
}