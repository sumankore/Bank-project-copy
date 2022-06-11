using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_project.Models
{
    public class logindetails
    {
        [Key]
        public int loginid { get; set; }
        [Required(ErrorMessage = "email is required")]

        [EmailAddress(ErrorMessage = "Enter valid Email address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Type Password")]
        public string password { get; set; }
        [Required(ErrorMessage ="Select Role")]
        public string role { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int LoginFailedCount { get; set; }
        public bool AccountLocked { get; set; }
        public bool isactive { get; set; }
        public int userid { get; set; }

       
        public Registration registration { get; set; }
    }
}