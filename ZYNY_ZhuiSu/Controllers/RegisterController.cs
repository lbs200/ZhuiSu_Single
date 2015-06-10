using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class RegisterController : Controller
    {

        #region 用户注册

        public ActionResult GetP(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.AdministrationRegion
                         select new
                         {
                             ID = m.ID,
                             Province = m.Province,
                             City = m.City,


                         };
                return Json(pp.ToList().Where(m => m.City.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetZIP(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.AdministrationRegion
                         select new
                         {
                             ID = m.ID,
                             Province = m.Province,
                             City = m.City,
                             ZIP = m.Postal_Code,

                         };
                return Json(pp.ToList().Where(m => m.ID.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetBM(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.Code_Scheme
                         select new
                         {
                             Scheme_ID = m.Scheme_ID,
                             Prefix = m.Prefix,
                             Separator = m.Separator,
                             Name = m.Name,

                         };
                return Json(pp.ToList().Where(m => m.Scheme_ID.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetAllP(string sheng, string shi, string qu)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.Organization
                         select new
                         {
                             Province = m.Province,
                             City = m.City,
                             District = m.District

                         };
                pp = pp.Where(m => m.Province.ToString().Contains(sheng));
                pp = pp.Where(m => m.City.ToString().Contains(shi));
                pp = pp.Where(m => m.District.ToString().Contains(qu));
                return Json(pp.ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        //注册机构
        [HttpGet]
        public ActionResult Register()
        {

            List<LinqModel.Code_Scheme> cs = new DAL.Code_Scheme().GetAll();
            ViewData["BMFA"] = new SelectList(cs, "Scheme_ID", "Name");
            List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
            ViewData["Categories"] = new SelectList(gb, "C4", "Name");
            ViewBag.selectlist = new DAL.Region().GetAllSheng();
            //string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
            //string filePhysicalPath = Server.MapPath("~/images/" + uname + "/");
            //if (!Directory.Exists(filePhysicalPath))//判断上传文件夹是否存在，若不存在，则创建
            //{
            //    Directory.CreateDirectory(filePhysicalPath);//创建文件夹
            //}
            //Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            //ViewBag.ZIP = Convert.ToInt32(new DAL.Organization().GetMaxID()) + 1;
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Register(LinqModel.RegisterCheck model)
        {
            if (!new DAL.RegisterCheck().ExistOrgName(model.OrgName))
            {
                ViewData["result"] = "机构名称已存在！";
            }
            else if (!new DAL.RegisterCheck().ExistUserName(model.UserName))
            {
                ViewData["result"] = "用户名已存在！";
            }
            else
            {
                try
                {
                   
                    var cert = Request.Files["Cert"];
                    var busiLice = Request.Files["BusiLice"];
                    var orgCodeCard = Request.Files["OrgCodeCard"];
                    if (cert == null || busiLice == null || orgCodeCard == null)
                    {
                        ViewData["result"] = "图片上传错误！请尝试重新操作！";
                    }
                    else
                    {
                        string filePhysicalPath = Server.MapPath("~/images/" + model.UserName + "/");
                        if (!Directory.Exists(filePhysicalPath))//判断上传文件夹是否存在，若不存在，则创建
                        {
                            Directory.CreateDirectory(filePhysicalPath);//创建文件夹
                        }
                        if (new DAL.RegisterCheck().CheckImgFormat(cert.FileName) && new DAL.RegisterCheck().CheckImgFormat(busiLice.FileName) && new DAL.RegisterCheck().CheckImgFormat(orgCodeCard.FileName))
                        {
                            cert.SaveAs(filePhysicalPath + cert.FileName);
                            model.Cert = "/images/" + model.UserName + "/" + cert.FileName; ;

                            busiLice.SaveAs(filePhysicalPath + busiLice.FileName);
                            model.BusiLice = "/images/" + model.UserName + "/" + busiLice.FileName;

                            orgCodeCard.SaveAs(filePhysicalPath + orgCodeCard.FileName);
                            model.OrgCodeCard = "/images/" + model.UserName + "/" + orgCodeCard.FileName;

                            try
                            {
                                model.HangYeID = new DAL.RegisterCheck().GetHangYeId(Request.Form["Category"]);
                                model.Sup_Org = 0;
                                model.CreateTime = DateTime.Now;
                                model.CheckStatus = 0;
                                var ret = new DAL.RegisterCheck().Add(model);
                                if (ret.IsSuccess)
                                {
                                    Session.Add("regUserId", model);
                                    Session.Add("isRegister", true);
                                    return RedirectToAction("RegisterResult", "Register");
                                }
                            }
                            catch (Exception)
                            {
                                ViewData["result"] = "申请失败！请尝试重新操作！";
                            }
                        }
                        else
                        {
                            ViewData["result"] = "你上传格式不正确，正确格式:.jpg、.png、.bmp、.gif";
                        }


                    }



                }
                catch (Exception)
                {
                    ViewData["result"] = "图片上传失败！请尝试重新操作！";
                }
            }
            //获取路径


            List<LinqModel.Code_Scheme> cs = new DAL.Code_Scheme().GetAll();
            ViewData["BMFA"] = new SelectList(cs, "Scheme_ID", "Name");
            List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
            ViewData["Categories"] = new SelectList(gb, "C4", "Name");
            ViewBag.selectlist = new DAL.Region().GetAllSheng();
            return View(model);


        }
        public ActionResult RegisterResult()
        {
            
            ViewBag.Result = false;
            if (Session["regModel"] != null)
            {
                
                    //通过验证
               ViewBag.registerResult = Session["regModel"] as LinqModel.RegisterCheck;
               ViewBag.Result = true;
               Session.Remove("regModel");
               return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        

       


        [HttpPost]
        public bool ExistOrgName()
        {
            var orgName = Request.Form["orgName"];
            return new DAL.RegisterCheck().ExistOrgName(orgName);
        }
        [HttpPost]
        public bool ExistUserName()
        {
            var userName = Request.Form["userName"];
            return new DAL.RegisterCheck().ExistUserName(userName);
        }
        #endregion
        public ActionResult Index()
        {
            //var m = (Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]];
            //if (m != null)
            //{
            //    ViewBag.orgName = new DAL.Organization().GetModel(m.user.Org_ID).Name;
            //}
            return View();
        }

        public ActionResult Head()
        {
            return View();
        }

        public ActionResult Left()
        {
            string controller = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();
            string action = RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString();
            StringBuilder sb = new StringBuilder();
            if (controller == "Register" && action == "Register")
            {
                sb.Append("<li class=\"start active\"><a href=\"/Register/Register\"><i class=\"icon-bar-chart\"></i><span class=\"title\">注册</span></a>");
                sb.Append("<li class=\"last\"><a href=\"/Register/RegisterResult\"><i class=\"icon-bar-chart\"></i><span class=\"title\">查看审核结果</span></a>");
            }
            else if (controller == "Register" && action == "RegisterResult")
            {
                sb.Append("<li class=\"start\"><a href=\"/Register/Register\"><i class=\"icon-bar-chart\"></i><span class=\"title\">注册</span></a>");
                sb.Append("<li class=\"last active\"><a href=\"/Register/RegisterResult\"><i class=\"icon-bar-chart\"></i><span class=\"title\">查看审核结果</span></a>");
            }
            else
            {
                sb.Append("<li class=\"start\"><a href=\"/Register/Register\"><i class=\"icon-bar-chart\"></i><span class=\"title\">注册</span></a>");
                sb.Append("<li class=\"last\"><a href=\"/Register/RegisterResult\"><i class=\"icon-bar-chart\"></i><span class=\"title\">查看审核结果</span></a>");
            }
            
            ViewBag.menu = sb.ToString();
            return View();
        }

        public ActionResult Navigate()
        {
            //string controller = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();
            //string action = RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString();
            //string permissionName = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).listPermissionUser.FirstOrDefault(m => m.Controller == controller && m.Action == action).PermissionName;
            //List<LinqModel.View_S_Tree> listSTreeUser = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).listSTreeUser;
            //var currentMenu = listSTreeUser.FirstOrDefault(m => m.Controller == controller && m.Action == action);
            ////如果功能不在菜单里，就查找菜单Index
            //if (currentMenu == null)
            //{
            //    currentMenu = listSTreeUser.FirstOrDefault(m => m.Controller == controller && m.Action == "Index");
            //    if (currentMenu == null || currentMenu.Text == "首页")
            //    { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a></i></li>"; }
            //    else
            //    { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a><i class=\"icon-angle-right\"></i></li><li><a href=\"" + currentMenu.Url + "\">" + currentMenu.Text + "</a><i class=\"icon-angle-right\"></i></li><li>" + permissionName + "</li>"; }
            //}
            //else
            //{
            //    if (currentMenu.Text == "首页")
            //    { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a></i></li>"; }
            //    else
            //    { ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Manage\">首页</a><i class=\"icon-angle-right\"></i></li><li><a href=\"" + currentMenu.Url + "\">" + currentMenu.Text + "</a></li>"; }
            //}
            string controller = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();
            string action = RouteData.Route.GetRouteData(this.HttpContext).Values["action"].ToString();
            if (controller == "Register" && action == "Register")
            {
                ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Register/Index\">菜单</a><i class=\"icon-angle-right\"></i></li><li><a href=\"/" + controller + "/" + action + "\">注册</a></li>";
            }
            else if (controller == "Register" && action == "RegisterResult")
            {
                ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Register/Index\">菜单</a><i class=\"icon-angle-right\"></i></li><li><a href=\"/" + controller + "/" + action + "\">查看审核结果</a></li>";
            }
            else
            {
                ViewBag.navigate = " <li><i class=\"icon-home\"></i><a href=\"/Register/Index\">菜单</a><i class=\"icon-angle-right\"></i></li>";
            }
            return View();
        }

        public ActionResult Result()
        {
            return View();
        }
    }
}
