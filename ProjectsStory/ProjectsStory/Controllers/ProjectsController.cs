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
            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            int id = (int)Session["id"];

            Project project = model.Project;
            ProjectUpdate update = model.Update;
            User user = context.Users.Where(u => u.UserId == id).FirstOrDefault();
            if (user == null)
            {
                return View();
            }

            Repository repo = user.Repository ;
            //repo = context.Repositories.Where(r => r.Owner == user).FirstOrDefault();
            Project proj_check = repo.Projects.Where(p => p.Title == project.Title).FirstOrDefault();

            //Check if project name already exist
            //int id = (int)Session["id"];
            //Project p_check = context.Projects.Where( p => p.Title == (project.Title) && p.UserId == (id) ).FirstOrDefault();

            if(proj_check != null)
            {
                ViewBag.ErrorMessage = "Posiadasz już projekt o tej nazwie";
                return View(model);
            }
            
            

            project.Repository = user.Repository;
            update.PublicationDate = DateTime.Now;
            update.Project = project;

            context.Projects.Add(project);
            context.ProjectUpdates.Add(update);
            context.SaveChanges();

            return RedirectToAction("List", "Projects");
        }

        public ActionResult Update(int id)
        {
            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            Project project = context.Projects.Where(p => p.ProjectId.Equals(id) && p.Repository.OwnerId.Equals(Session["id"])).FirstOrDefault();
            if(project == null)
                return RedirectToAction("Incex", "Home");
            
            var update = new ProjectUpdate() { Project = project, ProjectId = id };
            return View(update);
        }

        [HttpPost]
        public ActionResult Update(ProjectUpdate update)
        {
            if(!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            if(!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Unknow Error";
                return View(update);
            }

            int id = (int)Session["id"];
            var project = context.Projects.Where(p => p.Repository.OwnerId.Equals(id)).FirstOrDefault();
            if(project == null)
            {
                ViewBag.ErrorMessage = "Session Error";
                return View(update);
            }

            update.Project = project;
            context.ProjectUpdates.Add(update);

            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                return View(update);
            }

            return RedirectToAction("Index", "Home");

        }

        public ActionResult List()
        {
            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            int id = (int)Session["id"];
            User usr = context.Users.Where(u => u.UserId.Equals(id)).FirstOrDefault();
            List<Project> projects = context.Projects.Where(pp => pp.Repository.Owner.UserId.Equals( id )).ToList();
            //List<Project> projects = usr.Projects.ToList();
            foreach(Project p in projects)
            {
                p.ProjectUpdates = context.ProjectUpdates.Where(v => v.ProjectId.Equals(p.ProjectId)).ToList();
            }
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