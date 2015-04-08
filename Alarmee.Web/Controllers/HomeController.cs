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
        private TestPlanModel GetModel()
        {
            var client = new WardManagerClient();
            var wardState = client.GetWardState();
            client.Close();

            return new TestPlanModel(wardState);
        }

		public ActionResult Index()
		{

            return View(GetModel());
		}

        [HttpGet]
        public ActionResult Update()
        {
            return PartialView("TestPlanPartialView", GetModel());
        }

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}