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
        public ActionResult Create(ProjectModel model, HttpPostedFileBase file)
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

            Project proj_check = repo.Projects.Where(p => p.Title == project.Title).FirstOrDefault();
            if(proj_check != null)
            {
                ViewBag.ErrorMessage = "Project name already exists in your repository";
                return View(model);
            }

            project.Repository = user.Repository;
            if (update == null)
                update = new ProjectUpdate();
            update.PublicationDate = DateTime.Now;
            update.Project = project;

            if (file != null)
            {
                int updates = 1;
                string fileName = "update" + updates + "_" + file.FileName;
                //Check if path for file exists
                string dirPath = "~/Content/Images/Projects/user" + user.UserId + "/project_" + project.Title;

                bool exists = Directory.Exists(Server.MapPath(dirPath));
                if (!exists)
                    Directory.CreateDirectory(Server.MapPath(dirPath));

                string path = Path.Combine(Server.MapPath(dirPath), fileName);

                // file is uploaded
                file.SaveAs(path);
                dirPath = "/Content/Images/Projects/user" + user.UserId + "/project_" + project.Title;

                update.ImageUrl = dirPath + "/" + fileName;
            }
            

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

        public ActionResult Delete(int id)
        {
            if (!IsSessionOpen())
                return RedirectToAction("Login", "Account");

            int userid = (int)Session["id"];

            User user = context.Users.Where(u => u.UserId.Equals(userid)).FirstOrDefault();
            if(user == null)
                return RedirectToAction("Login", "Account");

            Project project = user.Repository.Projects.Where(p => p.ProjectId.Equals(id)).FirstOrDefault();

            if (user == null)
                return RedirectToAction("Index", "Home");

            string dirPath = "~/Content/Images/Projects/user" + user.UserId + "/project_" + project.Title;
            DeleteDirectory(dirPath);

            context.ProjectUpdates.RemoveRange(context.ProjectUpdates.Where(u => u.ProjectId.Equals(project.ProjectId)));
            

            context.Entry(project).State = System.Data.Entity.EntityState.Deleted;

            context.SaveChanges();

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
            return Session["id"] != null;

        }



        private void DeleteDirectory(string folderPath)
        {
            string path = Server.MapPath(folderPath);

            try
            {

                string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

                //Delete files
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                }
                //then delete folder
                Directory.Delete(path);
            }
            catch(Exception e)
            {
                return;
            }

        }


    }
}