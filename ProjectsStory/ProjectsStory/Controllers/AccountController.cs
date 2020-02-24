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
        UserContext context = new UserContext();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            user.Avatar = "Default.png";
            //Hashing password
            string pass = Crypto.SHA256(user.Password);
            user.Password = pass;

            if(context.Users.FirstOrDefault(v => v.Email.ToLower() == user.Email.ToLower()) == null)
            {
                context.Users.Add(user);

                try
                {
                    //User urr = context.Users.FirstOrDefault(v => v.Email.ToLower() == "mzawiski97@gmail.com");
                    //Hashing password
                    //string passs = Crypto.SHA256(urr.Password);
                    //urr.Password = passs;
                    context.SaveChanges();
                }
                catch(NullReferenceException ex)
                {
                    ViewBag.ErrorMessage = "Unknow Error. Please try again";
                    return View(user);
                }

                return RedirectToRoute("Login");
            }
            else
            {
                ViewBag.ErrorMessage = "Email already exists";
                return View(user);
            }
            
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                User usr = context.Users.Where(a => a.Email.Equals(user.Email)).FirstOrDefault();
                
                if (usr != null)
                {
                    bool passCorrect = Crypto.VerifyHashedPassword(usr.Password, user.Password);
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
                        return View(user);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Incorrect email";
                    return View(user);
                }
            }
            return View(user);

        }
    }
}