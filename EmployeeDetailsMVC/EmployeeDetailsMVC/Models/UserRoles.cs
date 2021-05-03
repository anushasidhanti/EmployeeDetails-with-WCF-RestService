using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeDetailsMVC.Models
{
    public class UserRoles
    {
        
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Role { get; set; }

        public virtual Users UserTable { get; set; }
        
    }
}