using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageRuKu_PaiMaController : BaseController
    {
        //
        // GET: /ManageRuKu_PaiMa/
        public ActionResult Index(int? page)
        {
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            ViewBag.txtSearch = Request.QueryString["txtSearch"];
            ViewBag.timeS = Request.QueryString["timeS"];
            ViewBag.timeE = Request.QueryString["timeE"];
            if (Request.QueryString["timeS"] == null)
            {
                ViewBag.timeS = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            if (Request.QueryString["timeE"] == null)
            {
                ViewBag.timeE = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;

            return View(new DAL.RuKu_PaiMa().GetList(orgID, ViewBag.txtSearch, ViewBag.timeS, ViewBag.timeE, pIndex, 10));
        }

        public ActionResult Info(int id)
        {
            return View(new DAL.RuKu_PaiMa().GetModel(id));
        }
    }
}