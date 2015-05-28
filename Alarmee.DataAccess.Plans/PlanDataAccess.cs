using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using Alarmee.Contracts.DataAccess.Plans;

namespace Alarmee.DataAccess.Plans
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class PlanDataAccess : IPlanDataAccess
    {
        public Dictionary<string, string> GetPlanList()
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load("Plans.xml");
                XmlNodeList nodePlanList = document.GetElementsByTagName("Plan");

                Dictionary<string, string> planList = new Dictionary<string, string>();
                foreach (XmlNode nodePlan in nodePlanList)
                {
                    planList[nodePlan.Attributes["Id"].Value] = nodePlan.Attributes["Name"].Value;
                }
                return planList;
            }
            catch
            {
                return null;
            }
        }

        public Plan GetPlan(string id)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load("Plans.xml");
                Plan plan = new Plan();
                plan.Id = id;
                XmlNode nodePlan = document.SelectSingleNode("/Plans/Plan[@Id='" + id + "']");
                if (nodePlan != null)
                {
                    plan.Name = nodePlan.Attributes["Name"].Value;

                    foreach (XmlNode nodePlanChild in nodePlan.ChildNodes)
                    {
                        if (nodePlanChild.Name == "Room")
                        {
                            plan.Rooms.Add(GetRoom(nodePlanChild));
                        }
                    }
                }
                return plan;
            }
            catch
            {
                return null;
            }
        }

        private Room GetRoom(XmlNode nodeRoom)
        {
            Room room = new Room();

            foreach (XmlNode nodeRoomChild in nodeRoom.ChildNodes)
            {
                switch (nodeRoomChild.Name)
                {
                    case "Name":
                        room.Name = nodeRoomChild.InnerText;
                        room.NamePosition = GetVertex(nodeRoomChild);
                        break;
                    case "Vertex":
                        room.Vertices.Add(GetVertex(nodeRoomChild));
                        break;
                    case "Bed":

                        room.Beds.Add(GetBed(nodeRoomChild));
                        break;
                }
            }
            return room;
        }

        private Bed GetBed(XmlNode nodeBed)
        {
            Bed bed = new Bed();
            foreach (XmlNode nodeBedChild in nodeBed.ChildNodes)
            {
                switch (nodeBedChild.Name)
                {
                    case "Name":
                        bed.Name = nodeBedChild.InnerText;
                        bed.NamePosition = GetVertex(nodeBedChild);
                        break;
                    case "IpAddress":
                        bed.IpAddresses.Add(nodeBedChild.InnerText);
                        break;
                    case "Vertex":
                        bed.Vertices.Add(GetVertex(nodeBedChild));
                        break;
                }
            }
            return bed;
        }

        private Vertex GetVertex(XmlNode nodePoint)
        {
            return new Vertex() 
            {
                X = Convert.ToInt32(nodePoint.Attributes["X"].Value),
                Y = Convert.ToInt32(nodePoint.Attributes["Y"].Value)
            };
        }
    }
}
