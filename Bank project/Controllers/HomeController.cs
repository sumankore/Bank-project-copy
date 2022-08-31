using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bank_project.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Text;

namespace Bank_project.Controllers
{
    
    public class HomeController : Controller
    {
        acccreatecontext businesobj = new acccreatecontext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LoginPage()
        {
            var rolelist = businesobj.role.Where(x => x.isactive == true);
            //var rolelist= businesobj.role.ToList();
            ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename");
            return View();
        }
        string roles = "";
        [HttpPost]
        public ActionResult LoginPage(logindetails loginn)
        {
            var rolelist = businesobj.role.Where(x => x.isactive == true);
            //var rolelist= businesobj.role.ToList();
            ViewBag.rolelist = new SelectList(rolelist, "roleid", "rolename");

            var email = loginn.Email.ToLower();

            var roleslist = businesobj.role.Where(p => p.roleid == loginn.role);
            foreach (var rolename in roleslist)
                //roles = rolename.roleid;
                roles = rolename.rolename;
            var get_user = businesobj.logindetails.FirstOrDefault(p => p.Email.ToLower() == loginn.Email.ToLower());
            if (get_user != null)
            {
                var password = get_user.password;
                var accountblocked = get_user.AccountLocked;
                var loginpass = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(loginn.password)));
                bool IsValidUser = password == loginpass && accountblocked == false && roles == "Employee";
                //if (password == loginn.password && accountblocked == false && roles == "Employee")
                if (IsValidUser)
                {
                    Session["userid"] = get_user.userid.ToString();
                    Session["Email"] = get_user.Email.ToString();
                    get_user.LastLoginDate = DateTime.Now;
                    businesobj.SaveChanges();
                    FormsAuthentication.SetAuthCookie(loginn.Email, false);
                    return RedirectToAction("Index", "Officer");

                }
                else if (password == loginpass && accountblocked == false && roles == "Customer")
                {
                    Session["userid"] = get_user.userid.ToString();
                    Session["Email"] = get_user.Email.ToString();
                    get_user.LastLoginDate = DateTime.Now;
                    businesobj.SaveChanges();
                    return RedirectToAction("Index", "Customer");
                }
                else if (password == loginpass && accountblocked == false && roles == "Manager")
                {
                    Session["userid"] = get_user.userid.ToString();
                    Session["Email"] = get_user.Email.ToString();
                    get_user.LastLoginDate = DateTime.Now;
                    businesobj.SaveChanges();
                    return RedirectToAction("Index", "Customer");
                }
                else if (password == loginn.password && accountblocked == false && roles == "Admin")
                {
                    Session["userid"] = get_user.userid.ToString();
                    Session["Email"] = get_user.Email.ToString();
                    get_user.LastLoginDate = DateTime.Now;
                    businesobj.SaveChanges();
                    return RedirectToAction("Index", "admin");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password or role does not match.");
                    if (get_user.Email == loginn.Email && loginn.Email != "admin@gmail.com")
                    {
                        var loginfailedcount = get_user.LoginFailedCount;
                        get_user.LoginFailedCount = loginfailedcount + 1;
                        businesobj.SaveChanges();
                        if (get_user.LoginFailedCount < 3)
                        {
                            get_user.AccountLocked = true;
                            ViewBag.accountblockmsg = "Your account will be blocked after 3 attempts";
                            LoginPage();
                        }
                        else if (get_user.LoginFailedCount > 3)
                        {
                            get_user.AccountLocked = true;
                            businesobj.SaveChanges();

                            ViewBag.accountblockmsg = "Your account has been blocked please reset your password by to unblock";
                            LoginPage();
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Email or Password or role does not match.");
                LoginPage();
            }
            return View();
        }
        public ActionResult changepassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult changepassword(Registration reg)
        {
            reg.ActivationCode = TempData["Email"].ToString();
            var empl = businesobj.registration.FirstOrDefault(x => x.ActivationCode == reg.ActivationCode && x.otp == reg.otp);

            //reg.Email = empl.Email;
            //var logintbl = businesobj.logindetails.FirstOrDefault(x => x.Email == reg.Email);
            if (empl == null)
            {
                ModelState.AddModelError("EmailNotExists", "otp is incorrect");
                return View();
            }
            else
            {
                reg.Email = empl.Email;
                var logintbl = businesobj.logindetails.FirstOrDefault(x => x.Email == reg.Email);

                var newpass = Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(reg.password)));
                empl.password = newpass;
                logintbl.password = newpass;
                businesobj.Entry(empl).State = System.Data.Entity.EntityState.Modified;
                logintbl.AccountLocked = false;
                businesobj.Entry(logintbl).State = System.Data.Entity.EntityState.Modified;
                businesobj.SaveChanges();

                ViewBag.passchanged = "Your password has been changed! Login now";
            }


            return View();
        }

        public ActionResult forgotpassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult forgotpassword(Registration pass)
        {

            var empl = businesobj.registration.FirstOrDefault(x => x.Email == pass.Email);
            if (empl == null)
            {
                ModelState.AddModelError("EmailNotExists", "This email does not exists");
                return View();
            }
            var objUsr = businesobj.registration.Where(x => x.Email == pass.Email).FirstOrDefault();

            // Genrate OTP     
            string OTP = GeneratePassword();

            objUsr.ActivationCode = Guid.NewGuid().ToString();
            objUsr.otp = OTP;
            businesobj.Entry(objUsr).State = System.Data.Entity.EntityState.Modified;
            businesobj.SaveChanges();
            ForgotPasswordEmailToUser(objUsr.Email, objUsr.ActivationCode.ToString(), objUsr.otp);
            ViewBag.otpsent = "Otp link has been sent to your email";
            return View();
        }

        public void ForgotPasswordEmailToUser(string email, string activationcode, string otp)
        {
            var GenarateUserVerificationLink = "/Home/changepassword/" + activationcode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

            var fromMail = new MailAddress("sumankore121@gmail.com", "Bank Name"); // set your email    
            var toEmail = new MailAddress(email);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            //smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("sumankore121@gmail.com", "csnombljapkgclxk");
            //smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);
            smtp.EnableSsl = true;
            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "Password Reset";
            Message.Body = "<br/> please click on the below link for password change for " + toEmail +
                           "<br/><br/><a href=" + link + ">" + link + "" + toEmail + "</a>" +
                           "<br/> Otp for password change: " + otp;
            Message.IsBodyHtml = true;
            smtp.Send(Message);

        }

        public string GeneratePassword()
        {
            string OTPLength = "4";
            string OTP = string.Empty;

            string Chars = string.Empty;
            Chars = "1,2,3,4,5,6,7,8,9,0";

            char[] seplitChar = { ',' };
            string[] arr = Chars.Split(seplitChar);
            string NewOTP = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                NewOTP += temp;
                OTP = NewOTP;
            }
            return OTP;
        }

        public ActionResult Logout()
        {
            Session["Email"] = null; //it's my session variable
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut(); //you write this when you use FormsAuthentication
            return RedirectToAction("LoginPage", "Home");

        }
    }
}