using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeDetailsMVC.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case alphabets only")]
        public string UserName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Password { get; set; }
        public ICollection<UserRoles> UserRole { get; set; }

        public string ErrorMessage { get; set; }
    }
}