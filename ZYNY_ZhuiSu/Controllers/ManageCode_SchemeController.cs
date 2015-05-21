using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageCode_SchemeController : BaseController
    {
        //
        // GET: /ManageCode_Scheme/
        public ActionResult Index()
        {
            return View(new DAL.Code_Scheme().GetAll());
        }
        public ActionResult Add()
        {
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Code_Scheme model)
        {
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.Code_Scheme().Add(model);

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