using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LinqModel;
using PagedList;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageORG_BYController : BaseController
    {

        // GET: ManageCategories
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)//系统管理员
            {
                return View(new DAL.ORG_BY().GetPgedList(txtSearch, pIndex, 10));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)//企业管理员
            {
                return View(new DAL.ORG_BY().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
                //.Where(m => m.Sup_Org == (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID)
            }
            else
            {
                return HttpNotFound();
            }


        }

        // GET: ManageCategories/Details/5
        public ActionResult Details(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ORG_BY ORG_BY = dc.ORG_BY.FirstOrDefault(m => m.ID == id);
                if (ORG_BY == null)
                {
                    return HttpNotFound();
                }
                return View(ORG_BY);
            }
        }

        // GET: ManageCategories/Create
        public ActionResult Create()
        {
            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
            Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            return View();
        }

        // POST: ManageCategories/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Org_ID,BYContent,Logo,LCSM")] ORG_BY ORG_BY)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (ORG_BY.Logo == null)
                    {
                        ViewData.ModelState.AddModelError("Logo", "*LOGO不能为空");

                    }
                    else if (ORG_BY.BYContent == null)
                    {
                        ViewData.ModelState.AddModelError("BYContent", "*请填写标语");
                    }
                    else if (ORG_BY.Org_ID == null || ORG_BY.Org_ID == -1)
                    {
                        ViewData.ModelState.AddModelError("Org_ID", "*请选择机构");
                    }
                    else if (ORG_BY.LCSM == null)
                    {
                        ViewData.ModelState.AddModelError("LCSM", "*请填写流程说明文字");
                    }
                    else
                    {
                        dc.ORG_BY.InsertOnSubmit(ORG_BY);
                        dc.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }

                return View(ORG_BY);
            }
        }

        // GET: ManageCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
                Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
                if (id == null)
                {

                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ORG_BY ORG_BY = dc.ORG_BY.FirstOrDefault(m => m.ID == id);
                if (ORG_BY == null)
                {
                    return HttpNotFound();
                }
                return View(ORG_BY);
            }
        }

        // POST: ManageCategories/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Org_ID,BYContent,Logo,LCSM")] ORG_BY model)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (model.Logo == null)
                    {
                        ViewData.ModelState.AddModelError("Logo", "*LOGO不能为空");

                    }
                    else if (model.BYContent == null)
                    {
                        ViewData.ModelState.AddModelError("BYContent", "*请填写标语");
                    }
                    else if (model.Org_ID == null || model.Org_ID == -1)
                    {
                        ViewData.ModelState.AddModelError("Org_ID", "*请选择机构");
                    }
                    else if (model.LCSM == null)
                     {
                         ViewData.ModelState.AddModelError("LCSM", "*请填写流程说明文字");
                     }
                     else
                     {
                         var temp = dc.ORG_BY.FirstOrDefault(m => m.ID == model.ID);
                         temp.BYContent = model.BYContent;
                         temp.Logo = model.Logo;
                         temp.Org_ID = model.Org_ID;
                         temp.LCSM = model.LCSM;

                         dc.SubmitChanges();
                         return RedirectToAction("Index");
                     }
                }
                return View(model);
            }
        }

        // GET: ManageCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ORG_BY ORG_BY = dc.ORG_BY.FirstOrDefault(m => m.ID == id);
                if (ORG_BY == null)
                {
                    return HttpNotFound();
                }
                return View(ORG_BY);
            }
        }

        // POST: ManageCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                ORG_BY ORG_BY = dc.ORG_BY.FirstOrDefault(m => m.ID == id);
                dc.ORG_BY.DeleteOnSubmit(ORG_BY);
                dc.SubmitChanges();
                return RedirectToAction("Index");
            }
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
