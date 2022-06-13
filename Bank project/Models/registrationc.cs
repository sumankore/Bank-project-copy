using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_project.Models
{
    public class registrationc
    {
        //{
        //    [Key]
        //    public string accountnumber { get; set; }
        //    public Registration Registration { get; set; }
        //    public logindetails logindetails { get; set; }
        //    public Transactions transactions { get; set; }
        //    public bankbal bankbal { get; set; }
        //    public roles roles { get; set; }
        //    public acctypes acctypes { get; set; }
        [Key]
        public int customerid  { get; set; }

        public string accountnumber { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime? DOB { get; set; }

        public string Gender { get; set; }

        public string acctype { get; set; }
        public string acctypename { get; set; }
        public string roleid { get; set; }
        public string role { get; set; }
        // public string accountnumber { get; set; }
        public string mobile { get; set; }
        public DateTime? AccountCreationDate { get; set; }

        public string aadhar { get; set; }

        public float bank_balance { get; set; }

        public string password { get; set; }

        public float Transactionamount { get; set; }

        public string tr_type { get; set; }

        public DateTime Transaction_Date { get; set; }

        public bool isActive { get; set; }
        public int TransactionID { get; set; }
       
        public float previous_balance { get; set; }
      

        // public  Registration bankbal { get; set; }
        public bankbal bankbal { get; set; }

    }
}