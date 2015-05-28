using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alarmee.Client.Web.Models
{
    public class ImageConverter
    {
        public string TypeToImage(string type)
        {
            switch (type.ToLower())
            {
                case "volumetric":
                    return "~/Content/Image/drop.png";
                case "syringe":
                    return "~/Content/Image/injection.png";
                default:
                    return "~/Content/Image/dro.png";
            }
        }

        public string TypeToWhiteImage(string type)
        {
            switch (type.ToLower())
            {
                case "volumetric":
                    return "~/Content/Image/drop-white.png";
                case "syringe":
                    return "~/Content/Image/injection-white.png";
                default:
                    return "~/Content/Image/drop-whit.png";
            }
        }
    }
}