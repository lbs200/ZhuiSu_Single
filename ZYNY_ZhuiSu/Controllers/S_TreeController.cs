using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LinqModel;

namespace ZYNY_ZhuiSu.Controllers
{
    public class S_TreeController : BaseController
    {

        // GET: S_Tree
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.S_Tree().GetPgedList(txtSearch, pIndex, 10));
        }

        // GET: S_Tree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            S_Tree s_Tree =new DAL.S_Tree().GetModel(id);
            if (s_Tree == null)
            {
                return HttpNotFound();
            }
            return View(s_Tree);
        }
        public ActionResult GetP(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.Permission
                         select new
                         {
                             PermissionID = m.PermissionID,
                             PermissionName = m.PermissionName,
                             CategoryID = m.CategoryID
                         };
                return Json(pp.ToList().Where(m => m.CategoryID.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
       
       
        // GET: S_Tree/Create
        public ActionResult Create()
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                string categiryid = Request["CategoryDDL"];

                ViewBag.SelectCategory = categiryid;


                ViewBag.PP = new DAL.Permission().GetAllCategory(categiryid);
                ViewBag.selectlist = new DAL.Category().GetAll();

                ViewBag.PermissionID = new SelectList(dc.Permission, "PermissionID", "PermissionName");
                return View();
            }
        }

        // POST: S_Tree/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NodeID,Text,ParentID,ParentPath,Url,comment,PermissionID,ImageUrl")] S_Tree s_Tree)
        {

            using (var dc = DAL.DALBase.GetDataContext())
            {

                if (ModelState.IsValid)
                {
                    Common.Argument.RetResult ret = new DAL.S_Tree().Create(s_Tree);
                    return RedirectToAction("Index");
                }

                ViewBag.PermissionID = new SelectList(dc.Permission, "PermissionID", "PermissionName", s_Tree.PermissionID);
                return View(s_Tree);
            }
        }

        // GET: S_Tree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            S_Tree s_Tree = new DAL.S_Tree().GetModel(id);
            if (s_Tree == null)
            {
                return HttpNotFound();
            }
           // ViewBag.PermissionID = new SelectList(dc.Permission, "PermissionID", "PermissionName", s_Tree.PermissionID);
            return View(s_Tree);
        }

        // POST: S_Tree/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NodeID,Text,ParentID,ParentPath,Url,comment,PermissionID,ImageUrl")] LinqModel.S_Tree s_Tree)
        {
            if (ModelState.IsValid)
            {
                Common.Argument.RetResult ret = new DAL.S_Tree().Edit(s_Tree);
                return RedirectToAction("Index");
            }
            //ViewBag.PermissionID = new SelectList(dc.Permission, "PermissionID", "PermissionName", s_Tree.PermissionID);
            return View(s_Tree);
        }

        // GET: S_Tree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            S_Tree s_Tree = new DAL.S_Tree().GetModel(id);
            if (s_Tree == null)
            {
                return HttpNotFound();
            }
            return View(s_Tree);
        }

        // POST: S_Tree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Common.Argument.RetResult ret = new DAL.S_Tree().Del(id);
            return RedirectToAction("Index");
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
