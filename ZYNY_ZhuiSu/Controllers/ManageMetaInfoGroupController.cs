using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageMetaInfoGroupController :BaseController
    {
        // GET: ManageMetaInfoGroup
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.MetaInfoGroup().GetList(txtSearch, pIndex, 10));
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.MetaInfoGroup model)
        {
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.MetaInfoGroup().Add(model);

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
            return View(new DAL.MetaInfoGroup().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.MetaInfoGroup model)
        {
            Common.Argument.RetResult ret = new DAL.MetaInfoGroup().Update(model);
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
            Common.Argument.RetResult ret = new DAL.MetaInfoGroup().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = ("/ManageMetaInfoGroup/Index");
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageMetaInfoGroup/Index';</script>;");
            }
        }
    }
}