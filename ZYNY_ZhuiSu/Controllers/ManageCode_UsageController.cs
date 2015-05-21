using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageCode_UsageController : BaseController
    {
        //
        // GET: /ManageCode_Scheme/
        public ActionResult Index()
        {
            return View(new DAL.Code_Usage().GetAll());
        }
        public ActionResult Add()
        {
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Code_Usage model)
        {
            ViewBag.error = 0;
            Common.Argument.RetResult ret = new DAL.Code_Usage().Add(model);

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