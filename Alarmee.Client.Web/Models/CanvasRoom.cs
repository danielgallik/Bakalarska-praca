using System.Collections.Generic;

namespace Alarmee.Client.Web.Models
{
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
}