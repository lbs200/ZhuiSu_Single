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
    public class GBT4754941Controller : BaseController
    {

        // GET: GBT4754941
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.GB().GetPgedList(txtSearch, pIndex, 10));
        }
        // GET: GBT4754941/Details/5
        public ActionResult Details(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GBT475494 gBT475494 = dc.GBT475494.FirstOrDefault(m => m.ID == id);
                if (gBT475494 == null)
                {
                    return HttpNotFound();
                }
                return View(gBT475494);
            }
        }

        // GET: GBT4754941/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GBT4754941/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,C1,C2,C3,C4,Name,Remark")] GBT475494 gBT475494)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    dc.GBT475494.InsertOnSubmit(gBT475494);
                    dc.SubmitChanges();
                    return RedirectToAction("Index");
                }

                return View(gBT475494);
            }
        }

        // GET: GBT4754941/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GBT475494 gBT475494 = dc.GBT475494.FirstOrDefault(m => m.ID == id);
                if (gBT475494 == null)
                {
                    return HttpNotFound();
                }
                return View(gBT475494);
            }
        }

        // POST: GBT4754941/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,C1,C2,C3,C4,Name,Remark")] GBT475494 gBT475494)
        {
            Common.Argument.RetResult ret = new DAL.GB().Update(gBT475494);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(gBT475494);
            }
        }

        // GET: GBT4754941/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GBT475494 gBT475494 = dc.GBT475494.FirstOrDefault(m => m.ID == id);
                if (gBT475494 == null)
                {
                    return HttpNotFound();
                }
                return View(gBT475494);
            }
        }

        // POST: GBT4754941/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                GBT475494 gBT475494 = dc.GBT475494.FirstOrDefault(m => m.ID == id);
                dc.GBT475494.DeleteOnSubmit(gBT475494);
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
