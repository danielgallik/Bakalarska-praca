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
                document.Load("D:/Dropbox/C#/Bakalarska praca/Alarmee.WardPlan/Plans.xml");
            }
            catch
            {
                document = null;
            }
        }

        public Plan getPlan(int id)
        {
            if (document != null)
            {
                Plan plan = null;
                XmlNode nodePlan = document.SelectSingleNode("/Plans/Plan[@Id='" + id.ToString() + "']");
                if (nodePlan != null)
                {
                    plan = new Plan();
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
                return plan;
            }
            else
            {
                return null;
            }
        }
    }
}
