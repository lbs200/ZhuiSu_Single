using System;
using System.Collections.Generic;
using System.IO;
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
                    string filePhysicalPath = Server.MapPath("~/images/" + model.UserName + "/");
                    if (!Directory.Exists(filePhysicalPath))//判断上传文件夹是否存在，若不存在，则创建
                    {
                        Directory.CreateDirectory(filePhysicalPath);//创建文件夹
                    }
                    var cert = Request.Files["Cert"];
                    var busiLice = Request.Files["BusiLice"];
                    var orgCodeCard = Request.Files["OrgCodeCard"];
                    if (cert == null || busiLice == null || orgCodeCard == null)
                    {
                        ViewData["result"] = "图片上传错误！请尝试重新操作！";
                    }
                    else
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
                                return RedirectToAction("RegisterResult", "Home");
                            }
                        }
                        catch (Exception)
                        {
                            ViewData["result"] = "申请失败！请尝试重新操作！";
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
            if (Session["isRegister"] != null)
            {
                if ((bool)Session["isRegister"] == true)
                {
                    ViewBag.Register = true;
                    Session.Remove("isRegister");
                }
                else
                {
                    ViewBag.Register = false;
                    Session.Remove("isRegister");
                }
            }
            else
            {
                ViewBag.Register = false;
                Session.Remove("isRegister");
            }
            ViewBag.Login = true;
            ViewBag.Result = false;
            if (Session["regUserId"] != null)
            {
                var regCheckModel = Session["regUserId"] as LinqModel.RegisterCheck;
                LinqModel.RegisterCheck model = null;
                if (new DAL.RegisterCheck().CheckUser(regCheckModel.UserName, regCheckModel.UserPwd, out model))
                {
                    //通过验证
                    ViewBag.registerResult = model;
                }
                ViewBag.Login = false;
                ViewBag.Result = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult ExitRegister()
        {
            if (Session["regUserId"] != null)
            {
                Session.Remove("regUserId");
            }
            return RedirectToAction("RegisterResult","Home");
        }

        [HttpPost]
        public ActionResult RegisterResult(LinqModel.RegisterCheck model)
        {
            ViewData["err"] = "";
            ViewBag.Register = false;
            ViewBag.Login = true;
            ViewBag.Result = false;
            if (Session["regUserId"] != null)
            {
                var regCheckModel = Session["regUserId"] as LinqModel.RegisterCheck;
                if (new DAL.RegisterCheck().CheckUser(regCheckModel.UserName, regCheckModel.UserPwd, out model))
                {
                    //通过验证
                    ViewBag.registerResult = model;
                    ViewBag.Login = false;
                    ViewBag.Result = true;
                }
                else
                {
                    //用户名或密码不正确
                    ViewBag.Login = true;
                }
                ViewBag.Login = false;
                ViewBag.Result = true;
            }
            else
            {
                if (new DAL.RegisterCheck().CheckUser(model.UserName, model.UserPwd, out model))
                {
                    //通过验证
                    ViewBag.registerResult = model;
                    ViewBag.Login = false;
                    ViewBag.Result = true;
                }
                else
                {
                    //用户名或密码不正确
                    ViewBag.Login = true;
                    ViewData["err"] = "用户名或密码不正确！";
                }
            }
            return View();
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