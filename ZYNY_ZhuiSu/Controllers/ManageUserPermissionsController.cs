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
    public class ManageUserPermissionsController : BaseController
    {
        
       
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
        public ActionResult GetAllP(string rid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.UserPermission
                         select new
                         {
                             PermissionID = m.PermissionID,
                             RoleId = m.RoleId,

                         };
                return Json(pp.ToList().Where(m => m.RoleId.ToString().Equals(rid)), JsonRequestBehavior.AllowGet);
            }
        }
        // GET: ManageUserPermissions/Create
        public ActionResult Create(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                string categiryid = Request.QueryString["CategoryDDL"];

                ViewBag.SelectCategory = categiryid;
                ViewBag.CID = cid;
                ViewBag.selectlist = new DAL.Category().GetAll();
                ViewBag.PP = new DAL.Permission().GetAllCategory(categiryid);
                ViewBag.PermissionID = new SelectList(dc.Permission, "PermissionID", "PermissionName");
                ViewBag.RoleId = new SelectList(dc.Roles, "RoleId", "RoleName");
                return View();
            }
        }

        // POST: ManageUserPermissions/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PermissionID,RoleId")] UserPermission userPermission)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                string txtSearch = Request["txtSearch"];
                string categiryid = Request["CategoryDDL"];
                string pidsz = Request["checkvalues"];
                ViewBag.CVL = pidsz;
                ViewBag.SelectCategory = categiryid;
                ViewBag.txtSearch = txtSearch;
                ViewBag.selectlist = new DAL.Category().GetAll();
                ViewBag.PP = new DAL.Permission().GetAllCategory(categiryid);
                string[] ss = pidsz.Substring(0, pidsz.Length - 1).Split('*');
                if (ModelState.IsValid)
                {

                    dc.ExecuteCommand("DELETE FROM UserPermission WHERE RoleId = '" + userPermission.RoleId + "'");
                    foreach (string s in ss)
                    {
                        LinqModel.UserPermission pp = new UserPermission();
                        pp.RoleId = userPermission.RoleId;
                        pp.PermissionID = Convert.ToInt32(s);

                        dc.UserPermission.InsertOnSubmit(pp);
                        dc.SubmitChanges();
                    }

                    Common.Argument.RetResult ret = new Common.Argument.RetResult();
                    ret.Msg = "添加成功";
                    ViewBag.error = 1;
                    ViewBag.errorMsg = ret.Msg;


                    return RedirectToAction("Create");
                }

                ViewBag.PermissionID = new SelectList(dc.Permission, "PermissionID", "PermissionName", userPermission.PermissionID);
                ViewBag.RoleId = new SelectList(dc.Roles, "RoleId", "RoleName", userPermission.RoleId);
                return View(userPermission);
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
