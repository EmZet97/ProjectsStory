using ProjectsStory.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectsStory.Controllers
{
    public class ProjectsController : Controller
    {
        DatabaseContext context = new DatabaseContext();

        // GET: Projects
        public ActionResult Index()
        {
            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Create()
        {
            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public ActionResult Create(ProjectModel model)
        {
            Project project = model.Project;
            ProjectUpdate update = model.Update;

            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            //Check if project name already exist
            int id = (int)Session["id"];
            Project p_check = context.Projects.Where( pp => pp.Title == (project.Title) && pp.UserId == (id) ).FirstOrDefault();

            if(p_check != null)
            {
                ViewBag.ErrorMessage = "Posiadasz już projekt o tej nazwie";
                return View(model);
            }
            
            
            // GET USER
            User user = context.Users.Where(u => u.UserId.Equals(id)).FirstOrDefault();
            if(user == null)
            {
                return View();
            }

            project.User = user;
            update.PublicationDate = DateTime.Now;
            update.Project = project;
           

            context.Projects.Add(project);
            context.ProjectUpdates.Add(update);
            context.SaveChanges();

            return RedirectToAction("ShowProjects", "Projects");
        }

        public ActionResult List()
        {
            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");
            int id = (int)Session["id"];
            User usr = context.Users.Where(u => u.UserId.Equals(id)).FirstOrDefault();
            List<Project> p = context.Projects.Where(pp => pp.UserId.Equals( id )).ToList();
            List<Project> projects = usr.Projects.ToList();
            return View(projects);
        }

        private bool WriteTextToFile(string filename, string text)
        {
            string path = Server.MapPath("~/" + filename + ".txt");
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                sw.WriteLine(text);
            }
            return true;
        }

        private string ReadTextFromFile(string filename)
        {
            string path = Server.MapPath("~/" + filename + ".txt");
            string text = System.IO.File.ReadAllText(path);
            
            return text;
        }

        private bool IsSessionOpen()
        {
            try
            {
                int id = (int)Session["id"];                
            }
            catch(NullReferenceException ex)
            {
                return false;
            }
            return true;

        }
    }
}