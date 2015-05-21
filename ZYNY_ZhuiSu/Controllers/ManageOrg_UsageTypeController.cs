using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageOrg_UsageTypeController : BaseController
    {
        //
        // GET: /ManageOrg_UsageType/
        public ActionResult Index(int? page)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Org_UsageType().GetList(orgID, txtSearch, pIndex, 10));
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Org_UsageType model)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            model.OrgID = orgID;
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.Org_UsageType().Add(model);

            if (ret.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.error = 0;
            return View(new DAL.Org_UsageType().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Org_UsageType model)
        {
            Common.Argument.RetResult ret = new DAL.Org_UsageType().Update(model);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(model);
            }
        }
        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Org_UsageType().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = ("/ManageOrg_UsageType/Index");
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageOrg_UsageType/Index';</script>;");
            }
        }
    }
}