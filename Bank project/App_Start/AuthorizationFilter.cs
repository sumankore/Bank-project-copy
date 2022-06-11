using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using IAuthorizationFilter = System.Web.Mvc.IAuthorizationFilter;

namespace Bank_project.App_Start
{
    public class AuthorizationFilter : AuthorizeAttribute /*IAuthorizationFilter*/
    {
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{

        //    if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
        //       || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
        //    {
        //        // Don't check for authorization as AllowAnonymous filter is applied to the action or controller  
        //        return;
        //    }

        //    // Check for authorization  
        //    if (HttpContext.Current.Session["Email"] == null)
        //    {
        //        filterContext.Result = new RedirectResult("~/Home/Index");

        //    }
        //}


        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    HttpContext ctx = HttpContext.Current;
        //    // check if session is supported  
        //    if (ctx.Session != null)
        //    {
        //        // check if a new session id was generated  
        //        if (ctx.Session["Email"] == null || ctx.Session.IsNewSession)
        //        {
        //            //Check is Ajax request  
        //            if (filterContext.HttpContext.Request.IsAjaxRequest())
        //            {
        //                filterContext.HttpContext.Response.ClearContent();
        //                filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
        //            }
        //            // check if a new session id was generated  
        //            else
        //            {
        //                filterContext.Result = new RedirectResult("~/Account/Login");
        //            }
        //        }
        //    }
        //    base.HandleUnauthorizedRequest(filterContext);
        //}
    }
}