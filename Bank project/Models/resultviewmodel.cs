using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bank_project.Models
{
    public class resultviewmodel
    {
        [Key]
        public string id { get; set; }
        public IEnumerable<Registration> Registerview { get; set; }
        public IEnumerable<Transactions> Transactionsview { get; set; }
        //public List<Registration> Registerview { get; set; }
        //public List<Transactions> Transactionsview { get; set; }

    }
}