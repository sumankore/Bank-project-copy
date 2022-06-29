using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;

namespace Bank_project.Models
{
    public class resultviewmodel
    {
        [Key]
        public string id { get; set; }
        public Registration Registerviews { get; set; }

        public Transactions Transactionsviews { get; set; }
        public IEnumerable<Registration> Registerview { get; set; }
        public IEnumerable<Transactions> Transactionsview { get; set; }
        public List<Registration> Registerviewlist { get; set; }
        public List<Transactions> Transactionsviewlist { get; set; }
        public IPagedList<Registration> Registerviewpl { get; set; }
        public IPagedList<Transactions> Transactionsviewpl { get; set; }
        public List<acctypes> acctyplist { get; set; }
        public List<roles> roletyplist { get; set; }
        //public List<Registration> Registerview { get; set; }
        //public List<Transactions> Transactionsview { get; set; }

    }
}