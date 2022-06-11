using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_project.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionID{get;set;}
        [Display(Name ="Transaction Amount")]
        [Range(1, 100000000)]
        public float Transactionamount{ get; set; }
        [Display(Name ="Account Number")]
        public string accountnumber{ get; set; }
        [Display(Name = "Account Type")]
        public string acctypename { get; set; }
        //public int tr_typeid { get; set; }
        [Display(Name ="Transaction Type")]
        public string tr_type { get; set; }
        //public int Deposit { get; set; }
        [Display(Name ="Bank Balance")]
        public float bank_balance { get; set; }
        public float previous_balance { get; set; }
        public DateTime Transaction_Date { get; set; }

       // public  Registration bankbal { get; set; }
        public bankbal bankbal { get; set; }


    }
}