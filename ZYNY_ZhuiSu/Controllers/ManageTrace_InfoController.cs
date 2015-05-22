using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageTrace_InfoController : BaseController
    {
        //
        // GET: /ManageTrace_Info/
        public ActionResult Index()
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            ViewBag.orgID = orgID;
            ViewBag.orgName = new DAL.Organization().GetModel(orgID).Name;
            return View(new DAL.Products().GetAllwithORGID(orgID));
        }
        public ActionResult PrintStudent(int orgFlowID, int prodID, int? page)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            ViewBag.orgID = orgID;
            ViewBag.orgFlowID = orgFlowID;
            ViewBag.prodID = prodID;
            var modelMetaFlow = new DAL.Org_Flow().GetModelView(orgFlowID);
            ViewBag.flowName = modelMetaFlow.Flow_Name;
            ViewBag.search = 0;
            ViewBag.txtSearch = Request.QueryString["txtSearch"];
            if (Request.QueryString["txtSearch"] == null)
            {
                ViewBag.txtSearch = "";
            }
            else
            {
                ViewBag.search = 1;
            }
            ViewBag.timeS = Request.QueryString["timeS"];
            ViewBag.timeE = Request.QueryString["timeE"];
            if (Request.QueryString["timeS"] == null)
            {

                ViewBag.timeS = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            if (Request.QueryString["timeE"] == null)
            {
                ViewBag.timeE = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            //return View(new DAL.Trace_Info().GetModel(Request["txtSearch"], orgFlowID, prodID));

            return PartialView(new DAL.Trace_Info().GetList(ViewBag.txtSearch, orgFlowID, prodID, ViewBag.timeS, ViewBag.timeE, pIndex, 10));
        }
        [HttpPost]
        public ActionResult PrintStudent()
        {
            ViewBag.orgID = Request["orgID"];
            ViewBag.orgFlowID = Request["orgFlowID"];
            ViewBag.prodID = Request["prodID"];
            ViewBag.txtSearch = Request["txtSearch"];
            ViewBag.timeS = Request["timeS"];
            ViewBag.timeE = Request["timeE"];
            var modelMetaFlow = new DAL.Org_Flow().GetModelView(int.Parse(Request["orgFlowID"]));
            ViewBag.flowName = modelMetaFlow.Flow_Name;
            ViewBag.search = 1;
            return View(new DAL.Trace_Info().GetList(Request["txtSearch"], int.Parse(Request["orgFlowID"]), int.Parse(Request["prodID"]), ViewBag.timeS, ViewBag.timeE, 1, 10));
        }
        [HttpGet]
        public ActionResult List(int orgFlowID, int prodID, int? page)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            ViewBag.orgID = orgID;
            ViewBag.orgFlowID = orgFlowID;
            ViewBag.prodID = prodID;
            var modelMetaFlow = new DAL.Org_Flow().GetModelView(orgFlowID);
            ViewBag.flowName = modelMetaFlow.Flow_Name;
            ViewBag.search = 0;
            ViewBag.txtSearch = Request.QueryString["txtSearch"];
            if (Request.QueryString["txtSearch"] == null)
            {
                ViewBag.txtSearch = "";
            }
            else
            {
                ViewBag.search = 1;
            }
            ViewBag.timeS = Request.QueryString["timeS"];
            ViewBag.timeE = Request.QueryString["timeE"];
            if (Request.QueryString["timeS"] == null)
            {
                ViewBag.timeS = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            if (Request.QueryString["timeE"] == null)
            {
                ViewBag.timeE = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            //return View(new DAL.Trace_Info().GetModel(Request["txtSearch"], orgFlowID, prodID));
            return View(new DAL.Trace_Info().GetList(ViewBag.txtSearch, orgFlowID, prodID, ViewBag.timeS, ViewBag.timeE, pIndex, 10));
        }
        [HttpPost]
        public ActionResult List()
        {
            ViewBag.orgID = Request["orgID"];
            ViewBag.orgFlowID = Request["orgFlowID"];
            ViewBag.prodID = Request["prodID"];
            ViewBag.txtSearch = Request["txtSearch"];
            ViewBag.timeS = Request["timeS"];
            ViewBag.timeE = Request["timeE"];
            var modelMetaFlow = new DAL.Org_Flow().GetModelView(int.Parse(Request["orgFlowID"]));
            ViewBag.flowName = modelMetaFlow.Flow_Name;
            ViewBag.search = 1;
            return View(new DAL.Trace_Info().GetList(Request["txtSearch"], int.Parse(Request["orgFlowID"]), int.Parse(Request["prodID"]), ViewBag.timeS, ViewBag.timeE, 1, 10));
        }
        [HttpGet]
        public ActionResult UpSepList(int orgID, int orgFlowID, int prodID, int? page)
        {
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            var modelOrgFlow = new DAL.Org_Flow().GetModel(orgFlowID);
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            if (modelOrgFlow.Sup_Flow_ID == 0)
            {
                return RedirectToAction("Add", new { orgFlowID = orgFlowID, prodID = prodID, traceIDUp = 0 });
            }
            else
            {
                ViewBag.orgID = orgID;
                ViewBag.orgFlowID = orgFlowID;
                ViewBag.prodID = prodID;
                var modelMetaFlow = new DAL.Org_Flow().GetModelView(orgFlowID);
                ViewBag.FlowNameCurrent = modelMetaFlow.Flow_Name;
                modelMetaFlow = new DAL.Org_Flow().GetModelView(modelMetaFlow.Sup_Flow_ID);
                ViewBag.supFlowID = modelMetaFlow.Org_Flow_ID;
                ViewBag.supFlowName = modelMetaFlow.Flow_Name;
                ViewBag.search = 0;
                ViewBag.txtSearch = Request.QueryString["txtSearch"];
                if (Request.QueryString["txtSearch"] == null)
                {
                    ViewBag.txtSearch = "";
                }
                else
                {
                    ViewBag.search = 1;
                }
                ViewBag.timeS = Request.QueryString["timeS"];
                ViewBag.timeE = Request.QueryString["timeE"];
                if (Request.QueryString["timeS"] == null)
                {
                    ViewBag.timeS = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
                }
                if (Request.QueryString["timeE"] == null)
                {
                    ViewBag.timeE = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
                }
                string nextFlowIDs = new DAL.Org_Flow().GetNextFlowIDs(modelMetaFlow.Org_Flow_ID);

                return View(new DAL.Trace_Info().GetUpSepList(Request.QueryString["txtSearch"], modelMetaFlow.Org_Flow_ID, nextFlowIDs, prodID, ViewBag.timeS, ViewBag.timeE, pIndex, 10));
            }
        }
        [HttpPost]
        public ActionResult UpSepList()
        {
            ViewBag.orgID = Request["orgID"];
            ViewBag.orgFlowID = Request["orgFlowID"];
            ViewBag.prodID = Request["prodID"];
            ViewBag.txtSearch = Request["txtSearch"];
            ViewBag.supFlowID = Request["supFlowID"];
            var modelMetaFlow = new DAL.Org_Flow().GetModelView(int.Parse(Request["supFlowID"]));
            ViewBag.supFlowName = modelMetaFlow.Flow_Name;
            ViewBag.search = 1;
            return View(new DAL.Trace_Info().GetModel(Request["txtSearch"], int.Parse(Request["supFlowID"]), int.Parse(Request["prodID"])));
        }
        [HttpGet]
        public ActionResult Add(int orgFlowID, int prodID, int traceIDUp)
        {
            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
            Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            ViewBag.org_flow_id = orgFlowID;
            ViewBag.prodID = prodID;
            ViewBag.traceIDUp = traceIDUp;
            var modelMetaFlow = new DAL.Org_Flow().GetModelView(orgFlowID);
            ViewBag.flowName = modelMetaFlow.Flow_Name;
            ViewBag.upFlow = modelMetaFlow.Sup_Flow_ID;

            //判断是否是循环结束节点
            ViewBag.flowEnd = new DAL.Org_Flow_Cycle_Info().IsFlowEnd(orgFlowID);

            return View(new DAL.Org_Info().GetList(orgFlowID));
        }
        [HttpPost]
        public ActionResult Add()
        {
            var user = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user;

            LinqModel.Trace_Info model = new LinqModel.Trace_Info();
            model.Org_Flow_ID = int.Parse(Request["org_flow_id"]);
            model.Prod_Code_End = 0;
            model.Prod_Code_Start = 0;
            model.Prod_ID = int.Parse(Request["prodID"]);
            model.Prod_Code_Before = new DAL.Products().GetModel(int.Parse(Request["prodID"])).Adv_Code;
            model.Rec_Time = Common.Argument.Public.GetDateTimeNow();
            model.Trace_ID_Up = int.Parse(Request["traceIDUp"]);
            model.FlowOver = false;
            var modelorgFlow = new DAL.Meta_Flow().GetModel(new DAL.Org_Flow().GetModel(int.Parse(Request["org_flow_id"])).Flow_ID);
            model.Flow_Num = modelorgFlow.Abbr + "_" + model.Rec_Time.ToString("yyyyMMddHHmmssfff");

            if (Request["flowEnd"].ToLower() == "true")
            {
                if (Request["cycleFlowOver"] == "true")
                {
                    model.NextFlowID = "";
                }
                else
                {
                    //var modelCycle = new DAL.Org_Flow_Cycle_Info().GetModel((int)model.Prod_ID, int.Parse(Request["org_flow_id"]));
                    model.NextFlowID = "|" + Request["cycleStart"] + "|";
                }
            }

            StringBuilder sbErrorInfo = new StringBuilder();
            #region 组织XML字段信息
            StringBuilder sb = new StringBuilder();
            sb.Append("<root>");
            sb.Append("<orgFlowID value='" + Request["org_flow_id"] + "'/>");
            sb.Append("<userInfo ID='" + user.ID + "' name='" + user.UserName + "' userCode='" + user.User_Code + "'/>");
            sb.Append("<recTime value='" + model.Rec_Time.ToString("yyyyMMddHHmmssfff") + "'/>");

            var listInfo = new DAL.Org_Info().GetList(int.Parse(Request["org_flow_id"]));
            foreach (var m in listInfo)
            {
                var modelInfo = new DAL.Meta_Info().GetModel(m.Info_ID);
                if (modelInfo.Public || m.Public == 1)
                {
                    #region 判断必填项错误
                    if (m.Required != null && (bool)m.Required)//判断企业追溯信息元是否为必填
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(Request[m.Info_ID.ToString()].ToString()))
                            {
                                sbErrorInfo.Append(m.Info_Name + "为必填项；");
                            }
                        }
                        catch
                        {
                            sbErrorInfo.Append(m.Info_Name + "为必填项；");
                        }
                    }
                    else if(modelInfo.Required)//如果企业追溯信息元不为必填，则判断信息元中是否为必填
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(Request[m.Info_ID.ToString()].ToString()))
                            {
                                sbErrorInfo.Append(m.Info_Name + "为必填项；");
                            }
                        }
                        catch
                        {
                            sbErrorInfo.Append(m.Info_Name + "为必填项；");
                        }
                    }
                    #endregion
                    #region 判断链接格式错误
                    if (modelInfo.Data_Type == "链接")
                    {
                        if (!Request[m.Info_ID.ToString()].ToString().Contains('|') || !Request[m.Info_ID.ToString()].ToString().Contains("http"))
                        {
                            sbErrorInfo.Append(m.Info_Name + "链接格式输入错误：名称|链接地址；");
                        }
                    }
                    #endregion
                    if (modelInfo.Data_Type == "多选框")
                    {
                        sb.Append("<info InfoID='" + m.Info_ID + "'  InfoName='" + m.Info_Name + "' Public='1' Type='" + modelInfo.Data_Type + "'>");
                        var sss = Request[m.Info_ID.ToString()].Split(',');
                        foreach (var mm in sss)
                        {
                            sb.Append("<value>" + mm + "</value>");
                        }
                        sb.Append("</info>");
                    }
                    else if (modelInfo.Data_Type == "图片")
                    {
                        if (!string.IsNullOrEmpty(Request[m.Info_ID.ToString()]))
                            sb.Append("<img InfoID='" + m.Info_ID + "'  InfoName='" + m.Info_Name + "' InfoValue='" + Request[m.Info_ID.ToString()] + "' Public='1' Type='" + modelInfo.Data_Type + "'/>");
                    }
                    else
                    {
                        sb.Append("<info InfoID='" + m.Info_ID + "'  InfoName='" + m.Info_Name + "' InfoValue='" + Request[m.Info_ID.ToString()] + "' Public='1' Type='" + modelInfo.Data_Type + "'/>");
                    }
                }
                else
                {
                    #region 判断必填项错误
                    if (m.Required != null && (bool)m.Required)
                    {
                        if (string.IsNullOrEmpty(Request[m.Info_ID.ToString()].ToString()))
                        {
                            sbErrorInfo.Append(m.Info_Name + "为必填项；");
                        }
                    }
                    #endregion
                    #region 判断链接格式错误
                    if (modelInfo.Data_Type == "链接")
                    {
                        if (!Request[m.Info_ID.ToString()].ToString().Contains('|') || !Request[m.Info_ID.ToString()].ToString().Contains("http"))
                        {
                            sbErrorInfo.Append(m.Info_Name + "链接格式输入错误：名称|链接地址；");
                        }
                    }
                    #endregion
                    if (modelInfo.Data_Type == "多选框")
                    {
                        sb.Append("<info InfoID='" + m.Info_ID + "'  InfoName='" + m.Info_Name + "' Public='0' Type='" + modelInfo.Data_Type + "'>");
                        var sss = Request[m.Info_ID.ToString()].Split(',');
                        foreach (var mm in sss)
                        {
                            sb.Append("<value>" + mm + "</value>");
                        }
                        sb.Append("</info>");
                    }
                    else if (modelInfo.Data_Type == "图片")
                    {
                        if (!string.IsNullOrEmpty(Request[m.Info_ID.ToString()]))
                            sb.Append("<img InfoID='" + m.Info_ID + "'  InfoName='" + m.Info_Name + "' InfoValue='" + Request[m.Info_ID.ToString()] + "' Public='0' Type='" + modelInfo.Data_Type + "'/>");
                    }
                    else
                    {
                        sb.Append("<info InfoID='" + m.Info_ID + "'  InfoName='" + m.Info_Name + "' InfoValue='" + Request[m.Info_ID.ToString()] + "' Public='0' Type='" + modelInfo.Data_Type + "'/>");
                    }
                }
            }

            //sb.Append("<img src='" + System.Configuration.ConfigurationManager.AppSettings["hostName"] + fileFull + "' />");
            if (Request["upFlowOver"] == "true")
            {
                sb.Append("<upFlowOver value='true' />");
            }
            else
            {
                sb.Append("<upFlowOver value='false' />");
            }
            sb.Append("</root>");
            #endregion

            //必填信息为空，提示下边
            if (!string.IsNullOrEmpty(sbErrorInfo.ToString()))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = sbErrorInfo.ToString();
                return Content("<script>alert('" + sbErrorInfo.ToString() + "');</script>");
            }
            else
            {
                model.Trace_Info_Value = XElement.Parse(sb.ToString());

                Common.Argument.RetResult ret = new DAL.Trace_Info().Add(model);

                if (ret.IsSuccess)
                {
                    //RedirectToAction("Index");
                    return Content("<script>alert('" + ret.Msg + "');this.location = '/ManageTrace_Info/List?orgFlowID=" + Request["org_flow_id"] + "&prodID=" + Request["prodID"] + "&txtSearch=" + model.Flow_Num + "';</script>");
                }
                else
                {
                    ViewBag.error = 1;
                    ViewBag.errorMsg = ret.Msg;
                    return Content("<script>alert('" + ret.Msg + "');history.go(-1);</script>");

                }
            }
        }
        public ActionResult CreateEWM(int orgFlowID, int prodID, int traceID)
        {
            ViewBag.orgFlowID = orgFlowID;
            ViewBag.prodID = prodID;
            ViewBag.traceID = traceID;

            return View();
        }
        [HttpPost]
        public ActionResult CreateEWM()
        {
            int intBool = 0;
            if (!int.TryParse(Request["Prod_Code_Start"].ToString(), out  intBool) || !int.TryParse(Request["Prod_Code_End"].ToString(), out intBool))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "请输入正确的码段信息，无法进行激活！";
                return Content("<script>alert('" + ViewBag.errorMsg + "');history.go(-1);</script>");
            }
            else
            {
                var user = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user;
                LinqModel.Trace_Info model = new LinqModel.Trace_Info();
                model.Org_Flow_ID = int.Parse(Request["orgFlowID"]);
                model.Prod_Code_End = int.Parse(Request["Prod_Code_End"]);
                model.Prod_Code_Start = int.Parse(Request["Prod_Code_Start"]);
                model.Prod_ID = int.Parse(Request["prodID"]);
                model.Prod_Code_Before = new DAL.Products().GetModel(int.Parse(Request["prodID"])).Adv_Code;
                model.Rec_Time = Common.Argument.Public.GetDateTimeNow();
                model.Trace_ID_Up = int.Parse(Request["traceID"]);
                model.FlowOver = false;
                if (new DAL.Trace_Info().IsOverMax((int)model.Prod_ID, (int)model.Prod_Code_Start, (int)model.Prod_Code_End))
                {
                    ViewBag.error = 1;
                    ViewBag.errorMsg = "此段二维码已超过最大生成数量，无法进行激活！";
                    return Content("<script>alert('" + ViewBag.errorMsg + "');history.go(-1);</script>");
                }
                else
                {
                    if (new DAL.Trace_Info().IsUsed((int)model.Prod_ID, (int)model.Prod_Code_Start, (int)model.Prod_Code_End))
                    {
                        ViewBag.error = 1;
                        ViewBag.errorMsg = "此段二维码有使用过，无法进行激活！";
                        return Content("<script>alert('" + ViewBag.errorMsg + "');history.go(-1);</script>");
                    }
                    else
                    {
                        model.Flow_Num = "ZS_" + model.Rec_Time.ToString("yyyyMMddHHmmssfff");

                        #region 组织XML字段信息
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<root>");
                        sb.Append("<orgFlowID value='" + Request["orgFlowID"] + "'/>");
                        sb.Append("<userInfo ID='" + user.ID + "' name='" + user.UserName + "' userCode='" + user.User_Code + "'/>");
                        sb.Append("<recTime value='" + model.Rec_Time.ToString("yyyyMMddHHmmssfff") + "'/>");
                        sb.Append("<info InfoID='0' InfoName='开始二维码编号流水号' InfoValue='" + model.Prod_Code_Start + "' />");
                        sb.Append("<info InfoID='0' InfoName='结束二维码编号流水号' InfoValue='" + model.Prod_Code_End + "' />");
                        if (Request["upFlowOver"] == "true")
                        {
                            sb.Append("<upFlowOver value='true' />");
                        }
                        else
                        {
                            sb.Append("<upFlowOver value='false' />");
                        }
                        sb.Append("</root>");
                        #endregion

                        model.Trace_Info_Value = XElement.Parse(sb.ToString());
                        Common.Argument.RetResult ret = new DAL.Trace_Info().Add(model);
                        if (ret.IsSuccess)
                        {
                            //RedirectToAction("Index");
                            return Content("<script>alert('" + ret.Msg + "');this.location = '/ManageTrace_Info/List?orgFlowID=" + Request["orgFlowID"] + "&prodID=" + Request["prodID"] + "&txtSearch=" + model.Flow_Num + "';</script>");
                        }
                        else
                        {
                            ViewBag.error = 1;
                            ViewBag.errorMsg = ret.Msg;
                            return Content("<script>alert('" + ret.Msg + "');history.go(-1);</script>");
                        }
                    }
                }
            }
        }
        public ActionResult ListFlow(int? page)
        {
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            ViewBag.search = 0;
            ViewBag.txtSearch = Request.QueryString["txtSearch"];
            if (Request.QueryString["txtSearch"] == null)
            {
                ViewBag.txtSearch = "";
            }
            else
            {
                ViewBag.search = 1;
            }
            ViewBag.timeS = Request.QueryString["timeS"];
            ViewBag.timeE = Request.QueryString["timeE"];
            if (Request.QueryString["timeS"] == null)
            {
                ViewBag.timeS = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            if (Request.QueryString["timeE"] == null)
            {
                ViewBag.timeE = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            ViewBag.IsOver = Request.QueryString["IsOver"];
            if (Request.QueryString["IsOver"] == null)
            {
                ViewBag.IsOver = "false";
            }
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            var products = new DAL.Products().GetAllwithORGID(orgID);
            ViewBag.products = products;
            int prodID = 0;
            if (Request.QueryString["products"] != null)
            {
                ViewBag.productCurrent = Request.QueryString["products"];
                ViewBag.flows = new DAL.Org_Flow().GetList("", orgID, int.Parse(ViewBag.productCurrent), 1, int.MaxValue);
                int.TryParse(Request.QueryString["products"], out prodID);
            }
            else
            {
                if (products.Count > 0)
                {
                    ViewBag.flows = new DAL.Org_Flow().GetList("", orgID, products[0].ID, 1, int.MaxValue);
                    prodID = products[0].ID;
                }
            }
            int orgFlowID = 0;
            if (Request.QueryString["flows"] != null)
            {
                ViewBag.flowCurrent = Request.QueryString["flows"];
                int.TryParse(Request.QueryString["flows"], out orgFlowID);
            }
            else
            {
                if (ViewBag.flows != null && ((PagedList.PagedList<LinqModel.View_Org_Flow>)ViewBag.flows).Count > 0)
                {
                    orgFlowID = ((PagedList.PagedList<LinqModel.View_Org_Flow>)ViewBag.flows)[0].Org_Flow_ID;
                }
            }
            IPagedList<LinqModel.View_Trace_Info> list = new DAL.Trace_Info().GetListFlow(bool.Parse(ViewBag.IsOver), orgID, ViewBag.txtSearch, prodID, orgFlowID, ViewBag.timeS, ViewBag.timeE, pIndex, 10);
            if (list.Count > 0)
            {
                ViewBag.title2 = "【" + list[0].Name + "】产品【" + list[0].Flow_Name + "】流程";
            }
            else
            {
                ViewBag.title2 = "";
            }
            return View(list);
        }
        public ActionResult ListFlowEWM(int? page)
        {
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            ViewBag.search = 0;
            ViewBag.txtSearch = Request.QueryString["txtSearch"];
            if (Request.QueryString["txtSearch"] == null)
            {
                ViewBag.txtSearch = "";
            }
            else
            {
                ViewBag.search = 1;
            }
            ViewBag.timeS = Request.QueryString["timeS"];
            ViewBag.timeE = Request.QueryString["timeE"];
            if (Request.QueryString["timeS"] == null)
            {
                ViewBag.timeS = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            if (Request.QueryString["timeE"] == null)
            {
                ViewBag.timeE = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            }
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            string txtSearch = ViewBag.txtSearch;
            var result = new DAL.Trace_Info().GetListFlowEWM(orgID, txtSearch, ViewBag.timeS, ViewBag.timeE, pIndex, 10);
            return View(result);
        }

        public ActionResult GetOrgFlow()
        {
            int prodID = 0;
            int.TryParse(Request.QueryString["prodID"], out prodID);
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            List<LinqModel.View_Org_Flow> list = new DAL.Org_Flow().GetList("", orgID, prodID, 1, int.MaxValue).ToList();
            if (list == null)
            {
                list = new List<LinqModel.View_Org_Flow>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InfoAnylasis(long ewm, int prodID)
        {
            var modelFlow = new DAL.Trace_Info().GetModelByEwm(ewm, prodID);
            if (modelFlow != null && modelFlow.Trace_ID_List != null)
            {
                List<int> ids = new List<int>();
                string[] strs = modelFlow.Trace_ID_List.Split('|');

                foreach (var m in strs)
                {
                    if (!string.IsNullOrEmpty(m))
                    {
                        ids.Add(int.Parse(m));
                    }
                }
                List<LinqModel.Trace_Info_Pics_New> listCycle = new List<LinqModel.Trace_Info_Pics_New>();
                List<LinqModel.Trace_Info_Pics_New> list = new DAL.Trace_Info().GetList(ids, out listCycle);
                ViewBag.listCycle = listCycle;
                return View(list);
            }
            else
            {
                return View();
            }
        }

        public ActionResult SearchPoint()
        {

            ViewBag.timeS = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");

            ViewBag.timeE = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");

            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            var products = new DAL.Products().GetAllwithORGID(orgID);
            ViewBag.products = products;
            if (products.Count > 0)
            {
                ViewBag.flows = new DAL.Org_Flow().GetList("", orgID, products[0].ID, 1, int.MaxValue);
            }

            return View();
        }

        public ActionResult GetSearchPoint()
        {
            int orgFlowID = 0;
            int.TryParse(Request.QueryString["orgFlowID"], out orgFlowID);
            var list = new DAL.Org_Info().GetSearchPoint(orgFlowID);
            StringBuilder sb = new StringBuilder();
            foreach (var m in list)
            {
                sb.Append(Common.Argument.Public.GetDataTypeHtmlSearch(m.Data_Type, m.Info_Name, m.Info_ID.ToString(), false, m.Data_Type_Value));
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSearchPointResult(int prodID, int orgFlowID, string timeS, string timeE, bool isOver)
        {
            StringBuilder sb = new StringBuilder();
            string strText = Server.UrlDecode(Request.QueryString["strText"]);
            string strCheck = Server.UrlDecode(Request.QueryString["strCheck"]);
            string strRadio = Server.UrlDecode(Request.QueryString["strRadio"]);
            string strSelect = Server.UrlDecode(Request.QueryString["strSelect"]);
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            var list = new DAL.Trace_Info().GetListSearchPoint(isOver, prodID, orgFlowID, timeS, timeE, strText, strCheck, strRadio, strSelect);
            int count = 0;
            foreach (var m in list)
            {
                count++;
                string classname = "fafafb";
                if (count % 2 == 0)
                { classname = "ecf3fa"; }
                if ((bool)m.FlowOver)
                {
                    sb.Append("<tr onclick=\"RowClick(" + m.Trace_ID + ")\" id=\"" + m.Trace_ID + "\" style=\"background-color: #" + classname + "\"><td id=\"ext_" + m.Trace_ID + "\">+</td><td>" + m.Flow_Num + "</td><td class=\"hidden-480\">" + m.Rec_Time.ToString("yyyy-MM-dd HH:mm:ss") + "</td><td class=\"hidden-480\">已结束</td><td class=\"hidden-480\"><a href=\"javascript:void(0);\">查看详情</a></td></tr>");
                    sb.Append("<tr style=\"display:none; \" id=\"detail_" + m.Trace_ID + "\"><td colspan=\"10\"><div style=\"margin-left:25px;\"><p><table id=\"dayin1\" border=\"1\"  style=\"width:98%\"><tr><th width=\"80\">单据编号：</th><td colspan=\"2\">" + m.Flow_Num + "</td></tr><tr><th>流程名称：</th><td>" + m.Flow_Name + " </td><td style=\"vertical-align:middle; text-align:center;\" rowspan=\"2\"><img src=\"/Public/ShowImg?ewm=" + m.Flow_Num + "\" width=\"80\" height=\"80\" /></td></tr><tr><th>产品名称：</th><td>" + m.Name + "</td></tr><tr><th> 录入时间：</th><td colspan=\"2\">" + m.Rec_Time.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr><tr><th> 是否结束：</th><td colspan=\"2\">已结束</td></tr><tr><th> 录入详情：</th><td colspan=\"2\">");

                    if (m.Trace_Info_Value.Descendants("info").Count() > 0)
                    {
                        foreach (var mt in m.Trace_Info_Value.Descendants("info"))
                        {
                            sb.Append(mt.Attribute("InfoName").Value + "：" + mt.Attribute("InfoValue").Value + "<br />");
                        }

                    }
                    sb.Append("</td></tr>");
                    sb.Append("<tr><th style=\"vertical-align:top;\">操作图片：</th><td colspan=\"2\">");


                    if (m.Trace_Info_Value.Descendants("img").Count() > 0)
                    {
                        foreach (var mt in m.Trace_Info_Value.Descendants("img"))
                        {
                            sb.Append("<img src=\"" + mt.Attribute("InfoValue").Value + "\" width=\"300\" height=\"200\" />");
                        }
                    }
                    sb.Append("</td> </tr>");

                    sb.Append("</table></p></div></td></tr>");
                }
                else
                {
                    sb.Append("<tr onclick=\"RowClick(" + m.Trace_ID + ")\" id=\"" + m.Trace_ID + "\" style=\"background-color: #" + classname + "\"><td id=\"ext_" + m.Trace_ID + "\">+</td><td>" + m.Flow_Num + "</td><td class=\"hidden-480\">" + m.Rec_Time.ToString("yyyy-MM-dd HH:mm:ss") + "</td><td class=\"hidden-480\">进行中</td><td class=\"hidden-480\"><a href=\"javascript:void(0);\">查看详情</a></td></tr>");
                    sb.Append("<tr style=\"display:none; \" id=\"detail_" + m.Trace_ID + "\"><td colspan=\"10\"><div style=\"margin-left:25px;\"><p><table id=\"dayin1\" border=\"1\"  style=\"width:98%\"><tr><th width=\"80\">单据编号：</th><td colspan=\"2\">" + m.Flow_Num + "</td></tr><tr><th>流程名称：</th><td>" + m.Flow_Name + " </td><td style=\"vertical-align:middle; text-align:center;\" rowspan=\"2\"><img src=\"/Public/ShowImg?ewm=" + m.Flow_Num + "\" width=\"80\" height=\"80\" /></td></tr><tr><th>产品名称：</th><td>" + m.Name + "</td></tr><tr><th> 录入时间：</th><td colspan=\"2\">" + m.Rec_Time.ToString("yyyy-MM-dd HH:mm:ss") + "</td></tr><tr><th> 是否结束：</th><td colspan=\"2\">进行中</td></tr><tr><th> 录入详情：</th><td colspan=\"2\">");

                    if (m.Trace_Info_Value.Descendants("info").Count() > 0)
                    {
                        foreach (var mt in m.Trace_Info_Value.Descendants("info"))
                        {
                            if (mt.Attribute("InfoValue") != null)
                            {
                                sb.Append("<label><b>" + mt.Attribute("InfoName").Value + "：</b>" + mt.Attribute("InfoValue").Value + "</label><br />");
                            }
                            else
                            {
                                sb.Append(" <label><b>" + mt.Attribute("InfoName").Value + "：</b>");
                                foreach (var mtt in mt.Descendants("value"))
                                {
                                    sb.Append("<label>" + mtt.Value + " ，</label>");
                                }

                                sb.Append(" </label><br />");
                            }
                        }

                    }
                    sb.Append("</td></tr>");
                    sb.Append("<tr><th style=\"vertical-align:top;\">操作图片：</th><td colspan=\"2\">");


                    if (m.Trace_Info_Value.Descendants("img").Count() > 0)
                    {
                        foreach (var mt in m.Trace_Info_Value.Descendants("img"))
                        {
                            sb.Append("<img src=\"" + mt.Attribute("InfoValue").Value + "\" width=\"300\" height=\"200\" />");
                        }
                    }
                    sb.Append("</td> </tr>");

                    sb.Append("</table></p></div></td></tr>");
                }
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BaoZhuang()
        {
            if (Request.QueryString["clear"] != null && Request.QueryString["clear"] == "1")
            {
                Session["BaoZhuang_wai"] = null;
                Session["BaoZhuang_dgHidden"] = null;
                Session["BaoZhuang_es"] = null;
                Session["BaoZhuang_ee"] = null;
            }
            ViewBag.url = System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=";
            ViewBag.wai = Session["BaoZhuang_wai"] == null ? "" : Session["BaoZhuang_wai"].ToString().Replace(ViewBag.url, "");
            ViewBag.dgHidden = Session["BaoZhuang_dgHidden"] == null ? "" : Session["BaoZhuang_dgHidden"].ToString().Replace(ViewBag.url, "");
            ViewBag.es = Session["BaoZhuang_es"] == null ? "" : Session["BaoZhuang_es"].ToString().Replace(ViewBag.url, "");
            ViewBag.ee = Session["BaoZhuang_ee"] == null ? "" : Session["BaoZhuang_ee"].ToString().Replace(ViewBag.url, "");
            if (Request.QueryString["rdm"] == "1")
            {
                ViewBag.wai = Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
            }
            else if (Request.QueryString["rdm"] == "2")
            {
                ViewBag.dgHidden += "$" + Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
            }
            else if (Request.QueryString["rdm"] == "31")
            {
                ViewBag.es += Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
            }
            else if (Request.QueryString["rdm"] == "32")
            {
                ViewBag.ee += Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
            }
            string dg = ViewBag.dgHidden;
            ViewBag.dg = dg.Replace("$", "\n");
            if (string.IsNullOrEmpty(dg))
            {
                ViewBag.sCount = "0";
            }
            else
            {
                ViewBag.sCount = dg.Split('$').Length - 1;
            }
            ViewBag.BaoZhuangCount = System.Configuration.ConfigurationManager.AppSettings["BaoZhuangCount"];
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;

            #region 数据存到Session
            Session["BaoZhuang_wai"] = ViewBag.wai;
            Session["BaoZhuang_dgHidden"] = ViewBag.dgHidden;
            Session["BaoZhuang_es"] = ViewBag.es;
            Session["BaoZhuang_ee"] = ViewBag.ee;
            #endregion

            return View(new DAL.Org_BaoZhuang().GetAllByOrgID(orgID, "", 1, int.MaxValue));
        }
        [HttpPost]
        public ActionResult BaoZhuang(LinqModel.Org_BaoZhuang_Data modelTemp)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            bool check = true;
            long ewmParentNum = 0;
            List<long> ewmNums = new List<long>();
            StringBuilder sbewmNums = new StringBuilder();
            int prodID = 0;
            #region 判断码规则
            string ewm = Request["EWM_ParentNum"].Replace(System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=", "");
            List<LinqModel.Code_Scheme> listCS = new DAL.Code_Scheme().GetAll();
            int csID = 0;
            int point = 0;
            for (int i = 0; i < listCS.Count; i++)
            {
                if (ewm.StartsWith(listCS[i].Prefix))
                {
                    point = i;
                    csID = listCS[i].Scheme_ID;
                    break;
                }
            }
            List<LinqModel.Code_Seg_Info> listCSI = new DAL.Code_Seg_Info().GetList(csID);
            LinqModel.Products modelProd = new LinqModel.Products();
            string[] strs = ewm.Split(char.Parse(listCS[point].Separator));
            if (!long.TryParse(strs[listCSI.FirstOrDefault(m => m.Meaning == "流水号").Seg_No], out  ewmParentNum))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "请扫描正确的追溯码信息！";
                return Content("<script>alert('" + ViewBag.errorMsg + "');history.go(-1);</script>");
            }
            else
            {
                if (strs[listCSI.FirstOrDefault(m => m.Meaning == "产品/用途ID").Seg_No].StartsWith("1"))
                {
                    prodID = int.Parse(strs[listCSI.FirstOrDefault(m => m.Meaning == "产品/用途ID").Seg_No].Substring(1));
                }
                modelProd = new DAL.Products().GetModel(prodID);

                StringBuilder ewmStr = new StringBuilder();
                #region 判断是分段扫描还是单个扫描
                if (Request["sType"] == "d")
                {
                    ewmStr = ewmStr.Append(Request["EWM_Num"]);
                }
                else
                {
                    long numS = 0, numE = 0;
                    if (!long.TryParse(Request["EWM_Start"].Replace(System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=", "").Split(char.Parse(listCS[point].Separator))[listCSI.FirstOrDefault(m => m.Meaning == "流水号").Seg_No], out  numS))
                    {
                        check = false;
                    }
                    if (!long.TryParse(Request["EWM_End"].Replace(System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=", "").Split(char.Parse(listCS[point].Separator))[listCSI.FirstOrDefault(m => m.Meaning == "流水号").Seg_No], out  numE))
                    {
                        check = false;
                    }
                    if (numE < numS || (numE - numS) > int.Parse(System.Configuration.ConfigurationManager.AppSettings["BaoZhuangCount"]))
                    {
                        check = false;
                    }
                    else
                    {
                        for (long i = numS; i <= numE; i++)
                        {
                            ewmStr.Append(System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=" + modelProd.Adv_Code + i + "\r\n");
                        }
                    }
                }
                #endregion
                string[] ewms = ewmStr.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=", "").Replace("\r\n", "$").Split('$');
                foreach (var mm in ewms)
                {
                    if (!string.IsNullOrEmpty(mm))
                    {
                        strs = mm.Split(char.Parse(listCS[point].Separator));
                        long ewmNum = 0;
                        if (long.TryParse(strs[listCSI.FirstOrDefault(m => m.Meaning == "流水号").Seg_No], out  ewmNum))
                        {
                            ewmNums.Add(ewmNum);
                            sbewmNums.Append(ewmNum + ",");
                        }
                        else
                        {
                            check = false;
                            break;
                        }
                    }
                }
            }
            #endregion

            if (!check)
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "请扫描正确的追溯码信息！";
                return Content("<script>alert('" + ViewBag.errorMsg + "');history.go(-1);</script>");
            }
            else
            {
                #region 包装规则判断
                List<LinqModel.Org_BaoZhuang_Data> listdata = new List<LinqModel.Org_BaoZhuang_Data>();
                var modelDataParent = new LinqModel.Org_BaoZhuang_Data();
                modelDataParent.EWM_Num = 0;
                modelDataParent.EWM_ParentNum = ewmParentNum;
                modelDataParent.OrgID = orgID;
                modelDataParent.Prod_Code_Before = modelProd.Adv_Code;
                modelDataParent.Prod_ID = prodID;
                listdata.Add(modelDataParent);
                foreach (var temp in ewmNums)
                {
                    if (temp > 0)
                    {
                        var modelData = new LinqModel.Org_BaoZhuang_Data();
                        modelData.EWM_Num = temp;
                        modelData.EWM_ParentNum = ewmParentNum;
                        modelData.OrgID = orgID;
                        modelData.Prod_Code_Before = modelProd.Adv_Code;
                        modelData.Prod_ID = prodID;
                        listdata.Add(modelData);
                    }
                }
                Common.Argument.RetResult ret = new DAL.Org_BaoZhuang_Data().Add(listdata);
                #endregion
                if (!ret.IsSuccess)
                {
                    ViewBag.error = 1;
                    ViewBag.errorMsg = ret.Msg;
                    return Content("<script>alert('" + ViewBag.errorMsg + "');history.go(-1);</script>");
                }
                else
                {
                    var user = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user;
                    LinqModel.Trace_Info model = new LinqModel.Trace_Info();
                    model.Org_Flow_ID = 0;
                    model.Prod_Code_End = ewmParentNum;
                    model.Prod_Code_Start = ewmParentNum;
                    model.Prod_ID = prodID;
                    model.Prod_Code_Before = modelProd.Adv_Code;
                    model.Rec_Time = Common.Argument.Public.GetDateTimeNow();
                    model.Trace_ID_Up = 0;
                    model.FlowOver = false;
                    model.Flow_Num = "BZ_" + model.Rec_Time.ToString("yyyyMMddHHmmssfff");

                    #region 组织XML字段信息
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<root>");
                    sb.Append("<userInfo ID='" + user.ID + "' name='" + user.UserName + "' userCode='" + user.User_Code + "'/>");
                    sb.Append("<recTime value='" + model.Rec_Time.ToString("yyyyMMddHHmmssfff") + "'/>");
                    sb.Append("<info InfoID='0' InfoName='产品ID' InfoValue='" + prodID + "' />");
                    sb.Append("<info InfoID='0' InfoName='码前缀' InfoValue='" + modelProd.Adv_Code + "' />");
                    sb.Append("<info InfoID='0' InfoName='外包装码' InfoValue='" + ewmParentNum + "' />");
                    sb.Append("<info InfoID='0' InfoName='内包装码' InfoValue='" + sbewmNums.ToString() + "' />");
                    sb.Append("<info InfoID='0' InfoName='包装规格' InfoValue='" + new DAL.Org_BaoZhuang().GetModel(int.Parse(Request["Org_BaoZhuang"])).Name + "' />");
                    sb.Append("<upFlowOver value='false' />");
                    var listOrgData = new DAL.Org_BaoZhuang_GuiGe().GetAllByOrgID(orgID, int.Parse(Request["Org_BaoZhuang"]), "", 1, int.MaxValue);
                    foreach (var tempGuiGe in listOrgData)
                    {
                        sb.Append("<info InfoID='" + tempGuiGe.ID + "'  InfoName='" + tempGuiGe.NameGuiGe + "' InfoValue='" + Request[tempGuiGe.ID.ToString()] + "' />");
                    }

                    sb.Append("</root>");
                    #endregion

                    model.Trace_Info_Value = XElement.Parse(sb.ToString());
                    ret = new DAL.Trace_Info().Add(model);
                    if (ret.IsSuccess)
                    {
                        //RedirectToAction("Index");
                        Session["BaoZhuang_wai"] = "";
                        Session["BaoZhuang_dgHidden"] = "";
                        Session["BaoZhuang_es"] = "";
                        Session["BaoZhuang_ee"] = "";
                        return Content("<script>alert('" + ret.Msg + "');this.location = '/ManageTrace_Info/BaoZhuang';</script>");
                    }
                    else
                    {
                        ViewBag.error = 1;
                        ViewBag.errorMsg = ret.Msg;
                        return Content("<script>alert('" + ret.Msg + "');history.go(-1);</script>");
                    }
                }
            }
        }
        public ActionResult GetGuiGe()
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            int bzID = 0;
            int.TryParse(Request.QueryString["bzID"], out bzID);
            var list = new DAL.Org_BaoZhuang_GuiGe().GetAllByOrgID(orgID, bzID, "", 1, int.MaxValue);
            StringBuilder sb = new StringBuilder();
            foreach (var m in list)
            {
                sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + m.NameGuiGe + "  ：</label><div class=\"controls\"><input  type=\"text\" class=\"m-wrap\" id=\"" + m.ID + "\" name=\"" + m.ID + "\" value=\"" + m.ValueGuiGe + "\" /> </div></div>");
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaiMaRuKu()
        {
            if (Request.QueryString["clear"] != null && Request.QueryString["clear"] == "1")
            {
                Session["BaoZhuang_wai"] = null;
                Session["BaoZhuang_dgHidden"] = null;
                Session["BaoZhuang_es"] = null;
                Session["BaoZhuang_ee"] = null;
            }
            ViewBag.url = System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=";
            ViewBag.wai = Session["BaoZhuang_wai"] == null ? "" : Session["BaoZhuang_wai"].ToString().Replace(ViewBag.url, "");
            ViewBag.dgHidden = Session["BaoZhuang_dgHidden"] == null ? "" : Session["BaoZhuang_dgHidden"].ToString().Replace(ViewBag.url, "");
            ViewBag.es = Session["BaoZhuang_es"] == null ? "" : Session["BaoZhuang_es"].ToString().Replace(ViewBag.url, "");
            ViewBag.ee = Session["BaoZhuang_ee"] == null ? "" : Session["BaoZhuang_ee"].ToString().Replace(ViewBag.url, "");
            if (Request.QueryString["rdm"] == "1")
            {
                ViewBag.wai = Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
            }
            else if (Request.QueryString["rdm"] == "2")
            {
                ViewBag.dgHidden += "$" + Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
                ViewBag.es = "";
                ViewBag.ee = "";
            }
            else if (Request.QueryString["rdm"] == "31")
            {
                ViewBag.es += Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
                ViewBag.dgHidden = "";
            }
            else if (Request.QueryString["rdm"] == "32")
            {
                ViewBag.ee += Request.QueryString["txtSearch"].Replace(ViewBag.url, "");
                ViewBag.dgHidden = "";
            }
            string dg = ViewBag.dgHidden;
            ViewBag.dg = dg.Replace("$", "\n");
            if (string.IsNullOrEmpty(dg))
            {
                ViewBag.sCount = "0";
            }
            else
            {
                ViewBag.sCount = dg.Split('$').Length - 1;
            }
            ViewBag.BaoZhuangCount = System.Configuration.ConfigurationManager.AppSettings["BaoZhuangCount"];
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;

            #region 数据存到Session
            Session["BaoZhuang_wai"] = ViewBag.wai;
            Session["BaoZhuang_dgHidden"] = ViewBag.dgHidden;
            Session["BaoZhuang_es"] = ViewBag.es;
            Session["BaoZhuang_ee"] = ViewBag.ee;
            #endregion
            return View();
        }
        [HttpPost]
        public ActionResult PaiMaRuKu(LinqModel.RuKu_PaiMa modelTemp)
        {
            var modelUser = (Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]];
            int orgID = (int)modelUser.user.Org_ID;
            int userID = modelUser.user.ID;
            LinqModel.RuKu_PaiMa modelRuKu = new LinqModel.RuKu_PaiMa();
            modelRuKu.From_Org_Name = Request["txtSelectName"];
            modelRuKu.Org_ID = orgID;
            modelRuKu.User_ID = userID;
            modelRuKu.TimeAdd = Common.Argument.Public.GetDateTimeNow();
            if (string.IsNullOrEmpty(modelRuKu.From_Org_Name))
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = "请输入企业名称！";
                return Content("<script>alert('" + ViewBag.errorMsg + "');history.go(-1);</script>");
            }
            else
            {
                LinqModel.Trace_Info model = new LinqModel.Trace_Info();
                model.Org_Flow_ID = 0;
                model.Prod_Code_End = 0;
                model.Prod_Code_Start = 0;
                model.Prod_ID = 0;
                model.Prod_Code_Before = "";
                model.Rec_Time = Common.Argument.Public.GetDateTimeNow();
                model.Trace_ID_Up = 0;
                model.FlowOver = false;
                model.Flow_Num = "PMRK_" + model.Rec_Time.ToString("yyyyMMddHHmmssfff");

                #region 组织XML字段信息
                StringBuilder sb = new StringBuilder();
                sb.Append("<root>");
                sb.Append("<userInfo ID='" + userID + "' name='" + modelUser.user.UserName + "' userCode='" + modelUser.user.User_Code + "'/>");
                sb.Append("<recTime value='" + model.Rec_Time.ToString("yyyyMMddHHmmssfff") + "'/>");
                if (!string.IsNullOrEmpty(Request["EWM_Start"]))
                {
                    sb.Append("<info InfoID='0' InfoName='CodeStart' InfoValue='" + Request["EWM_Start"] + "' />");
                    sb.Append("<info InfoID='0' InfoName='CodeEnd' InfoValue='" + Request["EWM_End"] + "' />");
                }
                else
                {
                    sb.Append("<info InfoID='0' InfoName='EWM_Num' InfoValue='" + Request["EWM_Num_Hidden"] + "' />");
                }
                sb.Append("<upFlowOver value='false' />");
                sb.Append("</root>");
                #endregion

                model.Trace_Info_Value = XElement.Parse(sb.ToString());
                modelRuKu.Infos = model.Trace_Info_Value;
                Common.Argument.RetResult ret = new DAL.RuKu_PaiMa().Add(modelRuKu);
                new DAL.Trace_Info().Add(model);
                if (ret.IsSuccess)
                {
                    //RedirectToAction("Index");
                    Session["BaoZhuang_wai"] = "";
                    Session["BaoZhuang_dgHidden"] = "";
                    Session["BaoZhuang_es"] = "";
                    Session["BaoZhuang_ee"] = "";
                    return Content("<script>alert('" + ret.Msg + "');this.location = '/ManageTrace_Info/PaiMaRuKu';</script>");
                }
                else
                {
                    ViewBag.error = 1;
                    ViewBag.errorMsg = ret.Msg;
                    return Content("<script>alert('" + ret.Msg + "');history.go(-1);</script>");
                }
            }
        }
    }
}