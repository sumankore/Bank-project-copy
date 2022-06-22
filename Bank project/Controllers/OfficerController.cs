using Bank_project.Models;
using Bank_project.security;
using PagedList;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
            var customers = from accs in businesobj.registration.Where(x => x.role == "Customer" && x.isActive == true) select accs;
            if (!String.IsNullOrEmpty(Search_Data))

            {
                customers = customers.Where(accs => accs.Name.ToUpper().Contains(Search_Data.ToUpper())
                   /* || accs.LastName.ToUpper().Contains(Search_Data.ToUpper())*/);
            }
            // var customers = /*from stu in  select stu;*/businesobj.registration.Where(x => x.role == "Customer"&x.isActive==true);
            switch (Sorting_Order)
            {
                case "Name_Description":
                    customers = customers.OrderByDescending(x => x.Name);
                    break;
                case "Date":
                    customers = customers.OrderByDescending(x => x.AccountCreationDate);
                    break;

                case "Date_Enroll":

                    customers = customers.OrderBy(x => x.DOB);

                    break;
                default:
                    customers = customers.OrderBy(x => x.accountnumber);
                    break;
            }

            resultviewmodel srVM = new resultviewmodel();
            int pageSize = 3;
            int pageNumber = (Page_No ?? 1);
            srVM.Registerviewpl = customers.ToList().ToPagedList(pageNumber, pageSize);
            return View(srVM);


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


                    ViewBag.Email = Session["Email"];
                    reg.ActivationCode = Guid.NewGuid().ToString(); ;
                    SendEmailToUser(reg.Email, reg.ActivationCode.ToString());
                    var registration = new Registration()
                    {
                        EmailVerification = false,
                        acctype = reg.acctype,
                        Name = reg.Name,
                        DOB = reg.DOB,
                        Email = reg.Email,
                        Gender = reg.Gender,
                        acctypename = reg.acctypename,
                        mobile = reg.mobile,
                        role = reg.role,
                        roleid = reg.roleid,
                        ActivationCode = reg.ActivationCode,
                        accountcreatedby = ViewBag.Email,
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
                    ViewBag.regsuccess = "Your registration is successful please check your email";
                    return View("Registrationsuccess");
                }

            }
            return View();
        }

        public ActionResult Registrationsuccess()
        {
            return View();
        }
        public void SendEmailToUser(string emailId, string activationCode)
        {
            var GenarateUserVerificationLink = "admin/UserVerification/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

            var fromMail = new MailAddress("sumankore121@gmail.com", "Bank Name"); // set your email    
            var fromEmailpassword = "bhxavspkfpwckuae"; // Set your password     
            var toEmail = new MailAddress(emailId);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            //smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("sumankore121@gmail.com", "bhxavspkfpwckuae");
            //smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);
            smtp.EnableSsl = true;
            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "Registration Completed";
            Message.Body = "<br/> Your registration completed succesfully." +
                           "<br/> please click on the below link for account verification" +
                           "<br/><br/><a href=" + link + ">" + link + "</a>";
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }
        [HttpPost]
        public ActionResult UserVerification(string id)
        {
            bool Status = false;

            businesobj.Configuration.ValidateOnSaveEnabled = false; // Ignor to password confirmation     
            var IsVerify = businesobj.registration.Where(u => u.ActivationCode == new Guid(id).ToString()).FirstOrDefault();

            if (IsVerify != null)
            {
                IsVerify.EmailVerification = true;
                businesobj.SaveChanges();
                ViewBag.Message = "Email Verification completed";
                Status = true;
                ViewBag.pass = "Please reset your password before logging in";
            }
            else
            {
                ViewBag.Message = "Invalid Request...Email not verified";
                ViewBag.Status = false;
            }

            return View();
        }



        public ActionResult userverification()
        {

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
            //return strrandom;
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(strrandom)));
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


            resultviewmodel mymodel = new resultviewmodel();

            mymodel.Registerviewlist = Getaccounts().Where(x => x.userid == id).ToList();
            if (mymodel.Registerviewlist == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (var items in mymodel.Registerviewlist)
            {
                ViewBag.acnum = items.accountnumber;
                ViewBag.accountype = items.acctypename;
                //ViewBag.bankbal = items.bank_balance;
                ViewBag.email = items.Email;
            }
            //mymodel.Registerview = Getaccounts();
            return View(mymodel);


        }

        [HttpPost]
        public ActionResult Editcustacc(resultviewmodel accparm)
        {
            for (int i = 0; i < accparm.Registerviewlist.Count(); i++)
            {
                var acctyplist = businesobj.actypcrt.ToList();
                ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", accparm.Registerviewlist[i].acctype);
                //  var aclimit = businesobj.actypcrt.Find(accparm.acctype);
                // accparm.bank_balance = aclimit.min_limit;
                var name = businesobj.actypcrt.ToList().Where(c => c.acc_type_id == accparm.Registerviewlist[i].acctype);
                foreach (var acctypname in name)
                    accparm.Registerviewlist[i].acctypename = acctypname.account_type;
                //accparm.accountid = ViewBag.accountid;
                int x = Convert.ToInt32(TempData["accountid"]);
                accparm.Registerviewlist[i].Email = TempData["Email"].ToString();
                accparm.Registerviewlist[i].accountnumber = Convert.ToString(x);
                businesobj.Entry(accparm.Registerviewlist[i]).State = EntityState.Modified;
                businesobj.SaveChanges();
            }
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

            mymodel.Transactionsviewlist = GetTransactions().Where(x => x.accountnumber == id.ToString()).OrderByDescending(x => x.TransactionID).ToList();
            if (mymodel.Transactionsviewlist == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (var items in mymodel.Transactionsviewlist)
            {
                ViewBag.acnum = items.accountnumber;
                ViewBag.accountype = items.acctypename;
                ViewBag.bankbal = items.bank_balance;
            }
            //mymodel.Registerview = Getaccounts();
            return View(mymodel);

        }


        public ActionResult PrintPartialViewToPdf(int id)
        {
            resultviewmodel mymodel = new resultviewmodel();
            mymodel.Transactionsviewlist = GetTransactions().Where(x => x.accountnumber == id.ToString()).OrderByDescending(x => x.TransactionID).ToList();
            List<Transactions> Data = mymodel.Transactionsviewlist;
            //List<Transactions> Data = businesobj.Transaction.Where(x => x.accountnumber == id.ToString()).OrderByDescending(x => x.TransactionID).ToList();
            foreach (var items in mymodel.Transactionsviewlist)
            {
                ViewBag.acnum = items.accountnumber;
                ViewBag.accountype = items.acctypename;
                ViewBag.bankbal = items.bank_balance;
            }
            string n = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}.pdf",DateTime.Now);
            var fname = ViewBag.acnum+n ;
            return new PartialViewAsPdf("_JobPrint", mymodel)
            {
                //FileName = "TestPartialViewAsPdf.pdf"
                FileName = fname

            };
        }
        public ActionResult transfer(string id)
        {
            string accnumber = Convert.ToString(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            resultviewmodel mymodel = new resultviewmodel();
            //var transactionlist = businesobj.Transaction.Where(x => x.accountnumber == id);
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
        public ActionResult transfer(resultviewmodel transactionparam)
        {
            float bal = Convert.ToSingle(TempData["bankblc"]);
            for (int i = 0; i < transactionparam.Transactionsviewlist.Count; i++)
            {

                if (transactionparam.Transactionsviewlist[i].Transactionamount > 0 && transactionparam.Transactionsviewlist[i].tr_type == "Deposit")
                {
                    bal = transactionparam.Transactionsviewlist[i].Transactionamount + bal;
                }
                else if (transactionparam.Transactionsviewlist[i].Transactionamount > 0 && transactionparam.Transactionsviewlist[i].tr_type == "Withdraw")
                {
                    bal = bal - transactionparam.Transactionsviewlist[i].Transactionamount;
                }
                else if (transactionparam.Transactionsviewlist[i].Transactionamount <= 0 && (transactionparam.Transactionsviewlist[i].tr_type == "Deposit" || transactionparam.Transactionsviews.tr_type == "Withdraw"))
                {
                    ViewBag.transactamountmsg = "Please enter amount greater that 100";
                }
                transactionparam.Transactionsviewlist[i].bank_balance = bal;
                transactionparam.Transactionsviewlist[i].accountnumber = TempData["accountid"].ToString();
                transactionparam.Transactionsviewlist[i].Transaction_Date = DateTime.Now;
                transactionparam.Transactionsviewlist[i].acctypename = TempData["acctypname"].ToString();
                transactionparam.Transactionsviewlist[i].previous_balance = Convert.ToSingle(TempData["bankblc"]);
                int tid = Convert.ToInt32(TempData["trid"]);
                transactionparam.Transactionsviewlist[i].TransactionID = tid + 1;
                // businesobj.Entry(transactionparam).State = EntityState.Modified;
                businesobj.Transaction.Add(transactionparam.Transactionsviewlist[i]);
                businesobj.SaveChanges();
            }
            //var registrationu = new Registration()
            //{
            //    bank_balance = transactionparam.bank_balance,            
            //};
            //businesobj.Entry(registrationu).State = EntityState.Modified;
            //businesobj.SaveChanges();
            return View("Transactions");

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