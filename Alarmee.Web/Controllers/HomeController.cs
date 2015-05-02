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
        private PlanDetailModel GetModel(string id)
        {
            var client = new WardManagerClient();
            var wardState = client.GetWardState(id);
            client.Close();
            PlanDetailModel model = new PlanDetailModel(wardState);
            model.Id = id;
            return model;
        }

        public ActionResult Index()
        {
            var client = new WardManagerClient();
            Dictionary<string, string> wardPlan = client.getWardPlan();
            client.Close();
            return View(wardPlan);
        }

        public ActionResult Plan(string id = "")
        {
            return View(GetModel(id));
        }

        [HttpPost]
        public ActionResult Update(string id = "")
        {
            return PartialView("PlanDetailPartialView", GetModel(id));
        }
	}
}