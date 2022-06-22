﻿using Bank_project.Models;
using Bank_project.security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Bank_project.Controllers
{
    //[Authorize]
    [Customauthenticationfilter]
    [CustomAuthorize("Employee")]
    public class OfficerController : Controller
    {


        acccreatecontext businesobj = new acccreatecontext();
        //acctypmdl acctypobj = new acctypmdl();
        // GET: business

        public ActionResult Index()
        {

            ViewBag.Email = Session["Email"];
            ViewBag.Userid = Session["userid"];
            return View();
        }


        //[ChildActionOnly]
        //public ActionResult acclist()
        //{
        //    IEnumerable<Registration> acclist;

        //    using (acccreatecontext context = new acccreatecontext())
        //    {
        //        acclist = context.registration.Where(x => x.role == "Customer").ToList();
        //    }
        //    return PartialView("_LoginPartial", acclist);
        //}
        //public ActionResult Accountlist()
        //{

        //    //    var accounts = businesobj.registration.Where(x => x.isActive == true);
        //    //var accounts = businesobj.registration.Where(x => x.role == "Customer").FirstOrDefault();
        //    var accounts = businesobj.registration.Where(x => x.role == "Customer");


        //    return View(accounts);
        //}
        public ActionResult Custommeraccs(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {
            ViewBag.CurrentSortOrder = Sorting_Order;
            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name_Description" : "";
            ViewBag.SortingDate = Sorting_Order == "Date_Enroll" ? "Date_Description" : "Date";

            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;
            resultviewmodel customers = new resultviewmodel();
            var accounts = Getaccounts().ToList();
            int Size_Of_Page = 2;
           int No_Of_Page = (Page_No ?? 1);
            customers.Registerviewpl = accounts.Where(x => x.role == "Customer" & x.isActive == true).ToPagedList(No_Of_Page, Size_Of_Page);
            //var customers = from accs in businesobj.registration.Where(x => x.role == "Customer" && x.isActive == true) select accs;
            //if (!String.IsNullOrEmpty(Search_Data))

            //{
            //    //customers.Registerviewpl = customers.Registerviewpl.Where(x => x.Name.ToUpper().Contains(Search_Data.ToUpper()).ToPagedList(No_Of_Page, Size_Of_Page));
            //       /* || accs.LastName.ToUpper().Contains(Search_Data.ToUpper())*/);
            //}
           

            switch (Sorting_Order)
            {
                case "Name_Description":
                    customers.Registerviewpl = customers.Registerviewpl.OrderByDescending(x => x.Name).ToPagedList(No_Of_Page, Size_Of_Page);
                    break;
                case "Date":
                    customers.Registerviewpl = customers.Registerviewpl.OrderByDescending(x => x.DOB).ToPagedList(No_Of_Page, Size_Of_Page);
                    break;

                case "Date_Enroll":

                    customers.Registerviewpl = customers.Registerviewpl.OrderBy(x => x.AccountCreationDate).ToPagedList(No_Of_Page, Size_Of_Page);

                    break;
                default:
                    customers.Registerviewpl = customers.Registerviewpl.OrderBy(x => x.accountnumber).ToPagedList(No_Of_Page, Size_Of_Page);
                    break;
            }
            //int Size_Of_Page = 4;
            //int No_Of_Page = (Page_No ?? 1);
           
            customers.Registerviewpl = customers.Registerviewpl.ToPagedList(No_Of_Page, Size_Of_Page);
            return View(customers);
           
            //return View(customers/*.ToList()*/);
        }

        public ActionResult accreate()
        {

            var acctyplist = businesobj.actypcrt.Where(x => x.isactive == true);
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type");
            return View();
        }
        
        [HttpPost]
        public ActionResult accreate(registrationc reg)
        {
            bool flag = false;

            //var rolelist = businesobj.role.Where(x => x.isactive == true & x.rolename == "Customer");
            //ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename");
            //var rolenames = businesobj.role.Where(c => c.roleid == reg.roleid);
            //foreach (var rolename in rolenames)
            //    reg.role = rolename.rolename;
            var acctyplist = businesobj.actypcrt.Where(x => x.isactive == true);
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type");
            var name = businesobj.actypcrt.Where(c => c.acc_type_id == reg.acctype);
            foreach (var acctypname in name)
                reg.acctypename = acctypname.account_type;
            var useridds = 0;
            string passwordd = password();
            var empl = businesobj.registration.Where(x => x.accountnumber == reg.accountnumber || x.Email == reg.Email || x.aadhar == reg.aadhar || x.mobile == reg.mobile).ToList();
            if (empl != null)
            {
                for (int i = 0; i < empl.Count; i++)
                {
                    if (flag == false)
                    {
                        if (empl[i].Email == reg.Email)
                        {
                            ViewBag.emailexist = "Email already exists";
                            flag = false;
                            break;
                        }
                        else if (empl[i].accountnumber == reg.accountnumber)
                        {
                            ViewBag.accexist = "Account ID already exists";
                            flag = false;
                            break;
                        }
                        else if (empl[i].aadhar == reg.aadhar)

                        {
                            ViewBag.aadharexist = "Aadhar already exists";
                            flag = false;
                            break;
                        }
                        else if (empl[i].mobile == reg.mobile)

                        {
                            ViewBag.mobexist = "Mobile number already exists";
                            flag = false;
                            break;

                        }

                        //break;

                    }
                }
                //else
                if (flag == true || empl.Count < 1)

                {

                    // var name = businesobj.actypcrt.Where(c => c.acc_type_id == reg.acctype);
                    // foreach (var acctypname in name)

                    //     reg.acctypename = acctypname.account_type;

                    var rolenames = businesobj.role.Where(x => x.rolename == "Customer" & x.isactive == true);

                    foreach (var rolename in rolenames)

                        reg.role = rolename.rolename;
                    // var acctyplist = businesobj.actypcrt.Where(x => x.isactive == true);
                    // ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type");
                    // bool flag = false;
                    //int useridds = 0;
                    // var emailist = businesobj.registration.ToList();
                    var aclimit = businesobj.actypcrt.Find(reg.acctype);
                    reg.bank_balance = aclimit.min_limit;
                    int accountidd = AutoPRNo();
                    var roleids = businesobj.role.Find("2C");// role id is placed here
                    reg.roleid = roleids.roleid;

                    //string passwordd = password();



                    // for (int i = 0; i < emailist.Count - 1; i++)
                    // {
                    //     if (emailist[i].Email == reg.Email)
                    //     {
                    //         ViewBag.Message = "Email already exists!";
                    //         flag = true;
                    //         break;

                    //     }
                    // }

                    //if (flag == false)

                    var registration = new Registration()
                    {
                        acctype = reg.acctype,
                        Name = reg.Name,
                        DOB = reg.DOB,
                        Email = reg.Email,
                        Gender = reg.Gender,
                        acctypename = reg.acctypename,
                        mobile = reg.mobile,
                        role = reg.role,
                        roleid = reg.roleid,
                        bank_balance = reg.bank_balance,
                        isActive = true,
                        aadhar = reg.aadhar,
                        AccountCreationDate = DateTime.Now,
                        password = passwordd,
                        accountnumber = Convert.ToString(accountidd),


                    };
                    businesobj.registration.Add(registration);
                    businesobj.SaveChanges();
                    var userids = businesobj.registration.Where(c => c.Email == reg.Email);
                    foreach (var useridd in userids)
                        useridds = useridd.userid;

                    var bankbal = new bankbal()
                    {
                        isactive = true,
                        bank_balance = reg.bank_balance,
                        accountnumber = Convert.ToString(accountidd),
                        userid = useridds,

                    };
                    businesobj.bankbal.Add(bankbal);
                    businesobj.SaveChanges();
                    var logindetails = new logindetails()

                    {
                        userid = useridds,
                        Email = reg.Email,
                        password = passwordd,
                        role = reg.role,
                        isactive = true,
                        LastLoginDate = DateTime.Now,


                    };
                    businesobj.logindetails.Add(logindetails);
                    businesobj.SaveChanges();

                    var transactions = new Transactions()
                    {
                        Transactionamount = reg.bank_balance,
                        accountnumber = Convert.ToString(accountidd),
                        bank_balance = reg.bank_balance,
                        Transaction_Date = DateTime.Now,
                        tr_type = "Deposit",
                        acctypename = reg.acctypename,
                        previous_balance = reg.bank_balance,
                    };
                    businesobj.Transaction.Add(transactions);
                    businesobj.SaveChanges();

                    return View("Index");

                }

            }
            return View();
        }

        public string password()
        {
            string numbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*";
            Random objrandom = new Random();
            string passwordString = "";
            string strrandom = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                passwordString = numbers.ToCharArray()[temp].ToString();
                strrandom += passwordString;
            }
            //ViewBag.strongpwd = strrandom;
            TempData["pass"] = strrandom;
            return strrandom;
        }
        // int acoountiid;
        public int AutoPRNo()
        {

            string startingno = "1239";
            Random r = new Random();
            string lastno = r.Next(0, 999999).ToString();
            string accno = startingno + lastno;
            int accnumber = Convert.ToInt32(accno);

            return accnumber;
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //IEnumerable<Registration> custdetails = businesobj.registration.Where(x => x.userid == id);

            resultviewmodel mymodel = new resultviewmodel();
            var accounts = Getaccounts();
            mymodel.Registerview = Getaccounts().Where(x => x.userid == id);

            if (mymodel.Registerview == null)
            {
                return HttpNotFound();
            }
            return View(mymodel);

        }
        string acnum;
        string actype, email;
        float bankbal;

        public ActionResult Editcustacc(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //IEnumerable<Registration> ids = businesobj.registration.Where(x => x.userid == id);
            resultviewmodel ids = new resultviewmodel();
            //var accounts = Getaccounts();
           ids.Registerview = Getaccounts().Where(x => x.userid == id);
            if (ids == null)
            {
                return HttpNotFound();
            }
           

                foreach (var iems in ids.Registerview)
                {
                    acnum = iems.accountnumber;
                    actype = iems.acctype;
                    email = iems.Email;

                }

                var acctyplist = businesobj.actypcrt.ToList();
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", actype);
            ViewBag.accountnumber = acnum;
            var acctype = businesobj.actypcrt.Find(actype);

            ViewBag.Accounttype = acctype.account_type;
            ViewBag.Email = email;
            //ViewBag.
            return View(ids);
        }

        [HttpPost]
        public ActionResult Editcustacc(Registration accparm)
        {
            var acctyplist = businesobj.actypcrt.ToList();
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", accparm.acctype);
            //  var aclimit = businesobj.actypcrt.Find(accparm.acctype);
            // accparm.bank_balance = aclimit.min_limit;
            var name = businesobj.actypcrt.Where(c => c.acc_type_id == accparm.acctype);
            foreach (var acctypname in name)
                accparm.acctypename = acctypname.account_type;
            //accparm.accountid = ViewBag.accountid;
            int x = Convert.ToInt32(TempData["accountid"]);
            accparm.Email = TempData["Email"].ToString();
            accparm.accountnumber = Convert.ToString(x);
            businesobj.Entry(accparm).State = EntityState.Modified;
            businesobj.SaveChanges();
            return RedirectToAction("Accountlist");
        }

        public ActionResult Delete(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //IEnumerable<Registration> ids = businesobj.registration.Where(x => x.userid == id);
            //var accounts = Getaccounts();
            resultviewmodel ids = new resultviewmodel();
            ids.Registerview = Getaccounts().Where(x => x.userid == id);
            //var ids = businesobj.registration.Find(id);
            if (ids == null)
            {
                return HttpNotFound();
            }


            return View(ids);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult Delete1(int id)
        {
            var ids = businesobj.registration.Find(id);
            ids.isActive = false;
            //businesobj.businesses.Remove(ids);

            businesobj.SaveChanges();
            return RedirectToAction("Index");

        }
        private static List<Transactions> GetTransactions()
        {
            acccreatecontext db = new acccreatecontext();

            var transactionss = db.Transaction.ToList();
            return transactionss;
        }

        private static List<Registration> Getaccounts()
        {
            acccreatecontext db = new acccreatecontext();

            var accounts = db.registration.ToList();
            return accounts;
        }
        public ActionResult transactions(int id)
        {
            string accnumber = Convert.ToString(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resultviewmodel mymodel = new resultviewmodel();
            
            mymodel.Transactionsviewlist = GetTransactions().Where(x => x.accountnumber == id.ToString()).OrderByDescending(x=>x.TransactionID).ToList();
            if(mymodel.Transactionsviewlist==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach(var items in mymodel.Transactionsviewlist)
            {
                ViewBag.acnum = items.accountnumber;
                ViewBag.accountype = items.acctypename;
                ViewBag.bankbal = items.bank_balance;
            }
            //mymodel.Registerview = Getaccounts();
            return View(mymodel);
            
           
            //if (bankbalstbl == null)
            //{
            //    return HttpNotFound();
            //}


            //return View(bankbalstbl);
            // var bankbals = businesobj.bankbal.Find(id);
            // var bankbal = bankbals.bank_balance;
            //bankbals.userid
            // return View();

        }
        public ActionResult transfer(string id)
        {
            string accnumber = Convert.ToString(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resultviewmodel mymodel = new resultviewmodel();

            //mymodel.Transactionsview = GetTransactions().OrderByDescending(x=>x.TransactionID).Where(x=>x.accountnumber == id.ToString()).FirstOrDefault();
            mymodel.Transactionsviewlist = GetTransactions().Where(x => x.accountnumber == id.ToString())
                 .OrderByDescending(x => x.TransactionID).Take(1).ToList();
                 
            if (mymodel.Transactionsviewlist == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (var items in mymodel.Transactionsviewlist)
            {
                ViewBag.acnum = items.accountnumber;
                ViewBag.accountype = items.acctypename;
                ViewBag.bankbal = items.bank_balance;
                ViewBag.transactionid = items.TransactionID;
            }
            //mymodel.Registerview = Getaccounts();
            return View(mymodel);

        }
        [HttpPost]
        public ActionResult transfer(Transactions transactionparam)
        {
            float bal = Convert.ToSingle(TempData["bankblc"]);


            if (transactionparam.Transactionamount > 0 && transactionparam.tr_type == "Deposit")
            {
                bal = transactionparam.Transactionamount + bal;
            }
            else if (transactionparam.Transactionamount > 0 && transactionparam.tr_type == "Withdraw")
            {
                bal = bal - transactionparam.Transactionamount;
            }
            else if(transactionparam.Transactionamount<=0 &&(transactionparam.tr_type=="Deposit"||transactionparam.tr_type=="Withdraw"))
            {
                ViewBag.transactamountmsg = "Please enter amount greater that 100";
            }
            transactionparam.bank_balance = bal;
            transactionparam.accountnumber = TempData["accountid"].ToString();
            transactionparam.Transaction_Date = DateTime.Now;
            transactionparam.acctypename = TempData["acctypname"].ToString();
            transactionparam.previous_balance = Convert.ToSingle(TempData["bankblc"]);
            int tid = Convert.ToInt32(TempData["trid"]);
            transactionparam.TransactionID = tid + 1;
            // businesobj.Entry(transactionparam).State = EntityState.Modified;
            businesobj.Transaction.Add(transactionparam);
            businesobj.SaveChanges();

            //var registrationu = new Registration()
            //{
            //    bank_balance = transactionparam.bank_balance,            
            //};
            //businesobj.Entry(registrationu).State = EntityState.Modified;
            //businesobj.SaveChanges();
            return View();

        }

        //public ActionResult Transactionhistory()
        //{
        //    //string accnumber = Convert.ToString(id);
        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}

        //    var accnumber = TempData["accountid"].ToString();

        //    var transactionhistory = businesobj.Transaction.Where(x => x.accountnumber == accnumber).OrderByDescending(x => x.TransactionID);
        //    foreach (var transactiondetails in transactionhistory)
        //    {
        //        ViewBag.accountnumber = transactiondetails.accountnumber;
        //        ViewBag.acctype = transactiondetails.acctypename;
        //    }
        //    return View(transactionhistory);
        //}

    }
}