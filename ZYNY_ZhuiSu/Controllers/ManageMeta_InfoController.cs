using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageMeta_InfoController : BaseController
    {
        //
        // GET: /Meta_Info/
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Meta_Info().GetList(txtSearch, pIndex, int.MaxValue));
        }
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.listDataType = Common.Argument.Public.listDataType;
            ViewBag.codeScheme = new DAL.Ent_Code_Scheme().GetList(1, 1, int.MaxValue);
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Meta_Info model)
        {
            ViewBag.listDataType = Common.Argument.Public.listDataType;
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.Meta_Info().Add(model);

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
            ViewBag.listDataType = Common.Argument.Public.listDataType;
            ViewBag.error = 0;
            return View(new DAL.Meta_Info().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Meta_Info model)
        {
            ViewBag.listDataType = Common.Argument.Public.listDataType;
            Common.Argument.RetResult ret = new DAL.Meta_Info().Update(model);
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
            Common.Argument.RetResult ret = new DAL.Meta_Info().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = ("/ManageMeta_Info/Index");
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageMeta_Info/Index';</script>;");
            }
        }
    }
}