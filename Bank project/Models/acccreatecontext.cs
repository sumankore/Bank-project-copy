using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Data.Entity;

namespace Bank_project.Models
{
    public class acccreatecontext : DbContext

        {


        public  acccreatecontext() :base ("bankconstr")
        {

        }

        public DbSet<Registration>registration { get; set; }
        public DbSet<acctypes> actypcrt { get; set; }
        public DbSet<bankbal> bankbal { get; set; }
        public DbSet<Transactions> Transaction { get; set; }
        public DbSet<roles> role { get; set; }
        public DbSet<logindetails> logindetails { get; set; }
        public DbSet<registrationc> registrationc { get; set; }
        //public Registration Reg { get; set; }
        //public registrationc Regc { get; set; }

    }
}