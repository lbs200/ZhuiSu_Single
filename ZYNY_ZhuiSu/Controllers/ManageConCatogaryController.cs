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
    public class ManageContCatogaryController : Controller
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
                return View(new DAL.ContCatogary().GetPgedListWithORG(txtSearch, pIndex, 10, CategoryDDL));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)//企业管理员
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View(new DAL.ContCatogary().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
                //.Where(m => m.Sup_Org == (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID)
            }
            else
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
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
                ContCatogary ContCatogary = dc.ContCatogary.FirstOrDefault(m => m.ID == id);
                if (ContCatogary == null)
                {
                    return HttpNotFound();
                }
                return View(ContCatogary);
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
        public ActionResult Create([Bind(Include = "ID,Name,FatherID,Description,Assessment,Keywords,Remark,Org_ID")] ContCatogary ContCatogary)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (ContCatogary.Name == null)
                    {
                        ViewData.ModelState.AddModelError("Name", "*名称不能为空");

                    }
                    else if (ContCatogary.FatherID == null || ContCatogary.FatherID == -1)
                    {
                        ViewData.ModelState.AddModelError("FatherID", "*请选择上级栏目");
                    }
                    else if (ContCatogary.Org_ID == null || ContCatogary.Org_ID == -1)
                    {
                        ViewData.ModelState.AddModelError("Org_ID", "*请选择机构");
                    }
                   
                    else
                    {
                        dc.ContCatogary.InsertOnSubmit(ContCatogary);
                        dc.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }

                return View(ContCatogary);
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
                ContCatogary ContCatogary = dc.ContCatogary.FirstOrDefault(m => m.ID == id);
                if (ContCatogary == null)
                {
                    return HttpNotFound();
                }
                return View(ContCatogary);
            }
        }

        // POST: ManageCategories/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,FatherID,Description,Assessment,Keywords,Remark,Org_ID")] ContCatogary model)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (model.Name == null)
                    {
                        ViewData.ModelState.AddModelError("Name", "*名称不能为空");

                    }
                    else if (model.FatherID == null)
                    {
                        ViewData.ModelState.AddModelError("FatherID", "*请上级栏目");
                    }
                    else if (model.Org_ID == null||model.Org_ID==-1)
                     {
                         ViewData.ModelState.AddModelError("Org_ID", "*请选择机构");
                     }

                     else
                     {
                         var temp = dc.ContCatogary.FirstOrDefault(m => m.ID == model.ID);
                         temp.Description = model.Description;
                         temp.FatherID = model.FatherID;
                         temp.Assessment = model.Assessment;
                         temp.Name = model.Name;

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
                ContCatogary ContCatogary = dc.ContCatogary.FirstOrDefault(m => m.ID == id);
                if (ContCatogary == null)
                {
                    return HttpNotFound();
                }
                return View(ContCatogary);
            }
        }

        // POST: ManageCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                ContCatogary ContCatogary = dc.ContCatogary.FirstOrDefault(m => m.ID == id);
                dc.ContCatogary.DeleteOnSubmit(ContCatogary);
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
