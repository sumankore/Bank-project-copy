using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bank_project.Models
{
    public class acctypes
    {
      
       
        [Key]
        [Required]
        [Display(Name = "Account Type ID")]
        public string acc_type_id { get; set; }
        [Required]
        [Display(Name ="Account Type")]
        public string account_type { get; set; }
        [Required]
        [Display(Name = "Minimnum Limit")]
        public float min_limit{ get; set; }
        [Required]
        public bool isactive { get; set; }

       
      //  public int accountid { get; set; }
        //public Registration acccreatemdl { get; set; }
       

    }
}