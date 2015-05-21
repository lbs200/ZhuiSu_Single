using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManagePermissionController : BaseController
    {
        //
        // GET: /Permission/
        public ActionResult Index(int? page)
        {
            string txtSearch = Request.QueryString["txtSearch"];
            string categiryid = Request.QueryString["CategoryDDL"];
            
            ViewBag.SelectCategory = categiryid;
            ViewBag.txtSearch = txtSearch;
            ViewBag.selectlist = new DAL.Category().GetAll();
           
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            
            return View(new DAL.Permission().GetListBD(categiryid,txtSearch, pIndex, 10));
        }
        //[HttpPost]
        //public ActionResult Index()
        //{
        //    string txtSearch = Request["txtSearch"];
        //    string categiryid = Request["CategoryDDL"];
        //    ViewBag.SelectCategory = categiryid;
        //    ViewBag.txtSearch = txtSearch;
        //    ViewBag.selectlist = new DAL.Category().GetAll();
           
        //    return View(new DAL.Permission().GetListBD(categiryid, txtSearch, 1, 10));
        //}
        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.error = 0;
            return View(new DAL.Permission().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Permission model)
        {
            Common.Argument.RetResult ret = new DAL.Permission().Update(model);
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
            Common.Argument.RetResult ret = new DAL.Permission().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = ("/ManagePermission/Index");
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManagePermission/Index';</script>;");
            }
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Permission model)
        {
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.Permission().Add(model);

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
}