using System.Collections.Generic;

namespace Alarmee.Client.Web.Models
{
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
}