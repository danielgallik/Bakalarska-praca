
namespace Alarmee.Client.Web.Models
{
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
}