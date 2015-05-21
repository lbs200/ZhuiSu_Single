using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageEnt_Code_PoolController : BaseController
    {
        //
        // GET: /Ent_Code_Pool/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info(int id)
        {
            return View(new DAL.Ent_Code_Pool().GetModel(id));
        }
	}
}