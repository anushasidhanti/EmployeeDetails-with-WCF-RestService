using EmployeeDetailsMVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EmployeeDetailsMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        WebClient proxy = new WebClient();
        public ActionResult Index(string searchBy, string search, int? page, string sortBy)
        {
            List<Employees> employee = new List<Employees>();

            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/GetEmployees");
            byte[] _data = proxy.DownloadData(serviceUrl);
            Stream _stream = new MemoryStream(_data);
            StreamReader reader = new StreamReader(_stream);
            string result = reader.ReadToEnd();
            employee = JsonConvert.DeserializeObject<List<Employees>>(result);
            ViewBag.SortFullNameParameter = string.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SortGenderParameter = sortBy == "Gender" ? "Gender desc" : "Gender";
            var employees = employee.AsQueryable();
            if (searchBy == "Gender")
            {
                employees = employees.Where(x => x.Gender.ToLower().StartsWith(search.ToLower()) || search == null);
            }
            else if (searchBy == "FullName")
            {
                employees = employees.Where(x => x.FullName.ToLower().StartsWith(search.ToLower()) || search == null);
            }
            else
            {
                switch (sortBy)
                {
                    case "Name desc":
                        employees = employees.OrderByDescending(x => x.FullName);
                        break;
                    case "Gender desc":
                        employees = employees.OrderByDescending(x => x.Gender);
                        break;
                    case "Gender":
                        employees = employees.OrderBy(x => x.Gender);
                        break;
                    default:
                        employees = employees.OrderBy(x => x.FullName);
                        break;

                }

            }
            return View(employees.ToPagedList(page ?? 1, 3));
        }



        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create(Employees employee, HttpPostedFileBase UploadedImage)
        {
            string folderPath;
            if (UploadedImage != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(UploadedImage.FileName);
                string extension = Path.GetExtension(UploadedImage.FileName);
                fileName = fileName + DateTime.Now.ToString("Employee") + extension;
                employee.Photo = "~/Photos/" + fileName;
                string json = new JavaScriptSerializer().Serialize(employee);
                fileName = System.IO.Path.Combine(Server.MapPath("~/Photos/"), fileName);
                string pic = System.IO.Path.GetFileName(UploadedImage.FileName);
                folderPath = System.IO.Path.Combine("~/Photos/", pic);
                UploadedImage.SaveAs(fileName);
                proxy.Headers["Content-Type"] = "application/json";
                proxy.UploadString("http://localhost:61866/EmployeeDetails.svc/CreateEmployee", "POST", json);
            }
            else
            {
                ViewBag.Message = "Please Upload a Image of jpg or png format";
                return View();
            }
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Admin,Manager,Employee")]
        public ActionResult Details(int id)
        {

            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/EmployeeDetails/{0}", id);
            byte[] _data = proxy.DownloadData(serviceUrl);
            Stream _stream = new MemoryStream(_data);
            System.IO.StreamReader reader = new StreamReader(_stream);
            string result = reader.ReadToEnd();
            object model = JsonConvert.DeserializeObject<Employees>(result);
            return View(model);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/EmployeeDetails/{0}", id);
            byte[] _data = proxy.DownloadData(serviceUrl);
            Stream _stream = new MemoryStream(_data);
            System.IO.StreamReader reader = new StreamReader(_stream);
            string result = reader.ReadToEnd();
            object model = JsonConvert.DeserializeObject<Employees>(result);
            return View(model);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "FullName,EmailAddress,PersonalWebsite")] Employees employee, int id)
        {
            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/EmployeeDetails/{0}", id);
            byte[] _data = proxy.DownloadData(serviceUrl);
            Stream _stream = new MemoryStream(_data);
            System.IO.StreamReader reader = new StreamReader(_stream);
            string result = reader.ReadToEnd();
            object model = JsonConvert.DeserializeObject<Employees>(result);
            employee.FullName = ((EmployeeDetailsMVC.Models.Employees)model).FullName;
            employee.EmailAddress = ((EmployeeDetailsMVC.Models.Employees)model).EmailAddress;
            employee.PersonalWebsite = ((EmployeeDetailsMVC.Models.Employees)model).PersonalWebsite;
            string json = new JavaScriptSerializer().Serialize(employee);
            proxy.Headers["Content-Type"] = "application/json";
            proxy.UploadString("http://localhost:61866/EmployeeDetails.svc/EditEmployee", "PUT", json);
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/DeleteEmployee/{0}", id);
            string method = "DELETE";
            proxy.UploadString(serviceUrl, method, id);
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteMultipleRows(IEnumerable<int> employeeIdsToDelete)
        {
            byte[] employeeIds = employeeIdsToDelete.SelectMany(BitConverter.GetBytes).ToArray();
            string[] the_array = employeeIdsToDelete.Select(i => i.ToString()).ToArray();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string json = new JavaScriptSerializer().Serialize(employeeIdsToDelete);
            proxy.Headers["Content-Type"] = "application/json";
            //string json = jss.Serialize(employeeIdsToDelete);
            string serviceUrl = string.Format("http://localhost:61866/EmployeeDetails.svc/DeleteMultipleEmployee");
            string method = "DELETE";
            proxy.UploadString(serviceUrl, method, json);
            return RedirectToAction("Index");

        }

    }
}