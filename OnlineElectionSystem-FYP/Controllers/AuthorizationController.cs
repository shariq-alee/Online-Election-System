using OnlineElectionSystem_FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineElectionSystem_FYP.Controllers
{
    public class AuthorizationController : Controller
    {


        public static int userId;
        public static string userType;
        ElectionCommissionDatabaseEntities db;

        //
        // GET: /Login/
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View(); 
        }

        [HttpPost]
        public ActionResult AdminLogin(Models.Admin Userr)
        {
            var fetchedUser = IsValid(Userr.CNIC, Userr.Password);
            if (fetchedUser != null)
            {
                //HomeController.userId = fetchedUser.Id;
                //TempData["User"] = fetchedUser;
                if (fetchedUser.Type == "Admin")
                {
                    AdminController.user = fetchedUser;
                    HomeController.user = fetchedUser;
                    return RedirectToRoute(new { controller = "Home", action = "Dashboard"});
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This user is not an admin.");
                    return View();
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid CNIC or Password.");
            return View();
        }

        [HttpGet]
        public ActionResult CandidateLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CandidateLogin(Models.Citizen Userr)
        {

            var fetchedUser = IsValid(Userr.CNIC, Userr.Password);
            if (fetchedUser != null)
            {
                //HomeController.userId = fetchedUser.Id;
                //TempData["User"] = fetchedUser;
                if (fetchedUser.Type == "Candidate")
                {
                    HomeController.user = fetchedUser;
                    return RedirectToRoute(new { controller = "Home", action = "Dashboard" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This user is not a Candidate.");
                    return View();
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid CNIC or Password.");
            return View();
        }
        
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.Citizen Userr)
        {
            var fetchedUser = IsValid(Userr.CNIC, Userr.Password);
            if (fetchedUser != null)
            {
                //HomeController.userId = fetchedUser.Id;
                //TempData["User"] = fetchedUser;
                if (fetchedUser.Type == "Voter"){
                     HomeController.user = fetchedUser;
                     return Redirect("Home/Dashboard");
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "Only Voters can Login through this form.");
                    return View();
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid CNIC or Password.");
            return View();
        }


        private Citizen IsValid(string CNIC, string password)
        {
            db = new ElectionCommissionDatabaseEntities();
            var user = db.Citizens.FirstOrDefault(u => u.CNIC.Equals(CNIC));
            if (user != null)
            {
                if (user.Password == password)
                {
                    userId = user.Id;
                    userType = user.Type;
                    user.lastLogin = System.DateTime.Now.ToString();
                    db.SaveChanges();
                    return user;
                }
            }
            db.Database.Connection.Dispose();
            return null;
        }

        public ActionResult Logout()
        {
            HomeController.user = null;
            AdminController.user = null;
            //TempData["User"] = null;
            Session.Abandon();
            Request.Cookies.Clear();
            return Redirect("Login");
        }
    }
}
