using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Alarmee.WardManager.Contracts;
using Alarmee.Web.Models;
using WebGrease.Css.Extensions;

namespace Alarmee.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dictionary<string, string> wardPlan;

            try
            {
                var client = new WardManagerClient();
                wardPlan = client.GetWardPlans();
                client.Close();
            }
            catch
            {
                wardPlan = null;
            }
            
            return View(wardPlan);
        }

        public ActionResult Plan(string id = "")
        {
            return View(GetModel(id));
        }

        [HttpGet]
        public ActionResult GetPlan(string id = "")
        {
            return PartialView("PlanDetailPartialView", GetModel(id));
        }

        private PlanDetailModel GetModel(string id)
        {
            PlanDetailModel model = new PlanDetailModel();
            try
            {
                var client = new WardManagerClient();
                var wardState = client.GetWardState(id);
                client.Close();

                model.SetPlan(wardState);
            }
            catch
            {
                model.SetError(id, "Service is temporarily unavailable. <br /><br /> Please check your connection or contact your IT administrator.");
            }
            return model;
        }
    }
}