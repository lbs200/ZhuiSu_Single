using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageCode_Seg_InfoController : BaseController
    {
        //
        // GET: /ManageCode_Seg_Info/
        public ActionResult Index(int Scheme_ID)
        {
            ViewBag.Scheme_ID = Scheme_ID;
            return View(new DAL.Code_Seg_Info().GetList(Scheme_ID));
        }
        public ActionResult Add(int Scheme_ID)
        {
            ViewBag.Scheme_ID = Scheme_ID;
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Code_Seg_Info model)
        {
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.Code_Seg_Info().Add(model);

            if (ret.IsSuccess)
            {
                return RedirectToAction("Index", new { Scheme_ID = Request["Scheme_ID"] });
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
            return View(new DAL.Code_Seg_Info().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Code_Seg_Info model)
        {
            Common.Argument.RetResult ret = new DAL.Code_Seg_Info().Update(model);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index", new { Scheme_ID = Request["Scheme_ID"] });
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(model);
            }
        }
        public ActionResult Del(int Scheme_ID, int id)
        {
            Common.Argument.RetResult ret = new DAL.Code_Seg_Info().Del(id);
            //alert('" + ret.Msg + "');
            return Content("<script>this.location = '/ManageCode_Seg_Info/Index?Scheme_ID=" + Scheme_ID + "';</script>;");
        }
    }
}