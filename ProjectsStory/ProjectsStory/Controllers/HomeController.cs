using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsStory.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!IsSessionOpen())
            {
                ViewBag.ErrorMessage = "->" + Session["id"] + "<-" + !IsSessionOpen();
                //return View();
                return RedirectToAction("Login", "Account");
            }
            else
                return View();
        }

        private bool IsSessionOpen()
        {
            return Session["id"] != null;
            
        }
    }
}