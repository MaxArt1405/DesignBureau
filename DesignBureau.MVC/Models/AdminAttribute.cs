using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DesignBureau.MVC.Models
{
    public class AdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var current = UserSession.Current;
            if (current == null || !current.IsAdmin)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Auth/Login");
                }
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }

}