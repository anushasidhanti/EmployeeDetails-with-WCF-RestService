using EmployeeDetailsMVC.Models;
using Newtonsoft.Json;
using Scrypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;


namespace EmployeeDetailsMVC.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        WebClient proxy = new WebClient();
        List<Users> users = new List<Users>();

        // GET: Authentication
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Users model)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/GetUsers");
            byte[] _data = proxy.DownloadData(serviceUrl);
            Stream _stream = new MemoryStream(_data);
            StreamReader reader = new StreamReader(_stream);
            string result = reader.ReadToEnd();
            users = JsonConvert.DeserializeObject<List<Users>>(result);
            var Users = users.AsQueryable();
            var ValidUser = (from c in users where c.UserName.Equals(model.UserName) select c).SingleOrDefault();

            if (ValidUser == null)
            {
                ModelState.AddModelError("", "invalid Username or Password");
                return View();
            }
            bool isValidUser = encoder.Compare(model.Password, ValidUser.Password);
            if (isValidUser)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Username or password is invalid";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Signup()
        {
            return View();
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Signup(Users userData)
        {
            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/GetUsers");
            byte[] _data = proxy.DownloadData(serviceUrl);
            Stream _stream = new MemoryStream(_data);
            StreamReader reader = new StreamReader(_stream);
            string result = reader.ReadToEnd();
            users = JsonConvert.DeserializeObject<List<Users>>(result);
            var Users = users.AsQueryable();
            bool IsRegisterd = Users.Any(user => user.UserName.ToLower() ==
                    userData.UserName.ToLower());
            if (IsRegisterd)
            {
                FormsAuthentication.SetAuthCookie(userData.UserName, false);
                ModelState.AddModelError("UserName", "Username already exists");
                return View();
            }
            else
            {
                string json = new JavaScriptSerializer().Serialize(userData);
                proxy.Headers["Content-Type"] = "application/json";
                proxy.UploadString("http://localhost:61866/EmployeeDetails.svc/AddUser", "POST", json);
                ViewBag.Error = "Registered  successfully.Please Login";
            }

            return RedirectToAction("Login");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}