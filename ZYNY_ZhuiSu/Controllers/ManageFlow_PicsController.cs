using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageFlow_PicsController : BaseController
    {
        //
        // GET: /ManageFlow_Pics/
        public ActionResult Index(int? page, int? prodID)
        {
            int orgID = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID;
            string txtSearch = Request["txtSearch"];
            ViewBag.orgID = orgID;
            ViewBag.txtSearch = txtSearch;
            ViewBag.page = page;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }

            prodID = prodID ?? 0;
            if (Request["prods"] != null)
            {
                prodID = int.Parse(Request["prods"]);
            }
            #region 产品信息
            List<LinqModel.Products> prods = new DAL.Products().GetAllwithORGID(orgID);
            ViewBag.prods = prods;
            if (prods.Count > 0 && prodID == 0)
            { prodID = prods[0].ID; }
            #endregion
            ViewBag.prodsCurrent = prodID;
            var modelProd = new DAL.Products().GetModel((int)prodID);
            if (modelProd != null && modelProd.ID > 0)
            {
                ViewBag.prodName = modelProd.Name;
            }
            else
            {
                ViewBag.prodName = "暂无产品";
            }
            #region 流程信息
            List<string> listFlow = new List<string>();
            var list = new DAL.Org_Flow().GetListAndFlow(txtSearch, orgID, (int)prodID, pIndex, 10, out listFlow);
            for (int i = 0; i < listFlow.Count; i++)
            {
                ViewBag.listFlow += "开始&nbsp;→ &nbsp;" + listFlow[i] + "结束<br />";
            }
            #endregion
            return View(list);
        }

        public ActionResult ListFlowPics(int orgFlowID, int? page, int prodID)
        {
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            ViewBag.page = pIndex;
            ViewBag.prodID = prodID;
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
            ViewBag.orgFlowID = orgFlowID;
            var modelMetaFlow = new DAL.Org_Flow().GetModelView(orgFlowID);
            ViewBag.flowName = modelMetaFlow.Flow_Name;
            ViewBag.prodName = new DAL.Products().GetModel(prodID).Name;
            return View(new DAL.Flow_Pics().GetList(orgFlowID, ViewBag.timeS, ViewBag.timeE, pIndex, 12));
        }
        [HttpGet]
        public ActionResult Add(int orgFlowID, int prodID, int? page)
        {
            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
            Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";
            ViewBag.Up_Time = Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd");
            ViewBag.orgFlowID = orgFlowID;
            ViewBag.page = page;
            ViewBag.prodID = prodID;
            return View();
        }
        [HttpPost]
        public ActionResult Add()
        {
            var user = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user;
            LinqModel.Flow_Pics model = new LinqModel.Flow_Pics();
            model.Org_Flow_ID = int.Parse(Request["orgFlowID"]);
            model.Pic_Description = Request["Pic_Description"];
            model.Pic_Path = System.Configuration.ConfigurationManager.AppSettings["urlImg"] + Request["Pic_Path"];
            model.Up_Time = DateTime.Parse(Request["Up_Time"]);
            Common.Argument.RetResult ret = new DAL.Flow_Pics().Add(model);
            if (ret.IsSuccess)
            {
                return Content("<script>this.location = '/ManageFlow_Pics/ListFlowPics?orgFlowID=" + Request["orgFlowID"] + "&prodID=" + Request["prodID"] + "&page=" + Request["page"] + "';</script>");
            }
            else
            {
                return Content("<script>history.go(-1);</script>");
            }

            //var files = Request.Files["InfoFile"];
            //if (files == null || files.ContentLength <= 0)
            //{
            //    Response.Write("<script>alert('请选择图片格式文件。现支持：.jpg，.jpge，.gif，.bmp，.png ');history.go(-1);</script>");
            //}
            //else
            //{
            //    string filePath = "/images/" + user.Org_ID + "/" + Common.Argument.Public.GetDateTimeNow().Year + "/" + Common.Argument.Public.GetDateTimeNow().Month + "/" + Common.Argument.Public.GetDateTimeNow().Day;
            //    if (!System.IO.Directory.Exists(Server.MapPath("~" + filePath)))
            //    {
            //        System.IO.Directory.CreateDirectory(Server.MapPath("~" + filePath));//不存在就创建目录 
            //    }
            //    string extendName = System.IO.Path.GetExtension(files.FileName).ToLower();
            //    string fileFull = filePath + "/" + Common.Argument.Public.GetDateTimeNow().ToFileTime() + extendName;
            //    if (extendName == ".jpg" || extendName == ".jpge" || extendName == ".gif" || extendName == ".bmp" || extendName == ".png")
            //    {
            //        Image image = Image.FromStream(files.InputStream);
            //        int towidth = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ImageWidth"]);
            //        int toheight = image.Height * towidth / image.Width;
            //        Bitmap objNewBitMap = new Bitmap(towidth, toheight, PixelFormat.Format32bppArgb);
            //        //从指定的 Image 对象创建新 Graphics 对象     
            //        Graphics objGraphics = Graphics.FromImage(objNewBitMap);
            //        //清除整个绘图面并以透明背景色填充     
            //        objGraphics.Clear(Color.Transparent);
            //        //在指定位置并且按指定大小绘制 原图片 对象     
            //        objGraphics.DrawImage(image, new Rectangle(0, 0, towidth, toheight));
            //        objNewBitMap.Save(Server.MapPath("~" + fileFull));
            //        image.Dispose();
            //        //files.SaveAs(Server.MapPath("~" + fileFull));
            //        LinqModel.Flow_Pics model = new LinqModel.Flow_Pics();
            //        model.Org_Flow_ID = int.Parse(Request["orgFlowID"]);
            //        model.Pic_Description = Request["Pic_Description"];
            //        model.Pic_Path = System.Configuration.ConfigurationManager.AppSettings["urlImg"] + fileFull;
            //        model.Up_Time = DateTime.Parse(Request["Up_Time"]);

            //        Common.Argument.RetResult ret = new DAL.Flow_Pics().Add(model);
            //        if (ret.IsSuccess)
            //        {
            //            Response.Write("<script>alert('" + ret.Msg + "');this.location = '/ManageFlow_Pics/ListFlowPics?orgFlowID=" + Request["orgFlowID"] + "&prodID=" + Request["prodID"] + "&page=" + Request["page"] + "';</script>");
            //        }
            //        else
            //        {
            //            Response.Write("<script>alert('" + ret.Msg + "');history.go(-1);</script>");
            //        }
            //    }
            //    else
            //    {
            //        Response.Write("<script>alert('请选择图片格式文件。现支持：.jpg，.jpge，.gif，.bmp，.png ');history.go(-1);</script>");
            //    }
            //}

        }

        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Flow_Pics().Del(id);

            return Content("<script>this.location = '/ManageFlow_Pics/ListFlowPics?orgFlowID=" + Request.QueryString["orgFlowID"] + "&prodID=" + Request["prodID"] + "&page=" + Request.QueryString["page"] + "';</script>");
        }
    }
}