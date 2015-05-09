using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using Alarmee.WardPlan.Contract;

namespace Alarmee.WardPlan
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class PlanDataAccess : IPlanDataAccess
    {
        XmlDocument document = new XmlDocument();

        public PlanDataAccess()
        {
            try
            {
                document.Load("Plans.xml");
            }
            catch
            {
                document = null;
            }
        }

        public Dictionary<string, string> GetPlanList()
        {
            Dictionary<string, string> planList = new Dictionary<string, string>();
            if (document != null)
            {
                XmlNodeList nodePlanList = document.GetElementsByTagName("Plan");
                foreach (XmlNode nodePlan in nodePlanList)
                {
                    planList[nodePlan.Attributes["Id"].Value] = nodePlan.Attributes["Name"].Value;
                }
            }
            return planList;
        }

        public Plan GetPlan(string id)
        {
            Plan plan = new Plan();
            if (document != null)
            {
                XmlNode nodePlan = document.SelectSingleNode("/Plans/Plan[@Id='" + id + "']");
                if (nodePlan != null)
                {
                    plan.Name = nodePlan.Attributes["Name"].Value;

                    foreach(XmlNode nodePlanChild in nodePlan.ChildNodes)
                    {
                        if (nodePlanChild.Name == "Room")
                        {
                            Plan.Room room = new Plan.Room();

                            foreach (XmlNode nodeRoom in nodePlanChild.ChildNodes)
                            {
                                switch (nodeRoom.Name)
                                {
                                    case "Name":
                                        room.Name = nodeRoom.InnerText;
                                        room.NamePosition = new Plan.Point()
                                        {
                                            X = Convert.ToInt32(nodeRoom.Attributes["X"].Value),
                                            Y = Convert.ToInt32(nodeRoom.Attributes["Y"].Value)
                                        };
                                        break;
                                    case "Vertex":
                                        room.Vertices.Add(new Plan.Point()
                                        {
                                            X = Convert.ToInt32(nodeRoom.Attributes["X"].Value),
                                            Y = Convert.ToInt32(nodeRoom.Attributes["Y"].Value)
                                        });
                                        break;
                                    case "Bed":
                                        Plan.Bed bed = new Plan.Bed();
                                        foreach (XmlNode nodeBed in nodeRoom.ChildNodes)
                                        {
                                            switch (nodeBed.Name)
                                            {
                                                case "Name":
                                                    bed.Name = nodeBed.InnerText;
                                                    bed.NamePosition = new Plan.Point()
                                                    {
                                                        X = Convert.ToInt32(nodeBed.Attributes["X"].Value),
                                                        Y = Convert.ToInt32(nodeBed.Attributes["Y"].Value)
                                                    };
                                                    break;
                                                case "IpAddress":
                                                    bed.IpAddresses.Add(nodeBed.InnerText);
                                                    break;
                                                case "Vertex":
                                                    bed.Vertices.Add(new Plan.Point()
                                                    {
                                                        X = Convert.ToInt32(nodeBed.Attributes["X"].Value),
                                                        Y = Convert.ToInt32(nodeBed.Attributes["Y"].Value)
                                                    });
                                                    break;
                                            }
                                        }
                                        room.Beds.Add(bed);
                                        break;
                                }
                            }
                            plan.Rooms.Add(room);
                        }
                    }
                }
            }
            return plan;
        }
    }
}
