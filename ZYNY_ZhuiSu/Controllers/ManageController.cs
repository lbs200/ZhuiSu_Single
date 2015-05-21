using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageController : BaseController
    {
        //
        // GET: /Manage/
        public ActionResult Index()
        {
            var m = (Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]];
            if (m != null)
            {
                ViewBag.orgName = new DAL.Organization().GetModel(m.user.Org_ID).Name;
            }
            return View();
        }

        public ActionResult Head()
        {
            var model = (Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]];
            ViewBag.loginname = model.user.UserName;
            ViewBag.userPhoto = model.user.Photo;
            return View();
        }

        public ActionResult Left()
        {
            ViewBag.controller = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"];
            ViewBag.action = RouteData.Route.GetRouteData(this.HttpContext).Values["action"];

            List<LinqModel.View_S_Tree> sTreeUser = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).listSTreeUser;
            LinqModel.View_S_Tree modelCurrent = sTreeUser.FirstOrDefault(m => m.Controller == ViewBag.controller && m.Action == ViewBag.action);
            //如果功能不在菜单里，就查找菜单Index
            if (modelCurrent == null)
            {
                modelCurrent = sTreeUser.FirstOrDefault(m => m.Controller == ViewBag.controller && m.Action == "Index");
            }
            //如果还是找不到菜单，那就显示首页菜单项
            if (modelCurrent == null)
            {
                modelCurrent = sTreeUser.FirstOrDefault(m => m.Controller == "Manage" && m.Action == "Index");
            }
            StringBuilder sb = new StringBuilder();
            int count = sTreeUser.Count(m => m.ParentID == 0);
            int n = 0;
            foreach (var temp1 in sTreeUser.Where(m => m.ParentID == 0))
            {
                n++;

                if (sTreeUser.Count(m => m.ParentID == temp1.NodeID) > 0)
                {
                    if (temp1.NodeID == modelCurrent.ParentID)
                    {
                        if (n == 1)
                        { sb.Append("<li class=\"start active\"><a href=\"javascript:;\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"arrow\"></span></a>"); }
                        else if (n == count)
                        { sb.Append("<li class=\"last active\"><a href=\"javascript:;\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"arrow\"></span></a>"); }
                        else
                        { sb.Append("<li class=\"active\"><a href=\"javascript:;\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"arrow\"></span></a>"); }
                        //添加二级菜单
                        sb.Append(" <ul class=\"sub-menu\">");
                        foreach (var temp2 in sTreeUser.Where(m => m.ParentID == temp1.NodeID))
                        {
                            if (temp2.NodeID == modelCurrent.NodeID)
                            {
                                sb.Append("<li class=\"active\"><a href=\"" + temp2.Url + "\">" + temp2.Text + "</a></li>");
                            }
                            else
                            {
                                sb.Append("<li><a href=\"" + temp2.Url + "\">" + temp2.Text + "</a></li>");
                            }
                        }
                        sb.Append("</ul>");
                    }
                    else
                    {
                        if (n == 1)
                        { sb.Append("<li class=\"start\"><a href=\"javascript:;\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"arrow\"></span></a>"); }
                        else if (n == count)
                        { sb.Append("<li class=\"last\"><a href=\"javascript:;\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"arrow\"></span></a>"); }
                        else
                        { sb.Append("<li class=\"\"><a href=\"javascript:;\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"arrow\"></span></a>"); }
                        //添加二级菜单
                        sb.Append(" <ul class=\"sub-menu\">");
                        foreach (var temp2 in sTreeUser.Where(m => m.ParentID == temp1.NodeID))
                        {
                            sb.Append("<li><a href=\"" + temp2.Url + "\">" + temp2.Text + "</a></li>");
                        }
                        sb.Append("</ul>");
                    }
                    sb.Append("</li>");
                }
                else
                {
                    if (temp1.NodeID == modelCurrent.NodeID)
                    {
                        if (n == 1)
                        { sb.Append("<li class=\"start active\"><a href=\"" + temp1.Url + "\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"selected\"></span></a></li>"); }
                        else if (n == count)
                        { sb.Append("<li class=\"last active\"><a href=\"" + temp1.Url + "\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"selected\"></span></a></li>"); }
                        else
                        { sb.Append("<li class=\"active\"><a href=\"" + temp1.Url + "\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"selected\"></span></a></li>"); }
                    }
                    else
                    {
                        if (n == 1)
                        { sb.Append("<li class=\"start\"><a href=\"" + temp1.Url + "\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"selected\"></span></a></li>"); }
                        else if (n == count)
                        { sb.Append("<li class=\"last\"><a href=\"" + temp1.Url + "\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"selected\"></span></a></li>"); }
                        else
                        { sb.Append("<li class=\"\"><a href=\"" + temp1.Url + "\"><i class=\"" + temp1.ImageUrl + "\"></i><span class=\"title\">" + temp1.Text + "</span><span class=\"selected\"></span></a></li>"); }
                    }
                }
            }
            ViewBag.menu = sb.ToString();
            return View();
        }

        public ActionResult Navigate()
        {
            string controller = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();
            string action = RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString();
            string permissionName = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).listPermissionUser.FirstOrDefault(m => m.Controller == controller && m.Action == action).PermissionName;
            List<LinqModel.View_S_Tree> listSTreeUser = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).listSTreeUser;
            var currentMenu = listSTreeUser.FirstOrDefault(m => m.Controller == controller && m.Action == action);
            //如果功能不在菜单里，就查找菜单Index
            if (currentMenu == null)
            {
                currentMenu = listSTreeUser.FirstOrDefault(m => m.Controller == controller && m.Action == "Index");
                if (currentMenu == null || currentMenu.Text == "首页")
                { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a></i></li>"; }
                else
                { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a><i class=\"icon-angle-right\"></i></li><li><a href=\"" + currentMenu.Url + "\">" + currentMenu.Text + "</a><i class=\"icon-angle-right\"></i></li><li>" + permissionName + "</li>"; }
            }
            else
            {
                if (currentMenu.Text == "首页")
                { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a></i></li>"; }
                else
                { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a><i class=\"icon-angle-right\"></i></li><li><a href=\"" + currentMenu.Url + "\">" + currentMenu.Text + "</a></li>"; }
            }
            return View();
        }
    }
}