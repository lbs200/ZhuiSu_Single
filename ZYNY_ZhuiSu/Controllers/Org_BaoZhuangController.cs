using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class Org_BaoZhuangController : BaseController
    {
        public ActionResult Index(int? page)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Org_BaoZhuang().GetAllByOrgID(orgID, txtSearch, pIndex, 10));
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Org_BaoZhuang model)
        {
            ViewBag.error = 0;
            model.OrgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            if(string.IsNullOrEmpty(model.Name.Trim()))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "名称输入错误！";
                return View(model);
            }
            else
            {
                Common.Argument.RetResult ret = new DAL.Org_BaoZhuang().Add(model);

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
            
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.error = 0;
            return View(new DAL.Org_BaoZhuang().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Org_BaoZhuang model)
        {
            if (string.IsNullOrEmpty(model.Name.Trim()))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "名称输入错误！";
                return View(model);
            }
            else
            {
                Common.Argument.RetResult ret = new DAL.Org_BaoZhuang().Update(model);
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
        }
        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Org_BaoZhuang().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = ("/Org_BaoZhuang/Index");
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/Org_BaoZhuang/Index';</script>;");
            }
        }
    }
}