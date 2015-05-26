using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_Anylasis.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Head(int prodID, int usageID)
        {
            ViewBag.prodID = prodID;
            ViewBag.usageID = usageID;
            return View();
        }
        /// <summary>
        /// 主页(乔典修正，修复二维码格式不正确导致服务器报错的问题)
        /// </summary>
        /// <param name="ewm">二维码</param>
        /// <returns></returns>
        public ActionResult Index(string ewm)
        {
            //LinqModel.Organization ona = new DAL.Organization().GetModel(7);
            //string imgUrl = Common.Argument.Public.GetFirstImages(ona.Intro);
            //string content = Common.Argument.Public.HtmlClear(ona.Intro);
            try
            {
                //ewm = "1.2.156.326.188.0143.210403.1020.114.1";
                int csID = 0;
                int point = 0;
                List<LinqModel.Code_Scheme> listCS = new DAL.Code_Scheme().GetAll();

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
                ViewBag.orgID = 0;
                ViewBag.prodID = 0;
                ViewBag.usageID = 0;
                ViewBag.ewmNum = 0;
                ViewBag.ewm = ewm;

                string[] strs = ewm.Split(char.Parse(listCS[point].Separator));

                //追溯码
                if (strs.Length == listCS[point].SN_Position)
                {
                    int ID = 0;
                    ViewBag.prodID = 0;
                    ViewBag.usageID = 0;
                    int orgID = int.Parse(strs[listCSI.FirstOrDefault(m => m.Meaning == "企业ID").Seg_No]);
                    ViewBag.orgID = orgID;
                    if (strs[listCSI.FirstOrDefault(m => m.Meaning == "产品/用途ID").Seg_No].StartsWith("1"))
                    {
                        ID = int.Parse(strs[listCSI.FirstOrDefault(m => m.Meaning == "产品/用途ID").Seg_No].Substring(1));
                        ViewBag.prodID = ID;
                    }
                    else if (strs[listCSI.FirstOrDefault(m => m.Meaning == "产品/用途ID").Seg_No].StartsWith("2"))
                    {
                        ID = int.Parse(strs[listCSI.FirstOrDefault(m => m.Meaning == "产品/用途ID").Seg_No].Substring(1));
                        ViewBag.usageID = ID;
                    }
                    if (!new DAL.Products().Exist(ID, ewm, orgID))
                    {
                        ViewBag.prodID = 0;
                        ViewBag.usageID = 0;
                        ViewBag.ewmResult = "ERROR";
                        return View();
                    }

                    ViewBag.ewmNum = long.Parse(strs[listCSI.FirstOrDefault(m => m.Meaning == "流水号").Seg_No]);
                    if (ViewBag.usageID == 0)
                    {
                        new DAL.Trace_Rec().Scan(ewm);
                    }
                }
                //其它不支持解析
                else
                {
                    ViewBag.ewmResult = "ERROR";
                }
            }
            catch
            {
                ViewBag.ewmResult = "ERROR";
            }
            return View();
        }
        /// <summary>
        /// 实时视频
        /// </summary>
        /// <returns></returns>
        public ActionResult Video(int prodID)
        {
            ViewBag.VideoUrl = null;
            string url = null;
            if (new DAL.Products().VideoUrlById(prodID, out url))
            {
                ViewBag.VideoUrl = url;
            }
            return View();
        }
        [HttpGet]
        public ActionResult Product(int prodID, int usageID)
        {
            if (prodID > 0)
            {
                var model = new DAL.Products().GetModel(prodID);
                ViewBag.description = model.Description;
                ViewBag.name = model.Name;
                ViewBag.ewm = Request.QueryString["ewm"];
                ViewBag.prodID = prodID;
                ViewBag.urlProd = model.EWMUrl;
                ViewBag.videoUrl = model.VideoUrl;
                ViewBag.videoUrl = "aaa";
                if (string.IsNullOrEmpty(model.EWMUrl))
                {
                    ViewBag.urlProd = "#";// System.Configuration.ConfigurationManager.AppSettings["UrlSite"] + "/Products/ProInfo?oid=" + model.Org_ID + "&pid=" + model.ID;
                }
            }
            else if (usageID > 0)
            {
                ViewBag.usageID = usageID;
                ViewBag.ewm = Request.QueryString["ewm"];
            }
            return View();
        }
        [HttpPost]
        public ActionResult Product(string ewm, string pass)
        {
            string pw = Common.Argument.Public.GetEWMPass(System.Configuration.ConfigurationManager.AppSettings["urlZhuiSuNew"], ewm);
            if (pass == pw)
            {
                return RedirectToAction("Pass", new { ewm = ewm, pass = pass });
            }
            else
            {
                return RedirectToAction("ErrorPass", new { ewm = ewm });
            }
        }
        public ActionResult Flow(long ewmNum, int prodID)
        {
            var modelFlow = new DAL.Trace_Info().GetModelByEwm(ewmNum, prodID);
            if (modelFlow == null)//如果不是流程追溯，获取包装信息
            {
                #region 包装信息
                var model = new DAL.Trace_Info().GetModelByEwm(ewmNum, prodID, 0);
                if (model != null && model.Trace_ID > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div id=\"products\" class=\"content-section\"><div class=\"container\"><div class=\"row\"><div class=\" text-center\"><h1 class=\"section-title\">包装信息</h1></div> </div><div class=\"row col-md-12 col-sm-12\">");
                    if (model.Trace_Info_Value.Descendants("info").Count() > 0)
                    {
                        sb.Append("<p style=\"clear:both; padding:10px;\"><table>");
                        var mgg = model.Trace_Info_Value.Descendants("info").FirstOrDefault(m => m.Attribute("InfoName").Value == "包装规格");
                        sb.Append("<tr><th style=\"text-align:left;\">" + mgg.Attribute("InfoName").Value + " ：</th><td style=\"text-align:left;\">" + mgg.Attribute("InfoValue").Value + "</td></tr>");
                        foreach (var mt in model.Trace_Info_Value.Descendants("info"))
                        {
                            try
                            {
                                if (int.Parse(mt.Attribute("InfoID").Value) > 0)
                                {
                                    sb.Append("<tr><th style=\"text-align:left;\">" + mt.Attribute("InfoName").Value + " ：</th><td style=\"text-align:left;\">" + mt.Attribute("InfoValue").Value + "</td></tr>");
                                }
                            }
                            catch { }
                        }
                        //string[] ewms = model.Trace_Info_Value.Descendants("info").FirstOrDefault(m => m.Attribute("InfoName").Value == "内包装码").Attribute("InfoValue").Value.Split(',');
                        var listEwms = new DAL.Org_BaoZhuang_Data().GetList(prodID, long.Parse(model.Prod_Code_Start.ToString()));
                        sb.Append("<tr><th style=\"text-align:left;\">内包装码 ：</th><td style=\"text-align:left;\">");
                        foreach (var tm in listEwms)
                        {
                            sb.Append("<a href='" + System.Configuration.ConfigurationManager.AppSettings["AnylasisURL"] + "?ewm=" + model.Prod_Code_Before + tm.EWM_Num + "'>" + model.Prod_Code_Before + tm.EWM_Num + "</a><br/>");
                        }
                        sb.Append("</td></tr>");
                        sb.Append("</table></p>");
                    }
                    sb.Append("</div></div></div>");
                    ViewBag.BaoZhuang = sb.ToString();
                }
                #endregion
            }
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
        [HttpGet]
        public ActionResult Contact(int orgID, long ewmNum, int prodID, string ewm)
        {
            var model = new DAL.Organization().GetModel(orgID);
            ViewBag.prodID = prodID;
            ViewBag.orgID = orgID;
            ViewBag.ewmNum = ewmNum;
            ViewBag.ewm = ewm;
            ViewBag.urlEnt = model.EWMUrl;
            if (string.IsNullOrEmpty(model.EWMUrl))
            {
                ViewBag.urlEnt = "#";// System.Configuration.ConfigurationManager.AppSettings["UrlSite"] + "/Home/Index?oid=" + orgID;
            }
            return View(model);
        }
        [HttpPost]
        public void Contact(LinqModel.Complaint model)
        {
            if (string.IsNullOrEmpty(model.Complaint_Customer) || string.IsNullOrEmpty(model.Complaint_Tel) || string.IsNullOrEmpty(model.Content) || string.IsNullOrEmpty(model.E_mail))
            {
                Response.Write("<script>alert('请填写您的投诉资料信息！');history.go(-1);</script>");
            }
            else
            {
                model.Complaint_Date = Common.Argument.Public.GetDateTimeNow();
                Common.Argument.RetResult ret = new DAL.Complaint().Add(model);
                if (ret.IsSuccess)
                {
                    Response.Write("<script>alert('" + ret.Msg + "');this.location = '/Home/Index?ewm=" + model.Prod_Code + "';</script>");
                }
                else
                {
                    Response.Write("<script>alert('" + ret.Msg + "');history.go(-1);</script>");
                }
            }
        }
        public ActionResult Footer()
        {
            return View();
        }
        public ActionResult Pass(string ewm, string pass)
        {
            string pw = Common.Argument.Public.GetEWMPass(System.Configuration.ConfigurationManager.AppSettings["urlZhuiSuNew"], ewm);
            if (pass == pw)
            {
                return View(new DAL.Trace_Rec().Verify(ewm, true));
            }
            else
            {
                return RedirectToAction("Index", new { ewm = ewm });
            }
        }
        public ActionResult ErrorPass(string ewm)
        {
            new DAL.Trace_Rec().Verify(ewm, false);
            return View();
        }
        public ActionResult ERROR()
        {
            return View();
        }


    }
}