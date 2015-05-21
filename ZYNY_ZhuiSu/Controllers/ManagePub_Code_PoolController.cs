using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManagePub_Code_PoolController : BaseController
    {
        //
        // GET: /ManagePub_Code_Pool/
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Pub_Code_Pool().GetList(txtSearch, pIndex, 10));
        }
    }
}