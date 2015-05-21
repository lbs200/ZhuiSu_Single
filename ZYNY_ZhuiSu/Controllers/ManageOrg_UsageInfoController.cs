using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageOrg_UsageInfoController : BaseController
    {
        //
        // GET: /ManageOrg_UsageInfo/
        public ActionResult Index(int? page)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            ViewBag.Org_UsageType = new DAL.Org_UsageType().GetList(orgID, "", 1, int.MaxValue);
            int typeID = 0;
            if (Request["Org_UsageType_ID"] != null)
            {
                typeID = int.Parse(Request["Org_UsageType_ID"].ToString());
            }
            ViewBag.Org_UsageTypeCurrent = typeID;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Org_UsageInfo().GetList(orgID, typeID, txtSearch, pIndex, 10));
        }
        [HttpGet]
        public ActionResult Add()
        {
            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
            Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            ViewBag.Org_UsageType = new DAL.Org_UsageType().GetList(orgID, "", 1, int.MaxValue);
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(LinqModel.Org_UsageInfo model)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            model.OrgID = orgID;
            model.Status = 0;
            model.TimeAdd = Common.Argument.Public.GetDateTimeNow();
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.Org_UsageInfo().Add(model);

            if (ret.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                ViewBag.Org_UsageType = new DAL.Org_UsageType().GetList(orgID, "", 1, int.MaxValue);
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
            Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            ViewBag.error = 0;
            return View(new DAL.Org_UsageInfo().GetModel(id));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(LinqModel.Org_UsageInfo model)
        {
            Common.Argument.RetResult ret = new DAL.Org_UsageInfo().Update(model);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(new DAL.Org_UsageInfo().GetModel(model.ID));
            }
        }
        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Org_UsageInfo().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = ("/ManageOrg_UsageInfo/Index");
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageOrg_UsageInfo/Index';</script>;");
            }
        }
        public ActionResult Info(int id)
        {
            ViewBag.ID = id;
            return View(new DAL.Org_UsageInfo().GetModel(id));
        }
    }
}