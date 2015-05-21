using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LinqModel;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Security.Cryptography;
namespace ZYNY_ZhuiSu.Controllers
{
    public class CodeLogController : BaseController
    {


        // GET: Products

        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            string CategoryDDL = Request["CategoryDDL"];
            ViewBag.SelectCategory = CategoryDDL;

          
            ViewBag.txtSearch = txtSearch;
            
           
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)
            {
                ViewBag.selectlist = new DAL.Organization().GetAll();
                return View(new DAL.CodeLog().GetPgedListWithORG(txtSearch, pIndex, 10, CategoryDDL));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View(new DAL.CodeLog().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
            }
            else
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View();
            }

        }

        public ActionResult Download()
        {
            string path=Request["path"];
            var fileStream = new FileStream(Server.MapPath(path), FileMode.Open);
            var mimeType = "application/octet-stream";
            var fileDownloadName = "wendang.txt";
            return File(fileStream, mimeType, fileDownloadName);
        }
        protected override void Dispose(bool disposing)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (disposing)
                {
                    dc.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}
