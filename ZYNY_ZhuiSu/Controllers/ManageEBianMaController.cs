using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageEBianMaController : BaseController
    {
        //
        // GET: /ManageEBianMa/
        public ActionResult Index()
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;

            int page = Request.QueryString["page"] == null ? 1 : int.Parse(Request.QueryString["page"]);
            List<LinqModel.User> list = new List<LinqModel.User>();

            for (int i = 1; i < 1000; i++)
            {
                LinqModel.User model = new LinqModel.User();
                model.ID = i;
                model.UserName = i.ToString();
                list.Add(model);
            }
            //list = list.Where(m => m.ID > int.Parse(txtSearch)).ToPagedList(page, 10);
            return View(list.Where(m => m.ID > int.Parse(string.IsNullOrEmpty(txtSearch) ? "0" : txtSearch)).ToPagedList(page, 10));
        }

        public ActionResult Test()
        {
            return View();
        }
    }
}