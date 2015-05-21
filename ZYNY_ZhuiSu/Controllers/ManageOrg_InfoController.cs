using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageOrg_InfoController : BaseController
    {
        //
        // GET: /ManageOrg_Info/
        public ActionResult Index(int orgID, int orgFlowID, int prodID)
        {
            ViewBag.hidden = new DAL.Trace_Info().Exsist(prodID);
            ViewBag.orgID = orgID;
            ViewBag.orgFlowID = orgFlowID;
            ViewBag.prodID = prodID;
            ViewBag.orgName = new DAL.Organization().GetModel(orgID).Name;
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            ViewBag.orgFlowName = new DAL.Org_Flow().GetModelView(orgFlowID).Flow_Name;
            return View(new DAL.Org_Info().GetList(orgFlowID));
        }
        [HttpGet]
        public ActionResult Update(int id, int orgID, int orgFlowID, int prodID)
        {
            ViewBag.id = id;
            ViewBag.orgID = orgID;
            ViewBag.orgFlowID = orgFlowID;
            ViewBag.prodID = prodID;
            ViewBag.orgName = new DAL.Organization().GetModel(orgID).Name;
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            ViewBag.orgFlowName = new DAL.Org_Flow().GetModelView(orgFlowID).Flow_Name;
            return View(new DAL.Org_Info().GetModel(id));
        }
        [HttpPost]
        public ActionResult Update()
        {
            LinqModel.Org_Info model = new DAL.Org_Info().GetModelM(int.Parse(Request["id"]));
            model.Data_Type_Value = Request["Data_Type_Value"];
            bool s = false;
            bool.TryParse(Request["Search_Point"], out s);
            bool r = false;
            bool.TryParse(Request["Required"], out r);
            model.Search_Point = s;
            model.Required = r;
            Common.Argument.RetResult ret = new DAL.Org_Info().Update(model);
            return Content("<script>this.location = '/ManageOrg_Info/Index?orgID=" + Request["orgID"] + "&orgFlowID=" + Request["orgFlowID"] + "&prodID=" + Request["prodID"] + "';</script>;");
        }
        public ActionResult Del(int id, int orgID, int orgFlowID, int prodID)
        {
            Common.Argument.RetResult ret = new DAL.Org_Info().Del(id);
            return Content("<script>this.location = '/ManageOrg_Info/Index?orgID=" + Request["orgID"] + "&orgFlowID=" + Request["orgFlowID"] + "&prodID=" + Request["prodID"] + "';</script>;");
        }

        public JsonResult GetAutoComplete()
        {
            List<LinqModel.OrgAutoCompleteModel> list = new List<LinqModel.OrgAutoCompleteModel>();
            list = new DAL.Organization().GetAutoComplete();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}