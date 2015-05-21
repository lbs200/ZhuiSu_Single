using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LinqModel;
using System.Configuration;

namespace ZYNY_ZhuiSu.Controllers
{
    public class ManageUsersController : BaseController
    {

        // GET: ManageUsers
        public ActionResult Index(int? page)
        {
            //获取页面用户名字检索字段
            string txtUserNameSearch = Request["txtUserNameSearch"];
            ViewBag.txtUserNameSearch = txtUserNameSearch;
            ViewBag.orgs = new DAL.Organization().GetAll();
            ViewBag.roles = new DAL.Role().GetAll();
            int orgId = 0;
            int roleId = 0;
            int.TryParse(Request.QueryString["orgs"], out orgId);
            int.TryParse(Request.QueryString["roles"], out roleId);

            ViewBag.orgsValue = orgId;//页面选中的机构编号
            ViewBag.rolesValue = roleId;//页面选中的角色编号
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }//判断页码是否小于1
            int roleid = 0;
            roleid = (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID;
            if (roleid == 5 || roleid == 2)
            {
                return View(new DAL.User().GetPgedList(txtUserNameSearch, orgId, roleId, pIndex, 10));
            }
            else if (roleid == 1)
            {
                return View(new DAL.User().GetPgedListNew(txtUserNameSearch, orgId, roleId, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
            }
            else
            {
                return View(new DAL.User().GetPgedListNewUser(txtUserNameSearch, orgId, roleId, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.ID));
            }
          
        }
        // GET: ManageUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LinqModel.View_User model = new DAL.View_User().GetModel(id);

            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }


        // GET: ManageUsers/Create
        public ActionResult Create()
        {
            string shijiURL = ConfigurationManager.AppSettings["AURL"];//实际的网址

            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
           // Session["CKFinder:DynamicBaseUrl"] = shijiURL + "/ckfinder/userfiles/"+uname+"/";
            Session["CKFinder:DynamicBaseUrl"] =  "/images/" ;
            return View();
        }

        // POST: ManageUsers/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Org_ID,Type,UserName,Password,Photo,User_Code,Role_ID")] User user)
        {
            if (ModelState.IsValid)
            {
                if (new DAL.User().Exist(user.UserName))
                {
                    ModelState.AddModelError("UserName", "用户名已经被占用");
                }
                else
                {
                    if (user.UserName == null)
                    {
                        ViewData.ModelState.AddModelError("UserName", "用户名不能为空");
                    }
                    else if (user.Org_ID == null|| user.Org_ID==-1 )
                    {
                        ViewData.ModelState.AddModelError("Org_ID", "请选择机构");
                    }
                    else if (user.Type == null)
                    {
                        ViewData.ModelState.AddModelError("Type", "请选择用户类型");
                    }
                    else if (user.Password == null)
                    {
                        ViewData.ModelState.AddModelError("Password", "请填写密码");
                    }
                    else if (user.Password != Request["qrmm"])
                    {
                        ViewData.ModelState.AddModelError("Password", "两次密码不一致");
                    }
                    else
                    {
                        Common.Argument.RetResult ret = new DAL.User().Create(user);
                        return RedirectToAction("Index");
                    }
                    //user.Photo = "http://" + user.Photo;
                  
                }
            }

            return View(user);
        }

        // GET: ManageUsers/Edit/5
        public ActionResult Edit(int id)
        {
            string shijiURL = ConfigurationManager.AppSettings["AURL"];//实际的网址

            string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
          //  Session["CKFinder:DynamicBaseUrl"] = shijiURL + "/ckfinder/userfiles/" + uname+"/";
            Session["CKFinder:DynamicBaseUrl"] = "/images/";
           
            User user = new DAL.User().GetModel(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: ManageUsers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Org_ID,Type,UserName,Password,Photo,User_Code,Role_ID")] User user)
        {
            if (ModelState.IsValid)
            {
                  if (user.UserName == null)
                    {
                        ViewData.ModelState.AddModelError("UserName", "用户名不能为空");
                    }
                  else if (user.Org_ID == null || user.Org_ID == -1)
                    {
                        ViewData.ModelState.AddModelError("Org_ID", "请选择机构");
                    }
                    else if (user.Type == null)
                    {
                        ViewData.ModelState.AddModelError("Type", "请选择用户类型");
                    }
                    else if (user.Password == null)
                    {
                        ViewData.ModelState.AddModelError("Password", "请填写密码");
                    }
                  else if (user.Password != Request["qrmm"])
                  {
                      ViewData.ModelState.AddModelError("Password", "两次密码不一致");
                  }
                  else
                  {

                      Common.Argument.RetResult ret = new DAL.User().Edit(user);

                      ViewBag.checkBox = user.Type;
                      return RedirectToAction("Index");
                  }
              
            }
            return View(user);
        }

        // GET: ManageUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            View_User user = new DAL.View_User().GetModel(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: ManageUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Common.Argument.RetResult ret = new DAL.User().Del(id);
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
    }
}
