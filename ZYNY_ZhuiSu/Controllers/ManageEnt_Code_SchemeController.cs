using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageEnt_Code_SchemeController : BaseController
    {
        //
        // GET: /Ent_Code_Scheme/
        public ActionResult Index(int? page, int orgID)
        {
            ViewBag.txtSearch = Request["txtSearch"];
            ViewBag.orgID = orgID;
            int pIndex = page ?? 0;
            ViewBag.page = pIndex;
            if (pIndex <= 0) { pIndex = 1; }

            return View(new DAL.Ent_Code_Scheme().GetList(orgID, pIndex, 10));
        }
        [HttpGet]
        public ActionResult Add(int orgID)
        {
            ViewBag.Org_ID = orgID;
            ViewBag.error = 0;
            return View();
        }
        [HttpPost]
        public ActionResult Add(LinqModel.Ent_Code_Scheme model)
        {
            ViewBag.error = 0;
            model.Total_Seg = 2;
            model.SN_Position = 2;
            if (model.Date_Required == true)
            {
                model.Total_Seg = 3;
                model.SN_Position = 3;
            }
            Common.Argument.RetResult ret = new DAL.Ent_Code_Scheme().Add(model);

            if (ret.IsSuccess)
            {
                return RedirectToAction("Index", new { page = 1, orgID = model.Org_ID });
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Update(int id, int page, string txtSearch)
        {
            ViewBag.page = page;
            ViewBag.txtSearch = txtSearch;
            ViewBag.error = 0;
            return View(new DAL.Ent_Code_Scheme().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Ent_Code_Scheme model)
        {
            model.Total_Seg = 2;
            model.SN_Position = 2;
            if (model.Date_Required == true)
            {
                model.Total_Seg = 3;
                model.SN_Position = 3;
            }
            Common.Argument.RetResult ret = new DAL.Ent_Code_Scheme().Update(model);
            if (ret.IsSuccess)
            {
                return RedirectToAction("Index", new { page = Request["page"], orgID = model.Org_ID, txtSearch = Request["txtSearch"] });
            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(model);
            }
        }
        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Ent_Code_Scheme().Del(id);
            if (ret.IsSuccess)
            {
                //Response.Redirect("/ManageEnt_Code_Scheme/Index?page=" + Request["page"] + "&orgID=" + Request["orgID"] + "&txtSearch=" + Request["txtSearch"]);
                string redirect = "/ManageEnt_Code_Scheme/Index?page=" + Request["page"] + "&orgID=" + Request["orgID"] + "&txtSearch=" + Request["txtSearch"];
                return Content("<script>this.location = " + redirect + "</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageEnt_Code_Scheme/Index?page=" + Request["page"] + "&orgID=" + Request["orgID"] + "&txtSearch" + Request["txtSearch"] + "';</script>;");
            }
        }

        public ActionResult Info_Pool(int id)
        {
            return View(new DAL.Ent_Code_Pool().GetModel(id));
        }

        public ActionResult SearchEnt(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Organization().GetPgedList(txtSearch, pIndex, 10));
        }
    }
}