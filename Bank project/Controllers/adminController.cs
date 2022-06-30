using Bank_project.Models;
using Bank_project.security;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    [CustomAuthorize("Admin")]
    public class adminController : Controller
    {
        acccreatecontext businesobj = new acccreatecontext();
        acctypes custobj = new acctypes();
        [CustomAuthorize("Admin")]
        public ActionResult Index()
        {
            ViewBag.Email = Session["Email"];
            ViewBag.Userid = Session["userid"];
            return View();
        }

        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "Un Authorized Page!";

            return View();
        }


        public ActionResult Accounttypelist()
        {
            //var accounts = businesobj.actypcrt.ToList();
            resultviewmodel rm = new resultviewmodel();
            rm.acctyplist = GetaccountsTypes().Where(x => x.isactive = true).ToList();

            return View(rm);
        }


        public ActionResult actypcreate()
        {

            return View();

        }
        [HttpPost]
        public ActionResult actypcreate(registrationc actyparam)
        {
            bool flag = false;
            var idlist = businesobj.actypcrt.ToList();
            for (int i = 0; i < idlist.Count; i++)
            {
                if (idlist[i].acc_type_id == actyparam.acc_type_id)
                {
                    ViewBag.acidexist = "Account id type already exists";
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                if (ModelState.IsValid)
                {
                    var actypes = new acctypes()
                    {
                        account_type = actyparam.account_type,
                        acc_type_id = actyparam.acc_type_id,
                        min_limit = actyparam.min_limit,
                        isactive = true,

                    };


                    businesobj.actypcrt.Add(actypes);

                    businesobj.SaveChanges();
                    
                }

            }
            ModelState.Clear();


            return RedirectToAction("Accounttypelist");

        }

        public ActionResult Editacctyp(string id)
        {
            resultviewmodel rm = new resultviewmodel();
            rm.acctyplist = GetaccountsTypes().Where(x => x.acc_type_id == id).ToList();
            foreach (var item in rm.acctyplist)
            {
                ViewBag.Accountype = item.account_type;
                ViewBag.actypid = item.acc_type_id;

            }
            return View(rm);

        }

        [HttpPost]
        public ActionResult Editacctyp(resultviewmodel acctyp)
        {
            for (int i = 0; i < acctyp.acctyplist.Count; i++)
            {

                acctyp.acctyplist[i].account_type = @TempData["actype"].ToString();
                acctyp.acctyplist[i].acc_type_id = @TempData["actypeid"].ToString();
                businesobj.Entry(acctyp.acctyplist[i]).State = EntityState.Modified;
                businesobj.SaveChanges();
            }
            return RedirectToAction("Accounttypelist");
        }

        public ActionResult Deleteactyp(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            resultviewmodel ids = new resultviewmodel();
            ids.acctyplist = GetaccountsTypes().Where(x => x.acc_type_id == id).ToList();
            //var ids = businesobj.registration.Find(id);
            if (ids == null)
            {
                return HttpNotFound();
            }
            return View(ids);
        }



        [HttpPost]
        [ActionName("Deleteactyp")]
        public ActionResult Deleteacty(string id)
        {
            var ids = businesobj.actypcrt.Find(id);
            //businesobj.actypcrt.Remove(ids);
            ids.isactive = false;

            businesobj.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult roleslist()
        {
            resultviewmodel rm = new resultviewmodel();
            rm.roletyplist = GetrolesTypes();

            return View(rm);

            //var roles = businesobj.role.ToList();
            ////var roles = businesobj.role.Find(roles.)


            //return View(roles);
        }

        public ActionResult rolecreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult rolecreate(Registration roleadd)

        {
            bool flag = false;
            var idlist = businesobj.role.ToList();
            for (int i = 0; i < idlist.Count; i++)
            {
                if (idlist[i].roleid == roleadd.roleid)
                {
                    ViewBag.acidexist = "Role id already exists";
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                if (ModelState.IsValid)
                {
                    var roles = new roles()
                    {
                        roleid = roleadd.roleid,
                        isactive = true,
                        rolename = roleadd.role,
                    };

                    businesobj.role.Add(roles);
                    businesobj.SaveChanges();

                }

            }
            return RedirectToAction("roleslist");
        }

        public ActionResult editrole(string id)
        {
            resultviewmodel rm = new resultviewmodel();
            rm.roletyplist = GetrolesTypes().Where(x => x.roleid == id).ToList();
            foreach (var item in rm.roletyplist)
            {
                ViewBag.Rolename = item.rolename;
                ViewBag.roleid = item.roleid;

            }
            return View(rm);

            //var ids = businesobj.role.Find(id);
            //return View(ids);
        }


        [HttpPost]
        public ActionResult editrole(resultviewmodel role)
        {
            for (int i = 0; i < role.roletyplist.Count; i++)
            {

                role.roletyplist[i].roleid = @TempData["roleid"].ToString();
                businesobj.Entry(role.acctyplist[i]).State = EntityState.Modified;
                businesobj.SaveChanges();

                //businesobj.Entry(role).State = EntityState.Modified;
                //businesobj.SaveChanges();
            }
            return RedirectToAction("roleslist");
        }

        public ActionResult Deleterole(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ids = businesobj.role.Find(id);
            if (ids == null)
            {
                return HttpNotFound();
            }
            return View(ids);
        }



        [HttpPost]
        [ActionName("Deleterole")]
        public ActionResult Deleteroles(string id)
        {
            var ids = businesobj.role.Find(id);
            //businesobj.actypcrt.Remove(ids);
            ids.isactive = false;

            businesobj.SaveChanges();
            return RedirectToAction("Index");

        }


        public ActionResult Employees(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
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
            var employeelist = from accs in businesobj.registration.Where(x => x.role != "Customer" && x.isActive == true) select accs;
            if (!String.IsNullOrEmpty(Search_Data))

            {
                employeelist = employeelist.Where(accs => accs.Name.ToUpper().Contains(Search_Data.ToUpper()));
            }
            switch (Sorting_Order)
            {
                case "Name_Description":
                    employeelist = employeelist.OrderByDescending(x => x.Name);
                    break;
                case "Date":
                    employeelist = employeelist.OrderByDescending(x => x.AccountCreationDate);
                    break;

                case "Date_Enroll":

                    employeelist = employeelist.OrderBy(x => x.DOB);

                    break;
                default:
                    employeelist = employeelist.OrderBy(x => x.accountnumber);
                    break;
            }

            resultviewmodel srVM = new resultviewmodel();
            int pageSize = 3;
            int pageNumber = (Page_No ?? 1);
            srVM.Registerviewpl = employeelist.ToList().ToPagedList(pageNumber, pageSize);
            return View(srVM);


        }



        public ActionResult addemployee()

        {
            var rolelist = businesobj.role.Where(x => x.isactive == true & x.rolename != "Customer");
            ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename");
            var acctyplist = businesobj.actypcrt.Where(x => x.isactive == true);
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type");
            return View();
        }


        [HttpPost]
        public ActionResult addemployee(registrationc reg)
        {
            bool flag = false;

            var rolelist = businesobj.role.Where(x => x.isactive == true & x.rolename != "Customer");
            ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename");
            var rolenames = businesobj.role.Where(c => c.roleid == reg.roleid);
            foreach (var rolename in rolenames)
                reg.role = rolename.rolename;

            var acctyplist = businesobj.actypcrt.Where(x => x.isactive == true);
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type");
            var name = businesobj.actypcrt.Where(c => c.acc_type_id == reg.acctype);
            foreach (var acctypname in name)
                reg.acctypename = acctypname.account_type;
            var useridds = 0;
            string passwordd = password();
            var empl = businesobj.registration.Where(x => x.accountnumber == reg.accountnumber || x.Email == reg.Email || x.aadhar == reg.aadhar || x.mobile == reg.mobile).ToList();
            //if(emplist!=null)
            //{
            //    ViewBag.alreadyexist = "Account Already exists";
            //    return View();
            //}

            // var empl = businesobj.registration.FirstOrDefault(x => x.Email == reg.Email);
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
                    //code 1 working
                    ////   var emailist = businesobj.registration.ToList();



                    ////   for (int i = 0; i < emailist.Count - 1; i++)
                    ////   {
                    ////       if (emailist[i].Email == reg.Email)
                    ////       {
                    ////           ViewBag.Message = "Email already exists!";
                    ////           flag = true;
                    ////           break;
                    ////       }
                    ////   }
                   
                  //var roleids = businesobj.role.Where(x=>x.roleid==reg.roleid);
                  //  foreach (var role in roleids)
                  //  {
                  //      reg.roleid = role.roleid;
                  //      reg.role = role.rolename;
                  //  }
                    ViewBag.Email = Session["Email"];
                    reg.ActivationCode = Guid.NewGuid().ToString(); ;
                    SendEmailToUser(reg.Email, reg.ActivationCode.ToString());
                    var registration = new Registration()
                    {
                        roleid = reg.roleid,
                        acctype = reg.acctype,
                        Name = reg.Name,
                        DOB = reg.DOB,
                        Email = reg.Email,
                        Gender = reg.Gender,
                        acctypename = reg.acctypename,
                        mobile = reg.mobile,
                        accountnumber = reg.accountnumber,
                        role = reg.role,
                        isActive = true,
                        aadhar = reg.aadhar,
                        AccountCreationDate = DateTime.Now,
                        password = passwordd,
                        ActivationCode = reg.ActivationCode,
                        accountcreatedby = ViewBag.Email,
                        //accountid = reg.accountid,

                    };
                    // if(emailist[i].accountid==reg.accountid||reg.accountid==0  )
                    businesobj.registration.Add(registration);
                    businesobj.SaveChanges();
                    var userids = businesobj.registration.Where(c => c.Email == reg.Email);
                    foreach (var useridd in userids)
                        useridds = useridd.userid;
                    var logindetails = new logindetails()
                    {
                        isactive = true,
                        Email = reg.Email,
                        password = passwordd,
                        role = reg.role,
                        userid = useridds,
                        LastLoginDate = DateTime.Now,
                    };
                    businesobj.logindetails.Add(logindetails);
                    businesobj.SaveChanges();
                    ViewBag.regsuccess = "Your registration is successful please check your email";
                    return View("Registrationsuccess");
                    //return View("Index");
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


        public ActionResult Editempacc(int id)
        {

            //var ids = businesobj.registration.Find(id);
            //var acctyplist = businesobj.actypcrt.ToList();
            //ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", ids.acctype);
            //var rolelist = businesobj.role.Where(x => x.isactive == true & x.rolename != "Customer");
            ////var rolelist= businesobj.role.ToList();
            //ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename", ids.roleid);


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
        int useridds = 0;
        [HttpPost]
        public ActionResult Editempacc(resultviewmodel accparm)
        {
            //var acctyplist = businesobj.actypcrt.ToList();
            //ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", accparm.acctype);
            //var name = businesobj.actypcrt.Where(c => c.acc_type_id == accparm.acctype);
            //foreach (var acctypname in name)


            //var rolelist = businesobj.role.ToList();
            //ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename",accparm.roleid);
            //var rolenames = businesobj.role.Where(c => c.roleid == accparm.roleid);
            //foreach (var rolename in rolenames)
            //    accparm.role = rolename.rolename;


            //var names = "";
            for (int i = 0; i < accparm.Registerviewlist.Count(); i++)
            {
                var acctyplist = businesobj.actypcrt.ToList();
                ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", accparm.Registerviewlist[i].acctype);
                ////  var aclimit = businesobj.actypcrt.Find(accparm.acctype);
                //// accparm.bank_balance = aclimit.min_limit;
                var name = businesobj.actypcrt.ToList().Where(c => c.acc_type_id == accparm.Registerviewlist[i].acctype);
                foreach (var acctypname in name)
                    accparm.Registerviewlist[i].acctypename = acctypname.account_type;
                //accparm.accountid = ViewBag.accountid;
                //var x = Convert.ToInt32(TempData["accountid"]);
                accparm.Registerviewlist[i].Email = TempData["Email"].ToString();
                //accparm.Registerviewlist[i].accountnumber = Convert.ToString(x);
                businesobj.Entry(accparm.Registerviewlist[i]).State = EntityState.Modified;
                businesobj.SaveChanges();
            }
            return RedirectToAction("Employees");
            //  var userids = businesobj.logindetails.Where(c => c.Email == accparm.Registerviewlist[i].Email);
            // foreach (var useridd in userids)
            // useridds = useridd.loginid;
            // businesobj.logindetails.Remove(useridd);
            //var logindetailss = new logindetails()
            //{ 
            // for (int i = 0; i < accparm.Registerviewlist.Count();i++)
            //{ 
            //    Email = accparm.Registerviewlist[i].Email,
            //    userid = accparm.Registerviewlist[i].userid,
            //    password = accparm.Registerviewlist[i].password,
            //    role = accparm.Registerviewlist[i].role,
            //    AccountLocked = false,
            //    isactive = true,
            //    loginid = useridds,
            //    LastLoginDate = DateTime.Now,
            //};
            // businesobj.logindetails.Add(logindetailss);
            //businesobj.Entry(logindetailss).State = EntityState.Added;
            // businesobj.SaveChanges();
            return RedirectToAction("Employees");

        }

        //public string password()
        //{
        //    string numbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz@!$#%&*";
        //    Random objrandom = new Random();
        //    string passwordString = "";
        //    string strrandom = string.Empty;
        //    for (int i = 0; i < 8; i++)
        //    {
        //        int temp = objrandom.Next(0, numbers.Length);
        //        passwordString = numbers.ToCharArray()[temp].ToString();
        //        strrandom += passwordString;
        //    }
        //    return strrandom;
        //}
        public ActionResult empdetails(int? id)
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
        public ActionResult empldelete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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
        [ActionName("empldelete")]
        public ActionResult empdelete(int id)
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

        private static List<acctypes> GetaccountsTypes()
        {
            acccreatecontext db = new acccreatecontext();

            var accounts = db.actypcrt.ToList();
            return accounts;
        }
        private static List<roles> GetrolesTypes()
        {
            acccreatecontext db = new acccreatecontext();

            var accounts = db.role.ToList();
            return accounts;
        }

    }

}


