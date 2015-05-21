using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class Org_BaoZhuang_GuiGeController : BaseController
    {
        public ActionResult Index(int bzID)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            ViewBag.bzID = bzID;
            var modelBZ = new DAL.Org_BaoZhuang().GetModel(bzID);
            ViewBag.bzName = modelBZ.Name;
            return View(new DAL.Org_BaoZhuang_GuiGe().GetAllByOrgID(orgID, bzID, txtSearch, 1, int.MaxValue));
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.bzID = Request.QueryString["bzID"];
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Org_BaoZhuang_GuiGe model)
        {
            ViewBag.error = 0;
            model.OrgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            if (string.IsNullOrEmpty(model.NameGuiGe.Trim()))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "名称输入错误！";
                return View(model);
            }
            else
            {
                Common.Argument.RetResult ret = new DAL.Org_BaoZhuang_GuiGe().Add(model);

                if (ret.IsSuccess)
                {
                    return RedirectToAction("Index", new { bzID = model.Org_BaoZhuang_ID });
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
            return View(new DAL.Org_BaoZhuang_GuiGe().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Org_BaoZhuang_GuiGe model)
        {
            if (string.IsNullOrEmpty(model.NameGuiGe.Trim()))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "名称输入错误！";
                return View(model);
            }
            else
            {
                Common.Argument.RetResult ret = new DAL.Org_BaoZhuang_GuiGe().Update(model);
                if (ret.IsSuccess)
                {
                    return RedirectToAction("Index", new { bzID = model.Org_BaoZhuang_ID });
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
            Common.Argument.RetResult ret = new DAL.Org_BaoZhuang_GuiGe().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = ("/Org_BaoZhuang_GuiGe/Index?bzID=" + Request.QueryString["bzID"]);
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/Org_BaoZhuang_GuiGe/Index?bzID=" + Request.QueryString["bzID"] + "';</script>;");
            }
        }
    }
}