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
    public class ManageRolesController : BaseController
    {

        // GET: ManageRoles
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)//系统管理员实施人员
            {
                return View(new DAL.Role().GetPgedList(txtSearch, pIndex, 10));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)//企业管理员
            {
                return View(new DAL.Role().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID));
                //.Where(m => m.Sup_Org == (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID)
            }
            else
            {
                return HttpNotFound();
            }
            
        }
        // GET: ManageRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinqModel.Roles roles = new DAL.Role().GetModel(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        // GET: ManageRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageRoles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleId,RoleName,Description,Jibie")] Roles roles)
        {
            if (ModelState.IsValid)
            {
                Common.Argument.RetResult ret = new DAL.Role().Create(roles);
                return RedirectToAction("Index");
            }

            return View(roles);
        }
        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles roles = new DAL.Role().GetModel(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        // POST: ManageRoles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "RoleId,RoleName,Description,Jibie")]  Roles roles)
        {
            if (ModelState.IsValid)
            {
                Common.Argument.RetResult ret = new DAL.Role().Edit(roles);
                return RedirectToAction("Index");
            }
            return View(roles);
        }

        // GET: ManageRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Roles roles = dc.Roles.FirstOrDefault(m => m.RoleId == id);
                if (roles == null)
                {
                    return HttpNotFound();
                }
                return View(roles);
            }
        }

        // POST: ManageRoles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleId,RoleName,Description,Jibie")] Roles roles)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    //dc.Entry(roles).State = EntityState.Modified;
                    dc.Roles.Attach(roles);
                    dc.SubmitChanges();
                    return RedirectToAction("Index");
                }
                return View(roles);
            }
        }

        // GET: ManageRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Roles roles =  new DAL.Role().GetModel(id);
            if (roles == null)
            {
                return HttpNotFound();
            }
            return View(roles);
        }

        // POST: ManageRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Common.Argument.RetResult ret = new DAL.Role().Del(id);
            
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
