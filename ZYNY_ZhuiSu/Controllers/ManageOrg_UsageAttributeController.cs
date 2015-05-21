using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageOrg_UsageAttributeController : BaseController
    {
        //
        // GET: /ManageOrg_UsageAttribute/
        public ActionResult Index(int infoID)
        {
            var modelInfo = new DAL.Org_UsageInfo().GetModel(infoID);
            ViewBag.InfoName = modelInfo.Name;
            ViewBag.InfoID = infoID;
            return View(new DAL.Org_UsageAttribute().GetList(infoID));
        }
        [HttpGet]
        public ActionResult Add(int infoID)
        {
            var modelInfo = new DAL.Org_UsageInfo().GetModel(infoID);
            ViewBag.InfoName = modelInfo.Name;
            ViewBag.InfoID = infoID;
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Org_UsageAttribute model)
        {
            ViewBag.error = 0;
            model.Status = 0;
            model.TimeAdd = Common.Argument.Public.GetDateTimeNow();
            Common.Argument.RetResult ret = new DAL.Org_UsageAttribute().Add(model);

            if (ret.IsSuccess)
            {
                return RedirectToAction("Index", new { infoID = model.Org_UsageInfo_ID });
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                var modelInfo = new DAL.Org_UsageInfo().GetModel(model.Org_UsageInfo_ID);
                ViewBag.InfoName = modelInfo.Name;
                ViewBag.InfoID = model.Org_UsageInfo_ID;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var model = new DAL.Org_UsageAttribute().GetModel(id);
            var modelInfo = new DAL.Org_UsageInfo().GetModel(model.Org_UsageInfo_ID);
            ViewBag.InfoName = modelInfo.Name;
            ViewBag.error = 0;
            return View(model);
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Org_UsageAttribute model)
        {
            Common.Argument.RetResult ret = new DAL.Org_UsageAttribute().Update(model);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index", new { infoID = model.Org_UsageInfo_ID });
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                var modelInfo = new DAL.Org_UsageInfo().GetModel(model.Org_UsageInfo_ID);
                ViewBag.InfoName = modelInfo.Name;
                return View(new DAL.Org_UsageAttribute().GetModel(model.ID));
            }
        }
        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Org_UsageAttribute().Del(id);
            return Content("<script>this.location = '/ManageOrg_UsageAttribute/Index?infoID=" + Request.QueryString["infoID"] + "';</script>;");
        }
    }
}