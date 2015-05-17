using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alarmee.Web.Models
{
    public class ColorConverter
    {
        private const string stateNotConfigured = "NotConfigured";
        private const string stateInactive = "Inactive";
        private const string stateRunning = "Running";
        private const string stateOk = "Ok";
        private const string statePreAlarm = "PreAlarm";
        private const string stateAlarm = "Alarm";

        public string BedStateToColor(string state)
        {
            switch (state)
            {
                case stateNotConfigured:
                    return "rgb(132, 132, 132)";
                case stateInactive:
                    return "rgb(155, 89, 182)";
                case stateRunning:
                    return "rgb(41, 188, 151)";
                case statePreAlarm:
                    return "rgb(247, 105, 14)";
                case stateAlarm:
                    return "rgb(196, 0, 6)";
                default:
                    return "rgb(132, 132, 132)";
            }
        }

        public string RoomStateToNameColor(string state)
        {
            switch (state)
            {
                case stateNotConfigured:
                    return "white";
                case stateOk:
                    return "rgb(132, 132, 132)";
                case statePreAlarm:
                    return "rgb(247, 105, 14)";
                case stateAlarm:
                    return "rgb(196, 0, 6)";
                default:
                    return "rgb(132, 132, 132)";
            }
        }

        public string RoomStateToColor(string state)
        {
            switch (state)
            {
                case stateNotConfigured:
                    return "rgb(132, 132, 132)";
                default:
                    return "white";
            }
        }
    }
}