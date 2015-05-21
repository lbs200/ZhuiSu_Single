using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Head()
        {
            return View();
        }

        public ActionResult Left()
        {
            return View();
        }

        public ActionResult Index()
        {
            if (Request.QueryString["txtSearch"] != null && !string.IsNullOrEmpty(Request.QueryString["txtSearch"]))
            {
                string ewm = Server.UrlDecode(Request.QueryString["txtSearch"]);
                if (ewm.StartsWith(System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"]))
                {
                    Uri url = new Uri(ewm);
                    ewm = url.Query;
                }
                else
                {
                    ewm = "?ewm=" + ewm;
                }
                return Redirect(System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + ewm);
            }
            ViewBag.txtSearch = Request.QueryString["txtSearch"];
            return View();
        }

        [HttpGet]
        public ActionResult Login(int? id)
        {
            ViewBag.username = Request.QueryString["username"];
            ViewBag.password = Request.QueryString["password"];
            if (id == 0)
            {
                ViewBag.loginResult = 0;
                ViewBag.loginResultStr = "用户名或密码错误。";
            }
            else if (id == 2)
            {
                ViewBag.loginResult = 2;
                ViewBag.loginResultStr = "未知错误。";
            }
            else if (id == 3)
            {
                ViewBag.loginResult = 3;
                ViewBag.loginResultStr = "验证码输入错误。";
            }
            else if (id == -1)
            {
                //清除cookie
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.Login login)
        {
            int result = 0;
            if (Session["CheckCode"] != null && Server.HtmlEncode(Request.Form["YZCode"]) == Session["CheckCode"].ToString())
            {
                //Session存储数据
                Common.Argument.LoginInfo session = new Common.Argument.LoginInfo();
                LinqModel.User resultModel = DAL.Login.LoginMethod(login.username, login.password, out result);
                //判断登陆
                switch (result)
                {
                    case 1:
                        //添加cookie：获取菜单列表，获取用户权限列表，记录用户基本信息
                        session.user = resultModel;
                        session.listPermissionUser = new DAL.UserPermission().GetList(resultModel.Role_ID);
                        List<LinqModel.View_S_Tree> sTreeUser = new List<LinqModel.View_S_Tree>();
                        foreach (var temp in Common.Argument.Public.listSTree)
                        {
                            //判断是否有权限
                            if (session.listPermissionUser.Count(m => m.PermissionID == temp.PermissionID) > 0)
                            {
                                //如果不是一级菜单，需要先把一级菜单加进去
                                if (temp.ParentID != 0 && sTreeUser.Count(m => m.NodeID == temp.ParentID) <= 0)
                                {
                                    if (sTreeUser.Count(m => m.NodeID == temp.NodeID) <= 0)
                                        sTreeUser.Add(Common.Argument.Public.listSTree.FirstOrDefault(m => m.NodeID == temp.ParentID));
                                }
                                if (sTreeUser.Count(m => m.NodeID == temp.NodeID) <= 0)
                                    sTreeUser.Add(temp);
                            }
                        }
                        session.listSTreeUser = sTreeUser;
                        session.listFlowUser = new DAL.Flow_User().GetAllByUser(resultModel.ID);
                        Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]] = session;
                        return RedirectToAction("Index", "Manage");
                    case 2:
                        return RedirectToAction("Login", "Home", new { id = result, username = login.username, password = login.password });
                    default:
                        return RedirectToAction("Login", "Home", new { id = result, username = login.username, password = login.password });
                }
            }
            else
            {
                result = 3;
                return RedirectToAction("Login", "Home", new { id = result, username = login.username, password = login.password });
            }
        }

        public ActionResult ErrorPermission()
        {
            return View();
        }

        public ActionResult Error()
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

        public ActionResult ImgTest()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetAppVersion()
        {
            var model = new DAL.AppVersion().GetNew();
            ContentResult result = new ContentResult();
            result.Content = "<root><VersionNum>" + model.VersionNum + "</VersionNum><UrlPath>" + model.UrlPath + "</UrlPath></root>";
            return result;
        }
    }
}