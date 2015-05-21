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
    public class ManageComContentController : BaseController
    {

        // GET: ManageCategories
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            string CategoryDDL = Request["CategoryDDL"];
            ViewBag.SelectCategory = CategoryDDL;
            
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)//系统管理员
            {
                ViewBag.selectlist = new DAL.Organization().GetAll();
                return View(new DAL.ComContent().GetPgedListWithORG(txtSearch, pIndex, 10, CategoryDDL));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)//企业管理员
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View(new DAL.ComContent().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
                //.Where(m => m.Sup_Org == (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID)
            }
            else
            {
                return View();
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
                ComContent ComContent = dc.ComContent.FirstOrDefault(m => m.ID == id);
                if (ComContent == null)
                {
                    return HttpNotFound();
                }
                return View(ComContent);
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
        public ActionResult Create([Bind(Include = "ID,Title,Contents,CategoryID,Assessment,Keywords,Remark,Org_ID")] ComContent ComContent)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (ComContent.Title == null)
                    {
                        ViewData.ModelState.AddModelError("Title", "*标题不能为空");
                        
                    }
                    else if (Request["CategoryID"] == "*")
                    {
                        ViewData.ModelState.AddModelError("CategoryID", "*请选择分类");
                    }
                    else if (Request["Org_ID"] == "*")
                    {
                        ViewData.ModelState.AddModelError("Org_ID", "*请选择机构");
                    }
                    else if (ComContent.Remark == null)
                    {
                        ViewData.ModelState.AddModelError("Remark", "*请选择日期");
                    }
                    else
                    {
                        dc.ComContent.InsertOnSubmit(ComContent);
                        dc.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }

                return View(ComContent);
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
                ComContent ComContent = dc.ComContent.FirstOrDefault(m => m.ID == id);
                if (ComContent == null)
                {
                    return HttpNotFound();
                }
                return View(ComContent);
            }
        }

        // POST: ManageCategories/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Contents,CategoryID,Assessment,Keywords,Remark,Org_ID")] ComContent model)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (model.Title == null)
                    {
                        ViewData.ModelState.AddModelError("Title", "*标题不能为空");
                    }
                    else if (model.CategoryID == null || model.CategoryID == -1)
                    {
                        ViewData.ModelState.AddModelError("CategoryID", "*请选择分类");
                    }
                    else if (model.Remark == null)
                    {
                        ViewData.ModelState.AddModelError("Remark", "*请选择日期");
                    }
                    else if (model.Org_ID == null || model.Org_ID == -1)
                {
                    ViewData.ModelState.AddModelError("Org_ID", "*请选择机构");
                }

                else
                {
                    var temp = dc.ComContent.FirstOrDefault(m => m.ID == model.ID);
                    temp.Title = model.Title;
                    temp.CategoryID = model.CategoryID;
                    temp.Assessment = model.Assessment;
                    temp.Contents = model.Contents;
                    temp.Keywords = model.Keywords;
                    temp.Remark = model.Remark;
                    temp.Org_ID = model.Org_ID;
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
                ComContent ComContent = dc.ComContent.FirstOrDefault(m => m.ID == id);
                if (ComContent == null)
                {
                    return HttpNotFound();
                }
                return View(ComContent);
            }
        }

        // POST: ManageCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                ComContent ComContent = dc.ComContent.FirstOrDefault(m => m.ID == id);
                dc.ComContent.DeleteOnSubmit(ComContent);
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
