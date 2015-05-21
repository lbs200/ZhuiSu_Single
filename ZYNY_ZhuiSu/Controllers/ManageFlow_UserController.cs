using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LinqModel;
using System.Text;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageFlow_UserController : Controller
    {
        public ActionResult GetP(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {


                var pp = from m in dc.User
                         select new
                         {
                             ID = m.ID,
                             UserName = m.UserName,
                             Org_ID = m.Org_ID,
                             Role_ID = m.Role_ID
                         };
                return Json(pp.ToList().Where(m => m.Org_ID.ToString().Equals(cid)), JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetZIP(string oid)
        {
            StringBuilder sb = new StringBuilder();
            List<LinqModel.Products> listP = new DAL.Products().GetAllwithORGID(int.Parse(oid));
            foreach (var m in listP)
            {
                List<View_Org_Flow> listF = new DAL.Org_Flow().GetList("", int.Parse(oid), m.ID, 1, int.MaxValue).ToList();
                if (listF != null && listF.Count > 0)
                {
                    sb.Append("<div> <h4>产品【" + m.Name + "】定制流程：</h4><div id='qxlb_" + m.ID + "' style='margin-left:50px'>");
                    foreach (var n in listF)
                    {
                        sb.Append("<label class='checkbox' ><input id= 'checkb_" + n.Org_Flow_ID + "' name='Org_Flow_ID' type='checkbox' value='" + n.Org_Flow_ID + "' property='mycheck' />" + "【" + m.Name + "】" + "（" + n.Seq_No + "）" + n.Flow_Name + " &nbsp;&nbsp;&nbsp; </label>");
                    }
                    sb.Append("</div> </div><input type=\"hidden\" id=\"pn_" + m.ID + "\" value=\"" + m.Name + "\" />");
                }
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.ORGFLOW
                         select new
                         {

                             Org_ID = m.Org_ID,
                             Org_Flow_ID = m.Org_Flow_ID,
                             Flow_ID = m.Flow_ID,
                             Sup_Flow_ID = m.Sup_Flow_ID,
                             Flow_Name = m.Flow_Name,
                             PName = m.PName,
                             Seq_No = m.Seq_No,




                         };
                return Json(pp.ToList().Where(m => m.Org_ID.ToString().Equals(oid)), JsonRequestBehavior.AllowGet);


            }
        }



        public ActionResult GetSJ(string sid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.SJ
                         select new
                         {

                             Org_Flow_ID = m.Org_Flow_ID,
                             Flow_ID = m.Flow_ID,
                             Sup_Flow_ID = m.Sup_Flow_ID,
                             Flow_Name = m.Flow_Name,
                             shagnjiname = m.Expr1,




                         };
                return Json(pp.ToList().Where(m => m.Org_Flow_ID.ToString().Equals(sid)), JsonRequestBehavior.AllowGet);


            }
        }
        public ActionResult Getxy(string uid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                var pp = from m in dc.Flow_User
                         where m.Use_ID==int.Parse(uid)
                         select new
                         {
                             ID = m.ID,
                             Org_Flow_ID = m.Org_Flow_ID,
                             Use_ID = m.Use_ID,

                         } ;
                return Json(pp.ToList(), JsonRequestBehavior.AllowGet);


            }
        }
        public string GetSJname(string sid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                LinqModel.View_Org_Flow ofm = dc.View_Org_Flow.FirstOrDefault(m => m.Org_Flow_ID.ToString().Equals(sid.ToString()));
                return ofm.Flow_Name;


            }
        }
        public string Test()
        {
            return "dddd";


        }

        public ActionResult GetAllQ(int uid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {

                var pp = from m in dc.Flow_User
                         select new
                         {
                             Org_Flow_ID = m.Org_Flow_ID,
                             Use_ID = m.Use_ID,

                         };
                return Json(pp.Where(m => m.Use_ID == uid).ToList(), JsonRequestBehavior.AllowGet);


            }
        }
        // GET: ManageFlow_User
        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)
            {
                return View(new DAL.View_Flow_Users().GetPgedList(txtSearch, pIndex, 10));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)
            {
                return View(new DAL.View_Flow_Users().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: ManageFlow_User/Details/5
        public ActionResult Details(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Flow_User flow_User = dc.Flow_User.FirstOrDefault(m => m.ID == id);
                if (flow_User == null)
                {
                    return HttpNotFound();
                }
                return View(flow_User);
            }
        }

        // GET: ManageFlow_User/Create
        public ActionResult Create(string cid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                string categiryid = Request.QueryString["CategoryDDL"];

                ViewBag.SelectCategory = categiryid;
                ViewBag.CID = cid;
                if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)
                {
                    ViewBag.selectlist = new DAL.Organization().GetAll();
                    ViewBag.pn = new DAL.Flow_User().GetAllProduct();

                }
                else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)
                {
                    ViewBag.selectlist = new DAL.Organization().Getorgid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                    ViewBag.pn = new DAL.Flow_User().GetProduct((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                }
                else
                {
                    ViewBag.selectlist = null;
                }


                ViewBag.ID = new SelectList(dc.Org_Flow, "Org_Flow_ID", "Current_State");
                ViewBag.Use_ID = new SelectList(dc.User, "ID", "Type");
                return View();
            }
        }

        // POST: ManageFlow_User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Org_Flow_ID,Use_ID")] Flow_User flow_User)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                string categiryid = Request["Use_ID"];
                string pidsz = Request["checkvalues"];
                string upidsz = Request["ucheckvalues"];
                string[] ss = pidsz.Substring(0, pidsz.Length - 1).Split('*');
                //string[] uss = upidsz.Substring(0, upidsz.Length - 1).Split('*');
                if (ModelState.IsValid)
                {
                    if (flow_User.Org_Flow_ID == null || flow_User.Org_Flow_ID == -1)
                    {
                        ViewData.ModelState.AddModelError("Org_Flow_ID", "请选择流程");
                    }
                    else if (flow_User.Use_ID == null || flow_User.Use_ID == -1)
                    {
                        ViewData.ModelState.AddModelError("Use_ID", "请选择用户");
                    }
                    
                else
                {

                    dc.ExecuteCommand("DELETE FROM Flow_User WHERE Use_ID =" + Convert.ToInt32(categiryid) + "");
                    for (int j = 0; j < ss.Length; j++)
                    {
                        LinqModel.Flow_User pp = new Flow_User();
                        pp.Use_ID = Convert.ToInt32(categiryid);
                        pp.Org_Flow_ID = Convert.ToInt32(ss[j]);
                        dc.Flow_User.InsertOnSubmit(pp);
                        dc.SubmitChanges();

                    }


                    Common.Argument.RetResult ret = new Common.Argument.RetResult();
                    ret.Msg = "添加成功";
                    ViewBag.error = 1;
                    ViewBag.errorMsg = ret.Msg;


                    return RedirectToAction("Index");
                }
                }

                ViewBag.ID = new SelectList(dc.Org_Flow, "Org_Flow_ID", "Current_State", flow_User.ID);
                ViewBag.Use_ID = new SelectList(dc.User, "ID", "Type", flow_User.Use_ID);
                return View(flow_User);
            }
        }

        // GET: ManageFlow_User/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Flow_User flow_User = dc.Flow_User.FirstOrDefault(m => m.ID == id);
                if (flow_User == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ID = new SelectList(dc.Org_Flow, "Org_Flow_ID", "Current_State", flow_User.ID);
                ViewBag.Use_ID = new SelectList(dc.User, "ID", "Type", flow_User.Use_ID);
                return View(flow_User);
            }
        }
        public ActionResult Update(int id)
        {
            LinqModel.Flow_User Flow_User = new DAL.Flow_User().GetModel(id);


            return View(Flow_User);
        }
        [HttpPost]
        public ActionResult Update(LinqModel.Flow_User model)
        {

            Common.Argument.RetResult ret = new DAL.Flow_User().Update(model);

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
        // POST: ManageFlow_User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Org_Flow_ID,Use_ID")] Flow_User flow_User)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    //dc.Entry(flow_User).State = EntityState.Modified;
                    dc.Flow_User.Attach(flow_User);
                    dc.SubmitChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.ID = new SelectList(dc.Org_Flow, "Org_Flow_ID", "Current_State", flow_User.ID);
                ViewBag.Use_ID = new SelectList(dc.User, "ID", "Type", flow_User.Use_ID);
                return View(flow_User);
            }
        }

        // GET: ManageFlow_User/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Flow_User flow_User = dc.Flow_User.FirstOrDefault(m => m.ID == id);
                if (flow_User == null)
                {
                    return HttpNotFound();
                }
                return View(flow_User);
            }
        }

        // POST: ManageFlow_User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                Flow_User flow_User = dc.Flow_User.FirstOrDefault(m => m.ID == id);
                dc.Flow_User.DeleteOnSubmit(flow_User);
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
