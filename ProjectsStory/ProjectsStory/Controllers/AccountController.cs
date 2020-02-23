using ProjectsStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActionResult Register(User user)
        {
            user.Avatar = "Default.png";            

            if(context.Users.FirstOrDefault(v => v.Email.ToLower() == user.Email.ToLower()) == null)
            {
                context.Users.Add(user);

                try
                {
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
    }
}