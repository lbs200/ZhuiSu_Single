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
    public class Prod_categoryController : BaseController
    {

        // GET: Prod_category
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            int sub = 0;
            if (Request["sub"] != null)
            {
                int.TryParse(Request["sub"], out sub);
                ViewBag.sub = Request["sub"];
            }
            return View(new DAL.Pro_Category().GetPgedList(sub, txtSearch, pIndex, 10));
        }

        // GET: Prod_category/Details/5
        public ActionResult Details(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Prod_category prod_category = dc.Prod_category.FirstOrDefault(m => m.ID == id);
                if (prod_category == null)
                {
                    return HttpNotFound();
                }
                return View(prod_category);
            }
        }

        // GET: Prod_category/Create
        public ActionResult Create()
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                ViewBag.ID = new SelectList(dc.Products, "ID", "Name");
                return View();
            }
        }

        // POST: Prod_category/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,Sup_Category")] Prod_category prod_category)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    dc.Prod_category.InsertOnSubmit(prod_category);
                    dc.SubmitChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.ID = new SelectList(dc.Products, "ID", "Name", prod_category.ID);
                return View(prod_category);
            }
        }

        // GET: Prod_category/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Prod_category prod_category = dc.Prod_category.FirstOrDefault(m => m.ID == id);
                if (prod_category == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ID = id.ToString();
                return View(prod_category);
            }
        }

        // POST: Prod_category/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,Sup_Category")] Prod_category prod_category)
        {
            Common.Argument.RetResult ret = new DAL.Pro_Category().Update(prod_category);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(prod_category);
            }
        }

        // GET: Prod_category/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Prod_category prod_category = dc.Prod_category.FirstOrDefault(m => m.ID == id);
                if (prod_category == null)
                {
                    return HttpNotFound();
                }
                return View(prod_category);
            }
        }

        // POST: Prod_category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                Prod_category prod_category = dc.Prod_category.FirstOrDefault(m => m.ID == id);
                dc.Prod_category.DeleteOnSubmit(prod_category);
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
