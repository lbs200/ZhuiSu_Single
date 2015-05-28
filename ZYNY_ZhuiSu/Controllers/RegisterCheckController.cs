using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class RegisterCheckController : BaseController
    {
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.RegisterCheck().GetPgedList(txtSearch,pIndex, 10));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new DAL.RegisterCheck().GetModel(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new DAL.RegisterCheck().GetModel(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult UpdateCheckStatus()
        {
            if (Request.Form["id"] != null &&  Request.Form["CheckStatus"] != null)
            {
                var id = Convert.ToInt32(Request.Form["id"]);
                var checkStatus = Convert.ToInt32(Request.Form["CheckStatus"]);
                var ret = new DAL.RegisterCheck().UpdateStatusById(id, checkStatus);
                if (ret.IsSuccess && checkStatus == 1)
                {
                    
                }
                return Content("操作完成");
            }
            
            return Content("操作失败！");
        }
        public bool CopyUpdate(int id)
        {
            var ret = new DAL.RegisterCheck().CopyToUserAndOrg(id);
            return false;
        }

        [HttpPost]
        public ActionResult Update()
        {
            try
            {
                if (Request.Form["ID"] != null && Request.Form["CheckStatus"] != null)
                {
                    var id = Convert.ToInt32(Request.Form["ID"]);
                    var checkStatus = Convert.ToInt32(Request.Form["CheckStatus"]);
                    var ret = new DAL.RegisterCheck().UpdateStatusById(id, checkStatus);
                    if (ret.IsSuccess && checkStatus == 1)
                    {
                        ret = new DAL.RegisterCheck().CopyToUserAndOrg(id);
                        if (ret.IsSuccess)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    
                }
            
                return RedirectToAction("Index");
            }
            catch
            {
                
            }
            return RedirectToAction("Index");
        }

        // GET: RegisterCheck/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisterCheck/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RegisterCheck/Edit/5
        

        // GET: RegisterCheck/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new DAL.RegisterCheck().GetModel(id.Value);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: RegisterCheck/Delete/5
        [HttpPost]
        public ActionResult Delete()
        {
            var id = Request.Form["ID"];
            try
            {
                if (id != null)
                {
                    var ret = new DAL.RegisterCheck().Delete(Convert.ToInt32(id));
                    if (ret.IsSuccess)
                    {

                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete","RegisterCheck",id);
            }
        }
    }
}
