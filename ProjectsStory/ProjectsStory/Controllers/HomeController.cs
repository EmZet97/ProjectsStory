using ProjectsStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsStory.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext context = new DatabaseContext();
        public ActionResult Index()
        {
            if (!IsSessionOpen())
            {
                ViewBag.ErrorMessage = "->" + Session["id"] + "<-" + !IsSessionOpen();
                //return View();
                return RedirectToAction("Login", "Account");
            }
            int id = (int)Session["id"];
            User user = context.Users.Where(u => u.UserId.Equals(id)).FirstOrDefault();
            var projects = user.Repository.Projects.ToList();
            //var updates = new List<ProjectUpdate>();
            //var eagerloading = test.zawodnicies.Include(x => x.druzyny);
            foreach (Project p in projects)
            {
                p.ProjectUpdates = p.ProjectUpdates;
                //foreach(ProjectUpdate u in updates)
                //{
                //    updates.Add(u);
                //}
            }
            //updates = updates.OrderBy(o => o.PublicationDate).ToList();
            return View(projects);
        }

        private bool IsSessionOpen()
        {
            return Session["id"] != null;
            
        }
    }
}