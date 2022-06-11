using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace Bank_project.security
{
    public class Customauthenticationfilter: ActionFilterAttribute,IAuthenticationFilter
    {
        //void IAuthenticationFilter.OnAuthentication(AuthenticationContext filterContext)
        //{
        //    if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["Email"])))
        //    {
        //        filterContext.Result = new HttpUnauthorizedResult();
        //    }
        //    throw new NotImplementedException();
        //}

        //void IAuthenticationFilter.OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        //{
        //    throw new NotImplementedException();
        //}
       public void OnAuthentication(AuthenticationContext filterContext)
    {
        if (string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session["Email"])))
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }

    public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
    {
        if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
        {
            //Redirecting the user to the Login View of Account Controller  
            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary
            {
                     { "controller", "Home" },
                     { "action", "LoginPage" }
            });
        }
    }
}
}

