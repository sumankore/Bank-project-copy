using Bank_project.Models;
using Bank_project.security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var accounts = businesobj.actypcrt.ToList();

            return View(accounts);
        }

        public ActionResult Createac(acctypes customermdlparam)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }
        public ActionResult actypcreate()
        {

            return View();

        }
        [HttpPost]
        public ActionResult actypcreate(acctypes actyparam)
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
                    actyparam.isactive = true;
                    businesobj.actypcrt.Add(actyparam);
                    businesobj.SaveChanges();

                }

            }
            ModelState.Clear();


            return View(actyparam);

        }

        public ActionResult Editacctyp(string id)
        {

            var ids = businesobj.actypcrt.Find(id);
            return View(ids);
        }

        [HttpPost]
        public ActionResult Editacctyp(acctypes acctyp)
        {

            //if (ModelState.IsValid)
            //{

            //    var acctypes = new acctypes()
            //    {
            //        //acc_type_id = acctyp.acc_type_id,
            //        account_type = acctyp.account_type,
            //        min_limit = acctyp.min_limit,
            //        isactive = acctyp.isactive,
            //    };
            //    businesobj.actypcrt.Add(acctypes);
            //    businesobj.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //var acctyplist = businesobj.actypcrt.ToList();
            //ModelState.Clear();
            //return View(acctyp);
            businesobj.Entry(acctyp).State = EntityState.Modified;
            businesobj.SaveChanges();
            return RedirectToAction("Accounttypelist");
        }

        public ActionResult Deleteactyp(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ids = businesobj.actypcrt.Find(id);
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
            var roles = businesobj.role.ToList();
            //var roles = businesobj.role.Find(roles.)

            return View(roles);
        }

        public ActionResult rolecreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult rolecreate(roles roleadd)

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
                    roleadd.isactive = true;
                    businesobj.role.Add(roleadd);
                    businesobj.SaveChanges();

                }

            }
            return View(roleadd);
        }

        public ActionResult editrole(string id)
        {

            var ids = businesobj.role.Find(id);
            return View(ids);
        }


        [HttpPost]
        public ActionResult editrole(roles role)
        {
            businesobj.Entry(role).State = EntityState.Modified;
            businesobj.SaveChanges();
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

        public ActionResult Employees()

        {
            var employlist = businesobj.registration.Where(x => x.role != "Customer"&&x.role!="Admin");
            //   var employees = businesobj.registration.ToList();

            return View(employlist);
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
                    ////if (flag == false)
                    ////{

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
                    return View("Index");
                }
            }

            return View();

        }
            //   bool flag = false;
            //   var name = businesobj.actypcrt.Where(c => c.acc_type_id == reg.acctype);
            //   foreach (var acctypname in name)

            //       reg.acctypename = acctypname.account_type;

            //   var rolenames = businesobj.role.Where(c => c.roleid== reg.role);
            //   foreach (var rolename in rolenames)

            //       reg.role=rolename.rolename;

            //   //var rolenames = businesobj.role.Where(x => x.rolename != "Customer" & x.isactive == true);

            //   //foreach (var rolename in rolenames)

            //   //    reg.role = rolename.rolename;
            //   var useridds =0;


            //   var emailist = businesobj.registration.ToList();

            ////   var aclimit = businesobj.actypcrt.Find(reg.acctype);
            //  // int accountidd = AutoPRNo();
            //   string passwordd = password();
            //   //  reg.bank_balance = aclimit.min_limit;

            //   for (int i = 0; i < emailist.Count - 1; i++)
            //   {
            //       if (emailist[i].Email == reg.Email)
            //       {
            //           ViewBag.Message = "Email already exists!";
            //           flag = true;
            //           break;

            //       }
            //   }
            //   if (flag == false)
            //   {
            //       var registration = new Registration()
            //       {
            //          acctype = reg.acctype,
            //           Name = reg.Name,
            //           DOB = reg.DOB,
            //           Email = reg.Email,
            //           Gender = reg.Gender,
            //           acctypename = reg.acctypename,
            //           mobile = reg.mobile,
            //           role = reg.role,
            //           // bank_balance = reg.bank_balance,
            //           isActive = true,
            //           aadhar = reg.aadhar,
            //           AccountCreationDate = DateTime.Now,
            //           password = passwordd,
            //          accountid = reg.accountid,
            //          };
            //       businesobj.registration.Add(registration);
            //       businesobj.SaveChanges();

            //       var userids = businesobj.registration.Where(c => c.Email == reg.Email);
            //       foreach (var useridd in userids)
            //           useridds = useridd.userid;

            //       var logindetails = new logindetails()

            //       {
            //           isactive = true,
            //           Email = reg.Email,
            //           password = passwordd,
            //           role = reg.role,
            //           userid = useridds,
            //           LastLoginDate = DateTime.Now,

            //       };
            //       businesobj.logindetails.Add(logindetails);
            //       businesobj.SaveChanges();
            //       return View("Index");
            //   }
            //   return View();

            public ActionResult Editempacc(int id)
        {

            var ids = businesobj.registration.Find(id);
            var acctyplist = businesobj.actypcrt.ToList();
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", ids.acctype);
            var rolelist = businesobj.role.Where(x => x.isactive == true & x.rolename != "Customer");
            //var rolelist= businesobj.role.ToList();
            ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename", ids.roleid);
            ViewBag.accountid = ids.accountnumber;
            ViewBag.Email = ids.Email;
            //ViewBag.
            return View(ids);
        }
        int useridds = 0;
        [HttpPost]
        public ActionResult Editempacc(Registration accparm)
        {
            var acctyplist = businesobj.actypcrt.ToList();
            ViewBag.acctypes = new SelectList(acctyplist, "acc_type_id", "account_type", accparm.acctype);
            var name = businesobj.actypcrt.Where(c => c.acc_type_id == accparm.acctype);
            foreach (var acctypname in name)
                accparm.acctypename = acctypname.account_type;

            var rolelist = businesobj.role.ToList();
            ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename",accparm.roleid);
            var rolenames = businesobj.role.Where(c => c.roleid == accparm.roleid);
            foreach (var rolename in rolenames)
                accparm.role = rolename.rolename;
            //  var aclimit = businesobj.actypcrt.Find(accparm.acctype);
            // accparm.bank_balance = aclimit.min_limit;
            
            //int x = Convert.ToInt32(TempData["accountid"]);
            string x = Convert.ToString(TempData["accountid"]);

            accparm.Email = TempData["Email"].ToString();
            accparm.accountnumber = x;
            businesobj.Entry(accparm).State = EntityState.Modified;
            businesobj.SaveChanges();
            var userids = businesobj.logindetails.Where(c => c.Email == accparm.Email);
            foreach (var useridd in userids)
                // useridds = useridd.loginid;
                businesobj.logindetails.Remove(useridd);
            var logindetailss = new logindetails()

            {
                Email = accparm.Email,
                userid = accparm.userid,
                password = accparm.password,
                role = accparm.role,
                AccountLocked = false,
                isactive = true,
                loginid = useridds,
                LastLoginDate = DateTime.Now,
            };
            businesobj.logindetails.Add(logindetailss);
            //businesobj.Entry(logindetailss).State = EntityState.Added;
            businesobj.SaveChanges();
            return RedirectToAction("Employees");

        }

        public string password()
        {
            string numbers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz@!$#%&*";
            Random objrandom = new Random();
            string passwordString = "";
            string strrandom = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                int temp = objrandom.Next(0, numbers.Length);
                passwordString = numbers.ToCharArray()[temp].ToString();
                strrandom += passwordString;
            }
            return strrandom;
        }
        public ActionResult empdetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var custdetails = businesobj.registration.Find(id);
            if (custdetails == null)
            {
                return HttpNotFound();
            }
            return View(custdetails);
        }
        public ActionResult empldelete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var custdetails = businesobj.registration.Find(id);
            if (custdetails == null)
            {
                return HttpNotFound();
            }
            return View(custdetails);
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
    }
}


