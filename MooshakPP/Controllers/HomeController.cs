using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MooshakPP.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //krissi er IT master
            return View();
        }

        public ActionResult About()
        {
            //fuck off
            //heyoheyjooeoeoeoe
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