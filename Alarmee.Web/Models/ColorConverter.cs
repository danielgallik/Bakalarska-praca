using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alarmee.Web.Models
{
    public class ColorConverter
    {
        public string StateToColor(string state)
        {
            switch (state)
            {
                case "Running":
                    return "rgb(0, 155, 107)";
                case "PreAlarm":
                    return "rgb(238, 116, 0)";
                case "Alarm":
                    return "rgb(200, 0, 0)";
                default:
                    return "gray";
            }
        }

        public string StateToRoomColor(string state)
        {
            switch (state)
            {
                case "PreAlarm":
                    return "rgb(238, 116, 0)";
                case "Alarm":
                    return "rgb(200, 0, 0)";
                default:
                    return "gray";
            }
        }
    }
}