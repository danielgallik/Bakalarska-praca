
namespace Alarmee.Client.Web.Models
{
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
}