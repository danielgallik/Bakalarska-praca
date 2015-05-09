using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alarmee.Web.Models
{
    public class ImageConverter
    {
        public string TypeToImage(string type)
        {
            switch (type.ToLower())
            {
                case "infusion":
                    return "~/Content/Image/drop.png";
                case "injection":
                    return "~/Content/Image/injection.png";
                default:
                    return "~/Content/Image/dro.png";
            }
        }

        public string TypeToWhiteImage(string type)
        {
            switch (type.ToLower())
            {
                case "infusion":
                    return "~/Content/Image/drop-white.png";
                case "injection":
                    return "~/Content/Image/injection-white.png";
                default:
                    return "~/Content/Image/drop-whit.png";
            }
        }
    }
}