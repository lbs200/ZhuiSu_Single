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
    public class ManageCategoriesController : BaseController
    {

        // GET: ManageCategories
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Category().GetPgedList(txtSearch, pIndex, 10));
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
                Category category = dc.Category.FirstOrDefault(m => m.CategoryID == id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
        }

        // GET: ManageCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageCategories/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (new DAL.Category().Exist(category.CategoryName))
                    {
                        ModelState.AddModelError("CategoryName", "名称已经被占用");
                    }
                    else
                    {
                        dc.Category.InsertOnSubmit(category);
                        dc.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }

                return View(category);
            }
        }

        // GET: ManageCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Category category = dc.Category.FirstOrDefault(m => m.CategoryID == id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
        }

        // POST: ManageCategories/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {

                Common.Argument.RetResult ret = new DAL.Category().Edit(category);

                
                return RedirectToAction("Index");

            }
            return View(category);
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
                Category category = dc.Category.FirstOrDefault(m => m.CategoryID == id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
        }

        // POST: ManageCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                Category category = dc.Category.FirstOrDefault(m => m.CategoryID == id);
                dc.Category.DeleteOnSubmit(category);
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
