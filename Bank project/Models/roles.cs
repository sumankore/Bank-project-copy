using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_project.Models
{
    public class roles
    {
        [Key]
        [Display(Name = "Role ID")]
        public string roleid { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string rolename { get; set; }
        public bool isactive { get; set; }
    }
}