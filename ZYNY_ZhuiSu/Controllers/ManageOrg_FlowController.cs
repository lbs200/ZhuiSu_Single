using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageOrg_FlowController : BaseController
    {
        //
        // GET: /ManageOrg_Flow/
        public ActionResult Index(int? page, int orgID, int? prodID)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.orgID = orgID;
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }

            prodID = prodID ?? 0;
            #region 产品信息
            List<LinqModel.Products> prods = new DAL.Products().GetAllwithORGID(orgID);
            ViewBag.prods = prods;
            if (prods.Count > 0 && prodID == 0)
            { prodID = prods[0].ID; }
            #endregion
            ViewBag.prodsCurrent = prodID;

            #region 流程信息
            List<string> listFlow = new List<string>();
            var list = new DAL.Org_Flow().GetListAndFlow(txtSearch, orgID, (int)prodID, pIndex, 10, out listFlow);
            for (int i = 0; i < listFlow.Count; i++)
            {
                ViewBag.listFlow += "开始&nbsp;→ &nbsp;" + listFlow[i] + "结束<br />";
            }
            #endregion

            if (prods.Count > 0)
            {
                ViewBag.titleFlow = "【企业：" + new DAL.Organization().GetModel(orgID).Name + "】【产品：" + prods.FirstOrDefault(m => m.ID == prodID).Name + "】";
                ViewBag.hidden = new DAL.Trace_Info().Exsist((int)prodID);
            }

            return View(list);
        }
        [HttpPost]
        public ActionResult Index()
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.orgID = Request["orgID"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = 1;
            int prodID = 0;
            if (!string.IsNullOrEmpty(Request["prods"]))
            {
                prodID = int.Parse(Request["prods"]);
            }
            #region 产品信息
            List<LinqModel.Products> prods = new DAL.Products().GetAllwithORGID(int.Parse(Request["orgID"]));
            ViewBag.prods = prods;
            #endregion
            ViewBag.prodsCurrent = prodID;

            #region 流程信息
            List<string> listFlow = new List<string>();
            var list = new DAL.Org_Flow().GetListAndFlow(txtSearch, int.Parse(Request["orgID"]), prodID, pIndex, 10, out listFlow);
            for (int i = 0; i < listFlow.Count; i++)
            {
                ViewBag.listFlow += "开始&nbsp;→ &nbsp;" + listFlow[i] + "结束<br />";
            }
            #endregion

            if (prods.Count > 0)
            {
                ViewBag.titleFlow = "【企业：" + new DAL.Organization().GetModel(int.Parse(Request["orgID"])).Name + "】【产品：" + prods.FirstOrDefault(m => m.ID == prodID).Name + "】";
                ViewBag.hidden = new DAL.Trace_Info().Exsist((int)prodID);
            }

            return View(list);
        }
        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Org_Flow().Del(id);
            string orgID = Request.QueryString["orgID"];
            if (ret.IsSuccess)
            {
                string redirect = ("/ManageOrg_Flow/Index?page=" + Request.QueryString["page"] + "&orgID=" + orgID + "&prodID=" + Request.QueryString["prodID"]);
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageOrg_Flow/Index?page=" + orgID + "&orgID=" + Request.QueryString["orgID"] + "&prodID=" + Request.QueryString["prodID"] + "';</script>;");
            }
        }
        public ActionResult DownFlowInfo(int id)
        {
            string[] resultGet = new DAL.Org_Flow().GetNextFlow(id);
            ViewBag.currentFlow = resultGet[0];
            ViewBag.nextFlow = resultGet[1];
            ViewBag.prodName = new DAL.Products().GetModel((int)(new DAL.Org_Flow().GetModelView(id).Prod_ID)).Name;
            ViewBag.orgName = new DAL.Organization().GetModel(new DAL.Org_Flow().GetModel(id).Org_ID).Name;

            return View();
        }
        public ActionResult Add(int orgID, int prodID)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            ViewBag.orgID = Request["orgID"];
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            ViewBag.prodID = prodID;
            ViewBag.titleFlow = "【企业：" + new DAL.Organization().GetModel(int.Parse(Request["orgID"])).Name + "】【产品：" + ViewBag.prodName + "】";
            if (string.IsNullOrEmpty(txtSearch))
            {
                //return View(Common.Argument.Public.listMeta_Flow);
                return View(new DAL.Meta_Flow().GetAll());
            }
            else
            {
                //return View(Common.Argument.Public.listMeta_Flow.Where(m => m.Flow_Name.Contains(txtSearch)).ToList());
                return View(new DAL.Meta_Flow().GetAll().Where(m => m.Flow_Name.Contains(txtSearch)).ToList());
            }
        }
        public string AddMethod(int id, int orgID, int prodID)
        {
            var modelFolw = new DAL.Meta_Flow().GetModel(id);
            LinqModel.Org_Flow model = new LinqModel.Org_Flow();
            model.Current_State = "";
            model.Flow_ID = modelFolw.Flow_ID;
            model.Org_ID = orgID;
            model.Seq_No = 0;
            model.Sup_Flow_ID = 0;
            model.Prod_ID = prodID;
            Common.Argument.RetResult ret = new DAL.Org_Flow().Add(model);
            return ret.Msg;
        }
        public ActionResult DownFlowAdd(int id, int orgID, int prodID)
        {
            if (Request["id"] != null)
            {
                id = int.Parse(Request["id"]);
            }
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            ViewBag.id = id;
            ViewBag.orgID = orgID;
            ViewBag.prodsCurrent = prodID;
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            ViewBag.orgName = new DAL.Organization().GetModel(orgID).Name;
            ViewBag.flowName = new DAL.Org_Flow().GetModelView(id).Flow_Name;
            return View(new DAL.Org_Flow().GetList(txtSearch, orgID, prodID, 1, int.MaxValue).Where(m => m.Org_Flow_ID != id && m.Sup_Flow_ID != id).ToList());
        }
        [HttpPost]
        public ActionResult DownFlowAdd()
        {
            int id = 0;
            if (Request["id"] != null)
            {
                id = int.Parse(Request["id"]);
            }
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            ViewBag.id = id;
            ViewBag.orgID = Request["orgID"];
            int orgID = int.Parse(Request["orgID"]);
            int prodID = int.Parse(Request["prods"]);
            ViewBag.prodsCurrent = prodID;
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            ViewBag.orgName = new DAL.Organization().GetModel(orgID).Name;
            ViewBag.flowName = new DAL.Org_Flow().GetModelView(id).Flow_Name;
            return View(new DAL.Org_Flow().GetList(txtSearch, int.Parse(Request["orgID"]), prodID, 1, int.MaxValue).Where(m => m.Org_Flow_ID != id && m.Sup_Flow_ID != id).ToList());
        }
        public string DownFlowAddMethod(int id, int nextID, int orgID)
        {
            Common.Argument.RetResult ret = new DAL.Org_Flow().UpdateNext(orgID, id, nextID);
            return ret.Msg;
        }
        public ActionResult SearchEnt(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            return View(new DAL.Organization().GetPgedList(txtSearch, pIndex, 10));
        }
        public ActionResult SepUp(int id, int orgID, int prodID)
        {
            Common.Argument.RetResult ret = new DAL.Org_Flow().SepUp(id, orgID, prodID);
            //string orgID = Request.QueryString["orgID"];

            return Content("<script>this.location = '/ManageOrg_Flow/Index?page=" + Request.QueryString["page"] + "&orgID=" + Request.QueryString["orgID"] + "&txtSearch" + Request.QueryString["txtSearch"] + "&prodID=" + prodID + "';</script>;");

        }
        public ActionResult SepDown(int id, int orgID, int prodID)
        {
            Common.Argument.RetResult ret = new DAL.Org_Flow().SepDown(id, orgID, prodID);
            //string orgID = Request.QueryString["orgID"];

            return Content("<script>this.location = '/ManageOrg_Flow/Index?page=" + Request.QueryString["page"] + "&orgID=" + Request.QueryString["orgID"] + "&txtSearch" + Request.QueryString["txtSearch"] + "&prodID=" + prodID + "';</script>;");

        }
        [HttpGet]
        public ActionResult Flow_Info(int id, int orgID, int prodID)
        {
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            ViewBag.orgName = new DAL.Organization().GetModel(orgID).Name;
            ViewBag.flowName = new DAL.Org_Flow().GetModelView(id).Flow_Name;
            ViewBag.hidden = new DAL.Trace_Info().Exsist(prodID);
            ViewBag.id = id;
            ViewBag.orgID = orgID;
            ViewBag.prodID = prodID;
            #region 已经添加的信息元
            List<LinqModel.View_Org_Info> list = new DAL.Org_Info().GetList(id);
            List<int> listCurrent = new List<int>();
            StringBuilder sb1 = new StringBuilder();
            foreach (var temp in list)
            {
                listCurrent.Add(temp.Info_ID);
                if (temp.Public == 1)
                {
                    sb1.Append(temp.Info_Name + "(公开)，");
                }
                else
                {
                    sb1.Append(temp.Info_Name + "，");
                }
            }
            ViewBag.multiSelect = sb1.ToString();
            #endregion

            #region 信息元分组
            List<LinqModel.MetaInfoGroup> listGroup = new DAL.MetaInfoGroup().GetAll();
            var m = new LinqModel.MetaInfoGroup();
            m.Group_ID = 0;
            m.Group_Name = "全部";
            listGroup.Insert(0, m);
            ViewBag.listGroup = listGroup;
            #endregion

            return View(new DAL.Meta_Info().GetAllByGroupWithOut(0, listCurrent, ""));
        }
        public ActionResult Flow_InfoAddTraceInfo(int orgFlowID, int infoID, int isPublic, int prodID, int orgID)
        {
            var model = new LinqModel.Org_Info();
            model.Org_Flow_ID = orgFlowID;
            model.Info_ID = infoID;
            model.Public = isPublic;
            Common.Argument.RetResult ret = new DAL.Org_Info().Add(model);
            if (ret.IsSuccess)
            {
                string redirect = ("/ManageOrg_Flow/Flow_Info?id=" + orgFlowID + "&orgID=" + orgID + "&prodID=" + prodID);
                return Content("<script>this.location = '" + redirect + "';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageOrg_Flow/Flow_Info?id=" + orgFlowID + "&orgID=" + orgID + "&prodID=" + prodID + "';</script>;");
            }
        }
        public ActionResult Get_Meta_Info(int groupID, int orgFlowID, int prodID)
        {
            ViewBag.hidden = new DAL.Trace_Info().Exsist(prodID);
            string txtSearch = Request.QueryString["txtSearch"];
            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = Common.Argument.Public.HtmlDecode(txtSearch);
            }

            #region 已经添加的信息元
            List<LinqModel.View_Org_Info> list = new DAL.Org_Info().GetList(orgFlowID);
            List<int> listCurrent = new List<int>();
            foreach (var temp in list)
            {
                listCurrent.Add(temp.Info_ID);
            }
            #endregion
            var listInfo = new DAL.Meta_Info().GetAllByGroupWithOut(groupID, listCurrent, txtSearch);
            StringBuilder sb = new StringBuilder();
            foreach (var m in listInfo)
            {
                sb.Append(" <tr><td>" + m.Info_Name + "</td><td><a href=\"javascript:void(0)\" title=\"" + m.Info_Description + "\">点击查看</a></td><td><input type=\"checkbox\" value=\"是否公开\" id=\"ck_" + m.Info_ID + "\" /></td><td>");
                if (ViewBag.hidden)
                {
                    sb.Append("<span style=\"color:red;\">无法新增</span>");
                }
                else
                {
                    sb.Append("<a href=\"javascript:ClickSelect(" + m.Info_ID + ")\">新增</a>");
                }
                sb.Append("</td></tr>");
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public string GetFlow(int orgID, int prodID)
        {
            StringBuilder sb = new StringBuilder();
            #region 流程信息
            List<string> listFlow = new List<string>();
            var list = new DAL.Org_Flow().GetListAndFlow("", orgID, (int)prodID, 1, 10, out listFlow);
            for (int i = 0; i < listFlow.Count; i++)
            {
                sb.Append("开始&nbsp;→ &nbsp;" + listFlow[i] + "结束<br />");
            }
            #endregion
            return sb.ToString();
        }
        public ActionResult Cycle(int orgID, int prodID)
        {
            ViewBag.prodID = prodID;
            ViewBag.orgID = orgID;
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            var listCycle = new DAL.Org_Flow_Cycle_Info().GetByProdID(prodID);
            ViewBag.listFlow = new DAL.Org_Flow().GetList("", orgID, prodID, 1, int.MaxValue);
            return View(listCycle);
        }
        [HttpPost]
        public ActionResult Cycle(LinqModel.Org_Flow_Cycle_Info model)
        {
            Common.Argument.RetResult ret = new DAL.Org_Flow_Cycle_Info().Add(model);

            return Content("<script>this.location = '/ManageOrg_Flow/Cycle?orgID=" + model.Org_ID + "&prodID=" + model.Prod_ID + "';</script>;");
        }
        public ActionResult CycleDel(int id, int orgID, int prodID)
        {
            Common.Argument.RetResult ret = new DAL.Org_Flow_Cycle_Info().Del(id);

            return Content("<script>this.location = '/ManageOrg_Flow/Cycle?orgID=" + orgID + "&prodID=" + prodID + "';</script>;");
        }
        public ActionResult CycleCondition(int id, int orgID, int prodID)
        {
            ViewBag.id = id;
            ViewBag.orgID = orgID;
            ViewBag.prodID = prodID;
            var model = new DAL.Org_Flow_Cycle_Info().GetModel(id);
            ViewBag.cycleInfo = new DAL.Org_Flow().GetModelView((int)model.CycleStart).Flow_Name + " -> " + new DAL.Org_Flow().GetModelView((int)model.CycleEnd).Flow_Name;
            ViewBag.conditioncycles = "2";//循环条件圈数
            ViewBag.conditionmeta = "id_2_1:'lol'";//循环条件信息元
            ViewBag.conditionmetavalue = "id_2_1:'lol'";//循环条件信息元值
            return View();
        }

        public string GetMeta_InfoType(int metaInfoID)
        {
            string result = "0";
            var modelMetaInfo = new DAL.Meta_Info().GetModel(metaInfoID);
            if (modelMetaInfo.Data_Type == "整数" || modelMetaInfo.Data_Type == "浮点数" || modelMetaInfo.Data_Type == "日期类型")
            {
                result = "0";
            }
            else
            {
                result = "1";
            }
            return result;
        }
    }
}