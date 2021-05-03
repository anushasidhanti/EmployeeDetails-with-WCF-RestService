using RestEmployeeDetails;
using Scrypt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.ModelBinding;

namespace RestEmployeeDetails
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class EmployeeService : IEmployeeDetails
    {
        UserDetailsEntities db = new UserDetailsEntities();
        public int AddEmployee(Employees employee)
        {
            try
            {
                db.tblEmployees.Add(employee);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new System.ServiceModel.Web.WebFaultException<string>(ex.ToString(), System.Net.HttpStatusCode.BadRequest);
            }
            return employee.Id;
        }


        public Employees EmployeeDetails(string id)
        {
            Int32 _id = Convert.ToInt32(id);
            return db.tblEmployees.SingleOrDefault(p => p.Id == _id);

        }

        //Get All Employees
        public List<Employees> GetEmployees()
        {
            return db.tblEmployees.ToList();
        }

        public bool EditEmployee(Employees employee)
        {
            Employees updateEmployee = db.tblEmployees.SingleOrDefault(p => p.Id == employee.Id);
            updateEmployee.FullName = employee.FullName;
            updateEmployee.Gender = employee.Gender;
            updateEmployee.EmailAddress = employee.EmailAddress;
            updateEmployee.Age = employee.Age;
            updateEmployee.AlternateText = employee.AlternateText;
            updateEmployee.PersonalWebsite = employee.PersonalWebsite;
            updateEmployee.Photo = employee.Photo;
            updateEmployee.Salary = employee.Salary;
            db.SaveChanges();
            return true;
        }

        public void DeleteEmployee(string id)
        {
            Int32 _id = Convert.ToInt32(id);
            Employees DeleteEmployee = db.tblEmployees.SingleOrDefault(p => p.Id == _id);
            db.tblEmployees.Remove(DeleteEmployee);
            db.SaveChanges();
        }

        public void DeleteMultipleEmployee(List<string> multipleEmployeeId)
        {
            foreach (string id in multipleEmployeeId)
            {
                Int32 _id = Convert.ToInt32(id);
                Employees DeleteEmployee = db.tblEmployees.SingleOrDefault(p => p.Id == _id);
                db.tblEmployees.Remove(DeleteEmployee);
                db.SaveChanges();
            }

        }

        public int AddUser(UserTable users)
        {
            try
            {
                users.UserRole = new List<UserRole>();
                UserRole roles = new UserRole();
                ScryptEncoder encoder = new ScryptEncoder();
                string encodedPassword = encoder.Encode(users.Password).ToString();
                users.Password = encodedPassword;
                roles.Role = "Employee";
                roles.UserId = users.Id;
                users.UserRole.Add(roles);

                foreach (UserRole role in users.UserRole)
                {
                    UserTable user = new UserTable();
                    role.UserId = users.Id;
                    user.UserName = users.UserName;
                    user.Password = users.Password;
                }
                db.UserTable.Add(users);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return users.Id;
        }

        public List<UserInfo> GetUsers()
        {

            List<UserInfo> usersInfo = new List<UserInfo>();
            List<UserTable> users = db.UserTable.ToList();

            foreach (UserTable user in users)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.Id = user.Id;
                userInfo.UserName = user.UserName;
                userInfo.Password = user.Password;

                List<UserRole> getRoles = db.UserRole.ToList();
                userInfo.UserRole = new List<Roles>();
                foreach (UserRole role in user.UserRole)
                {
                    Roles roles = new Roles();
                    roles.Role = role.Role;
                    userInfo.UserRole.Add(roles);
                }
                usersInfo.Add(userInfo);
            }
            return usersInfo;
        }


        public string[] GetRoles(string username)
        {
            string[] result = (from UserInfo in db.UserTable
                               join Roles in db.UserRole on UserInfo.Id equals Roles.UserId
                               where UserInfo.UserName == username
                               select Roles.Role).ToArray();
            return result;

        }

    }
}
