
namespace Alarmee.Client.Web.Models
{
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