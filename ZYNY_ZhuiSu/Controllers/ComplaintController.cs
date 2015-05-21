using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ComplaintController : BaseController
    {
        // GET: Complaint
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
            var products = new DAL.Products().GetAllwithORGID(orgID);
            ViewBag.products = products;
            List<int> listProdIDs = new List<int>();
            if (Request.QueryString["products"] == null || Request.QueryString["products"] == "0")
            {
                listProdIDs = (from m in products select m.ID).ToList();
                ViewBag.prodC = "0";
            }
            else
            {
                listProdIDs.Add(int.Parse(Request.QueryString["products"]));
                ViewBag.prodC = Request.QueryString["products"];
            }
            return View(new DAL.Complaint().GetList(listProdIDs, ViewBag.timeS, ViewBag.timeE, ViewBag.txtSearch, pIndex, 10));
        }
    }
}