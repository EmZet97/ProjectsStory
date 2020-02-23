using ProjectsStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsStory.Controllers
{
    public class UserController : Controller
    {
        UserContext context = new UserContext();
        // GET: User
        public ActionResult Index()
        {
            return View(context.Users.ToList());
        }
    }
}