using Bank_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bank_project.security
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var emailid = Convert.ToString(httpContext.Session["Email"]);
            if (!string.IsNullOrEmpty(emailid))
                using (var context = new acccreatecontext())
                {
                    var userRole = (from u in context.registration
                                    join r in context.role on u.roleid equals r.roleid
                                    where u.Email == emailid
                                    select new
                                    {
                                        r.rolename
                                    }).FirstOrDefault();
                    foreach (var role in allowedroles)
                    {
                        if (role == userRole.rolename) return true;
                    }
                }


            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }  
    
}