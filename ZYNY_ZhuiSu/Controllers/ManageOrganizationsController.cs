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
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageOrganizationsController : BaseController
    {
        public string Upload(FormContext from)
        {
            var file = System.Web.HttpContext.Current.Request.Files["Filedata"];
            string uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/images/");
            string url = "/images/" + file.FileName;
            if (file != null)
            {
                file.SaveAs(uploadPath + file.FileName);

                return url;//返回保存的地址
            }
            else
            {
                return "0";
            }

        }


        // GET: ManageOrganizations
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)//系统管理员
            {
                return View(new DAL.Organization().GetPgedList(txtSearch, pIndex, 10));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)//企业管理员
            {
                return View(new DAL.Organization().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
                //.Where(m => m.Sup_Org == (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID)
            }
            else{
                return View(new DAL.Organization().GetPgedListNewUser(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
           
            }


        }

        // GET: ManageOrganizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinqModel.View_Org model = new DAL.View_Org().GetModel(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult GetP(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.AdministrationRegion
                         select new
                         {
                             ID = m.ID,
                             Province = m.Province,
                             City = m.City,


                         };
                return Json(pp.ToList().Where(m => m.City.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetZIP(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.AdministrationRegion
                         select new
                         {
                             ID = m.ID,
                             Province = m.Province,
                             City = m.City,
                             ZIP = m.Postal_Code,

                         };
                return Json(pp.ToList().Where(m => m.ID.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetBM(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.Code_Scheme
                         select new
                         {
                             Scheme_ID = m.Scheme_ID,
                             Prefix = m.Prefix,
                             Separator = m.Separator,
                             Name = m.Name,

                         };
                return Json(pp.ToList().Where(m => m.Scheme_ID.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetAllP(string sheng, string shi, string qu)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.Organization
                         select new
                         {
                             Province = m.Province,
                             City = m.City,
                             District = m.District

                         };
                pp = pp.Where(m => m.Province.ToString().Contains(sheng));
                pp = pp.Where(m => m.City.ToString().Contains(shi));
                pp = pp.Where(m => m.District.ToString().Contains(qu));
                return Json(pp.ToList(), JsonRequestBehavior.AllowGet);
            }
        }

        // GET: ManageOrganizations/Create
      
        public ActionResult Create()
        {

            List<LinqModel.Code_Scheme> cs = new DAL.Code_Scheme().GetAll();
            ViewData["BMFA"] = new SelectList(cs, "Scheme_ID", "Name");
            List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
            ViewData["Categories"] = new SelectList(gb, "C4", "Name");
            ViewBag.selectlist = new DAL.Region().GetAllSheng();
            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName ;
            //string filePhysicalPath = Server.MapPath("~/images/" + uname + "/");
            //if (!Directory.Exists(filePhysicalPath))//判断上传文件夹是否存在，若不存在，则创建
            //{
            //    Directory.CreateDirectory(filePhysicalPath);//创建文件夹
            //}
            Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            //ViewBag.ZIP = Convert.ToInt32(new DAL.Organization().GetMaxID()) + 1;
            return View();
        }

        // POST: ManageOrganizations/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
         
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Org_ID,Name,Province,City,District,Address,Contact,Tel,E_mail_,Org_URL,Sup_Org,Intro,Brand,Cert,Org_Code,EWMUrl")] Organization organization)
        {

            if (ModelState.IsValid)
            {
                if (new DAL.Organization().Exict(organization.Name))
                {
                    ModelState.AddModelError("Name", "名称已经被占用");

                }
                else
                {
                    if (organization.Province == null)
                    {
                        ViewData.ModelState.AddModelError("Province", "*请选择省");
                        
                    }
                    else if (organization.City == null)
                    {
                        ViewData.ModelState.AddModelError("City", "*请选择市");
                    }
                    else if (organization.Sup_Org == null)
                    {
                        ViewData.ModelState.AddModelError("Sup_Org", "*请选择上级机构");
                    }
                    else if (organization.Intro == null)
                    {
                        ViewData.ModelState.AddModelError("Intro", "*请编辑企业简介");
                    }
                    
                    else
                    {
                        Common.Argument.RetResult ret = new DAL.Organization().Create(organization);
                        return RedirectToAction("Index");
                    }

                }


            }
            List<LinqModel.Code_Scheme> cs = new DAL.Code_Scheme().GetAll();
            ViewData["BMFA"] = new SelectList(cs, "Scheme_ID", "Name");
            List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
            ViewData["Categories"] = new SelectList(gb, "C4", "Name");
            ViewBag.selectlist = new DAL.Region().GetAllSheng();
           
            return View(organization);

        }

        // GET: ManageOrganizations/Edit/5
        public ActionResult Edit(int? id)
        {

            ViewBag.selectlist = new DAL.Region().GetAllSheng();
            ViewBag.ZIP = Convert.ToInt32(new DAL.Organization().GetMaxID()) + 1;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organization organization = new DAL.Organization().GetModel(id);

            if (organization == null)
            {
                return HttpNotFound();
            }
            List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
            ViewData["Categories"] = new SelectList(gb, "C4", "Name", organization.Org_Code.ToString().Trim().Substring(2, organization.Org_Code.ToString().Trim().IndexOf("-") - 2));
            return View(organization);
        }

        // POST: ManageOrganizations/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Org_ID,Name,Province,City,District,Address,Contact,Tel,E_mail_,Org_URL,Sup_Org,Intro,Brand,Cert,Org_Code")] LinqModel.Organization organization)
        {
            if (ModelState.IsValid)
            {
                if (new DAL.Organization().Exist(organization.Name))
                {
                    ModelState.AddModelError("Name", "用户名已经被占用");
                }
                else
                {
                    Common.Argument.RetResult ret = new DAL.Organization().Edit(organization);
                    return RedirectToAction("Index");
                }

            }
            return View(organization);
        }
        public ActionResult Update(int id)
        {
            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;           
            Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            LinqModel.Organization organization = new DAL.Organization().GetModel(id);

            ViewBag.selectlist = new DAL.Region().GetAllSheng();
            ViewBag.ZIP = Convert.ToInt32(new DAL.Organization().GetMaxID()) + 1;
            ViewBag.error = 0;
           // List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
           // ViewData["Categories"] = new SelectList(gb, "C4", "Name", organization.Org_Code.ToString().Trim().Substring(2, organization.Org_Code.ToString().Trim().IndexOf("-") - 2));

            return View(organization);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update(LinqModel.Organization model)
        {
            
                Common.Argument.RetResult ret = new DAL.Organization().Update(model);

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
        // GET: ManageOrganizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            View_Org organization = new DAL.View_Org().GetModel(id);
            if (organization == null)
            {
                return HttpNotFound();
            }
            return View(organization);
        }

        // POST: ManageOrganizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Common.Argument.RetResult ret = new DAL.Organization().Del(id);
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

        /////////////////////////////////////////////////////test upload pic//////////////////////////////////////////
       



    }
}
