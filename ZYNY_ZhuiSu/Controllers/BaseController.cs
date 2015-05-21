using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZYNY_ZhuiSu.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            string filePath = string.Format(@"{0}errorLog\{1}.txt", AppDomain.CurrentDomain.BaseDirectory, Common.Argument.Public.GetDateTimeNow().ToString("yyyyMMddHHmmssfff"));
            using (StreamWriter sw = System.IO.File.AppendText(filePath))
            {
                sw.Write(string.Format("{0}\r\n访问页面：{1}\r\n\r\n", filterContext.Exception.Message, HttpContext.Request.Url.PathAndQuery));
            }
            //跳转到错误页面
            #region
            int rCode = new HttpException(null, filterContext.Exception).GetHttpCode();//获取错误码
            if (filterContext.ExceptionHandled == true)
            {
                if (rCode != 500)//为什么要特别强调500 因为MVC处理HttpException的时候，如果为500 则会自动
                //将其ExceptionHandled设置为true，那么我们就无法捕获异常
                {
                    filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { Controller = "Home", Action = "Error" })); 
                }
            }
            if (rCode == 500)
            {
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { Controller = "Home", Action = "Error" })); 
            }
            #endregion
            //
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerMine = RouteData.Values["controller"].ToString();
            string actionMine = RouteData.Values["action"].ToString();
            if (controllerMine != "Manage" || (controllerMine == "Manage" && actionMine != "Head" && actionMine != "Left" && actionMine != "Navigate"))
            {
                string controller = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();
                string action = RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString();
                try
                {
                    Common.Argument.LoginInfo session = (Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]];
                    if (session.listPermissionUser.Count(m => m.Controller == controller && m.Action == action) <= 0)
                    {
                        filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { Controller = "Home", Action = "ErrorPermission" }));
                    }
                }
                catch
                {
                    filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { Controller = "Home", Action = "ErrorPermission" }));
                }
            }
        }
    }
}