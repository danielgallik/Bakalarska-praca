using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alarmee.Contracts.Manager.Monitoring;

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

        public void SetPlan(WardStateInfo wardState)
        {
            Id = wardState.Id;
            Name = wardState.Name;
            SuccessLoad = true;
            ErrorMessage = "";

            foreach (WardStateInfo.Room room in wardState.Rooms)
            {
                Vertex namePosition = new Vertex(room.NamePosition.X, room.NamePosition.Y);
                List<Vertex> vertices = new List<Vertex>();
                room.Vertices.ForEach(v => vertices.Add(new Vertex(v.X, v.Y)));
                CanvasRoom canvasRoom = new CanvasRoom(room.Name, namePosition, vertices, room.State);
                Rooms.Add(canvasRoom);
            }
            foreach (WardStateInfo.Bed bed in wardState.Beds)
            {
                Vertex namePosition = new Vertex(bed.NamePosition.X, bed.NamePosition.Y);
                List<Vertex> vertices = new List<Vertex>();
                bed.Vertices.ForEach(v => vertices.Add(new Vertex(v.X, v.Y)));
                CanvasBed canvasBed = new CanvasBed(bed.Name, namePosition, vertices, bed.State);
                Beds.Add(canvasBed);
            }
            wardState.Pumps.ForEach(p => Pumps.Add(new PumpListItem(p.Bed, p.RemainingTime, p.Medicament, p.Type, p.State, p.Progress)));
            wardState.Alerts.ForEach(a => Alerts.Add(new AlertListItem(a.Bed, a.Message, a.Medicament, a.Type, a.State)));
        }

        public class CanvasRoom
        {
            public string Name { get; private set; }
            public Vertex NamePosition { get; private set; }
            public string NameColor { get; private set; }
            public List<Vertex> Vertices { get; private set; }
            public string Color { get; private set; }

            public CanvasRoom(string name, Vertex namePosition, List<Vertex> vertices, string state)
            {
                ColorConverter colorConverter = new ColorConverter();

                Name = name;
                NamePosition = namePosition;
                NameColor = colorConverter.RoomStateToNameColor(state);
                Vertices = vertices;
                Color = colorConverter.RoomStateToColor(state);
            }
        }

        public class CanvasBed
        {
            public string Name { get; private set; }
            public Vertex NamePosition { get; private set; }
            public List<Vertex> Vertices { get; private set; }
            public string Color { get; private set; }

            public CanvasBed(string name, Vertex namePosition, List<Vertex> vertices, string state)
            {
                ColorConverter colorConverter = new ColorConverter();

                Name = name;
                NamePosition = namePosition;
                Vertices = vertices;
                Color = colorConverter.BedStateToColor(state);
            }
        }

        public class PumpListItem
        {
            public string Bed { get; private set; }
            public string RemainingTime { get; private set; }
            public string Medicament { get; private set; }
            public string Type { get; private set; }
            public string ProgressColor { get; private set; }
            public double Progress { get; private set; }

            public PumpListItem(string bed, string remainingTime, string medicament, string type, string state, double progress)
            {
                ColorConverter colorConverter = new ColorConverter();
                ImageConverter imageConvert = new ImageConverter();

                Bed = bed;
                RemainingTime = remainingTime;
                Medicament = medicament;
                Type = imageConvert.TypeToImage(type);
                ProgressColor = colorConverter.BedStateToColor(state);
                Progress = progress;
            }
        }

        public class AlertListItem
        {
            public string Bed { get; private set; }
            public string Message { get; private set; }
            public string Medicament { get; private set; }
            public string Type { get; private set; }
            public string Color { get; private set; }

            public AlertListItem(string bed, string message, string medicament, string type, string state)
            {
                ColorConverter colorConverter = new ColorConverter();
                ImageConverter imageConvert = new ImageConverter();

                Bed = bed;
                Message = message;
                Medicament = medicament;
                Type = imageConvert.TypeToWhiteImage(type);
                Color = colorConverter.BedStateToColor(state);
            }
        }

        public class Vertex
        {
            public int X { get; private set; }
            public int Y { get; private set; }

            public Vertex(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}