using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageGroup_DetailController : BaseController
    {
        // GET: ManageGroup_Detail
        public ActionResult Index(int? page)
        {
            string txtSearch = Request.QueryString["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int groupID = 0;
            List<LinqModel.MetaInfoGroup> list = new DAL.MetaInfoGroup().GetAll();
            var m = new LinqModel.MetaInfoGroup();
            m.Group_ID = 0;
            m.Group_Name = "全部";
            list.Insert(0, m);
            ViewBag.listGroup = list;
            if (Request.QueryString["groupID"] == null)
            {
                if (list.Count > 0)
                    groupID = list[0].Group_ID;
            }
            else
            {
                groupID = int.Parse(Request.QueryString["groupID"]);
            }
            ViewBag.groupID = groupID;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            ViewBag.index = pIndex;
            return View(new DAL.Group_Detail().GetList(txtSearch, groupID, pIndex, 10));
        }
        [HttpGet]
        public ActionResult Add()
        {
            int groupID = 0;
            List<LinqModel.MetaInfoGroup> listGroup = new DAL.MetaInfoGroup().GetAll();
            if (listGroup == null || listGroup.Count == 0)
            {
                return RedirectToAction("Index", "MetaInfoGroup");
            }
            else
            {
                groupID = listGroup[0].Group_ID;
            }
            ViewBag.listGroup = listGroup;
            if (Request.QueryString["groupID"] != null)
            {
                groupID = int.Parse(Request.QueryString["groupID"]);
            }
            ViewBag.groupID = groupID;
            StringBuilder sb = new StringBuilder();
            var list = new DAL.Group_Detail().GetList("", groupID, 1, int.MaxValue);
            List<int> listDetails = new List<int>();
            foreach (var m in list)
            {
                listDetails.Add(m.Info_ID);
                sb.Append(m.Info_Name + "，");
            }
            ViewBag.listName = sb.ToString();
            ViewBag.error = 0;
            return View(new DAL.Meta_Info().GetAllWithOut(listDetails, ""));
        }
        public ActionResult AddItem(int groupID, int infoID)
        {
            var model = new LinqModel.Group_Detail();
            model.Group_ID = groupID;
            model.Info_ID = infoID;
            Common.Argument.RetResult ret = new DAL.Group_Detail().Add(model);
            if (ret.IsSuccess)
            {
                string redirect = "/ManageGroup_Detail/Add?groupID=" + Request.QueryString["groupID"];
                return Content("<script>this.location = '"+ redirect +"';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageGroup_Detail/Add?groupID=" + Request.QueryString["groupID"] + "';</script>;");
            }
        }
        public ActionResult GetListMetaInfo(int groupID)
        {
            string txtSearch = Request.QueryString["txtSearch"];
            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = Common.Argument.Public.HtmlDecode(txtSearch);
            }
            var list = new DAL.Group_Detail().GetList("", groupID, 1, int.MaxValue);
            List<int> listDetails = new List<int>();
            foreach (var m in list)
            {
                listDetails.Add(m.Info_ID);
            }
            var listInfo = new DAL.Meta_Info().GetAllWithOut(listDetails, txtSearch);
            StringBuilder sb = new StringBuilder();
            foreach (var m in listInfo)
            {
                sb.Append("<tr><td>" + m.Info_Name + "</td><td>" + m.Info_Description + "</td><td><a href=\"javascript:ClickSelect(" + m.Info_ID + ")\">选择增加</a></td></tr>");
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListDetail(int groupID)
        {
            StringBuilder sb = new StringBuilder();
            var list = new DAL.Group_Detail().GetList("", groupID, 1, int.MaxValue);
            foreach (var m in list)
            {
                sb.Append(m.Info_Name + "，");
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Del(int id)
        {
            Common.Argument.RetResult ret = new DAL.Group_Detail().Del(id);
            if (ret.IsSuccess)
            {
                string redirect = "/ManageGroup_Detail/Index?groupID=" + Request.QueryString["groupID"] + "&page=" + Request.QueryString["pindex"];
                return Content("<script>this.location='"+ redirect+"';</script>");
            }
            else
            {
                return Content("<script>this.location = '/ManageGroup_Detail/Index?groupID=" + Request.QueryString["groupID"] + "&page=" + Request.QueryString["pindex"] + "';</script>;");
            }
        }
    }
}