using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MooshakPP.Utilities;

namespace MooshakPP.Handlers
{
    public class CustomHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            Exception ex = filterContext.Exception;

            Logger.Instance.LogException(ex);

            string viewName = "Error";

            string currentController = (string)filterContext.RouteData.Values["controller"];
            string currentActionName = (string)filterContext.RouteData.Values["action"];

            HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, currentController, currentActionName);
            ViewResult result = new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                TempData = filterContext.Controller.TempData
            };

            filterContext.Result = result;
            filterContext.ExceptionHandled = true;

            base.OnException(filterContext);
        }
    }
}