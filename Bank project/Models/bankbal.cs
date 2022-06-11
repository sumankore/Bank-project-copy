using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bank_project.Models
{
    public class bankbal
    {

        [Key]
         public int sno{ get; set; }

        public float bank_balance{ get; set; }
        public bool isactive{ get; set;}
         public string accountnumber { get; set; }     
       // public int accountid { get; set;}
        public int userid { get; set; }
        public Registration acccreatemdl { get; set; }

       
    }
}