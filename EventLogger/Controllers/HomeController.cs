using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventLogger.Models;

namespace EventLogger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult EventTypes()
        {
            ViewBag.Title = "";
            using(var context = new PilotDBEntities())
            return View("EventTypes", context.EventTypes.ToList());
        }
    }
}
