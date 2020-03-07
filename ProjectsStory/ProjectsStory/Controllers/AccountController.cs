using ProjectsStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ProjectsStory.Controllers
{
    public class AccountController : Controller
    {
        DatabaseContext context = new DatabaseContext();
        // GET: Account
        public ActionResult Register()
        {
            //User u = new User() { Email = "admin@admin.admin", Name = "Admin", Surname = "Admin", Nick = "Admin" };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            User user = new User() { Email = model.Email, Name = model.Name, Nick = model.Nick, Surname = model.Surname };
            if (ModelState.IsValid)
            {
                user.Avatar = "Default.png";
                //Hashing password
                string pass = Crypto.HashPassword(model.Password);
                user.Password = pass;

                if (context.Users.FirstOrDefault(v => v.Email.ToLower() == user.Email.ToLower()) == null)
                {
                    Repository repository = new Repository() { IsPublic = true, Projects = new List<Project>(), ShareUrl = "repo_" + user.Nick, Owner = user };
                    user.Repository = repository;
                    repository.Owner = user;

                    context.Users.Add(user);
                    context.Repositories.Add(repository);

                    try
                    {
                        context.SaveChanges();
                    }
                    catch (NullReferenceException ex)
                    {
                        ViewBag.ErrorMessage = "Unknow Error. Please try again";
                        return View(model);
                    }

                    LoginModel loginModel = new LoginModel() { Email = model.Email };
                    return RedirectToAction("Login", "Account", loginModel);
                }
                else
                {
                    ViewBag.ErrorMessage = "Email already exists";
                    return View(model);
                }
            }
            return View(model);
            
        }

        public ActionResult Login()
        {
            LoginModel model = new LoginModel() { Email = "admin@gmail.com", Password = "qwerty" };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            User usr = context.Users.Where(a => a.Email.Equals(model.Email)).FirstOrDefault();
                
                if (usr != null)
                {
                    bool passCorrect = Crypto.VerifyHashedPassword(usr.Password, model.Password);
                    if (passCorrect)
                    {
                        Session.Clear();
                        Session["id"] = usr.UserId;
                        Session["email"] = usr.Email;
                        Session["avatar"] = usr.Avatar;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Incorrect password";
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Incorrect email";
                    return View(model);
                }
            

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        
    }
}