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
    public class AdministrationRegionsController : BaseController
    {

        // GET: AdministrationRegions
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Region().GetPgedList(txtSearch, pIndex, 10));
        }

        // GET: AdministrationRegions/Details/5
        public ActionResult Details(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AdministrationRegion administrationRegion = dc.AdministrationRegion.FirstOrDefault(m => m.ID == id);
                if (administrationRegion == null)
                {
                    return HttpNotFound();
                }
                return View(administrationRegion);
            }
        }

        // GET: AdministrationRegions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdministrationRegions/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Province,City,District,Postal_Code")] AdministrationRegion administrationRegion)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (Request["City"] == "0")
                    {
                        administrationRegion.District = "0";
                    }
                    else if (Request["City2"] == "0")
                    {
                        administrationRegion.District = Request["City"];
                    }
                    else
                    {
                        administrationRegion.City = Request["City2"];
                        administrationRegion.District = Request["City"] + "," + Request["City2"];
                    }
                    dc.AdministrationRegion.InsertOnSubmit(administrationRegion);
                    dc.SubmitChanges();
                    return RedirectToAction("Index");
                }

                return View(administrationRegion);
            }
        }

        // GET: AdministrationRegions/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AdministrationRegion administrationRegion = dc.AdministrationRegion.FirstOrDefault(m => m.ID == id);
                if (administrationRegion == null)
                {
                    return HttpNotFound();
                }
                return View(administrationRegion);
            }
        }

        // POST: AdministrationRegions/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Province,City,District,Postal_Code")] AdministrationRegion administrationRegion)
        {
            if (Request["City"] == "0")
            {
                administrationRegion.District = "0";
            }
            else if (Request["City2"] == "0")
            {
                administrationRegion.District = Request["City"];
            }
            else
            {
                administrationRegion.City = Request["City2"];
                administrationRegion.District = Request["City"] + "," + Request["City2"];
            }
            Common.Argument.RetResult ret = new DAL.Region().Update(administrationRegion);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(administrationRegion);
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            ViewBag.error = 0;
            return View(new DAL.Region().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.AdministrationRegion model)
        {
            Common.Argument.RetResult ret = new DAL.Region().Update(model);
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

        // GET: AdministrationRegions/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                AdministrationRegion administrationRegion = dc.AdministrationRegion.FirstOrDefault(m => m.ID == id);
                if (administrationRegion == null)
                {
                    return HttpNotFound();
                }
                return View(administrationRegion);
            }
        }

        // POST: AdministrationRegions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                AdministrationRegion administrationRegion = dc.AdministrationRegion.FirstOrDefault(m => m.ID == id);
                dc.AdministrationRegion.DeleteOnSubmit(administrationRegion);
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
