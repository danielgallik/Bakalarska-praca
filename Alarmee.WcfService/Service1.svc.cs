using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Text;
using Alarmee.StateContract;

namespace Alarmee.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IStateDataAccess
    {
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetAllPumps")]
        public List<PumpState> GetAllPumps()
        {
            List<PumpState> list = new List<PumpState>();

            PumpState pump = new PumpState()
            {
                CurrentState = "Running",
                Medicament = "Morfium",
                Progres = 90,
                RemainingTime = "2 min"
            };

            list.Add(pump);
            return list;
        }
    }
}
