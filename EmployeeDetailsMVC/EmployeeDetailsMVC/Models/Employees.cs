using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EmployeeDetailsMVC.Models
{
    public partial class Employees
    {

        public int Id { get; set; }


        [Required]
        [StringLength(10, MinimumLength = 3)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Please enter upper and lower case alphabets only")]
        public string FullName { get; set; }


        [Required]
        public string Gender { get; set; }


        [Required]
        [Range(18,50)]
        public Nullable<int> Age { get; set; }


        [Required]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$",ErrorMessage ="Please enter a valid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }


        [Required]
        public Nullable<int> Salary { get; set; }


        [Required]
        [DataType(DataType.Url)]
        public string PersonalWebsite { get; set; }


        [Required]
        public string Photo { get; set; }



        [Required]
        public string AlternateText { get; set; }


    }
}