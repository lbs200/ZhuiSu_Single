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
using System.Text;
using System.Web.UI;
using System.IO;
using System.Security.Cryptography;
namespace ZYNY_ZhuiSu.Controllers
{
    public class ProductsController : BaseController
    {


        // GET: Products

        public ActionResult Index(int? page)
        {
            string txtSearch = Request["txtSearch"];
            string CategoryDDL = Request["CategoryDDL"];
            ViewBag.SelectCategory = CategoryDDL;

            ViewBag.txtSearch = txtSearch;


            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)
            {
                ViewBag.selectlist = new DAL.Organization().GetAll();
                return View(new DAL.Products().GetPgedListWithORG(txtSearch, pIndex, 10, CategoryDDL));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View(new DAL.Products().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
            }
            else
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View();
            }

        }
        public ActionResult DIndex(int? page)
        {
            string CategoryDDL = Request["CategoryDDL"];
            string txtSearch = Request["txtSearch"];
            ViewBag.SelectCategory = CategoryDDL;


            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)
            {
                ViewBag.selectlist = new DAL.Organization().GetAll();
                return View(new DAL.Products().GetPgedListWithORG(txtSearch, pIndex, 10, CategoryDDL));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View(new DAL.Products().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
            }
            else
            {
                return View();
            }

        }

        public ActionResult DIndexNew(int? page)
        {


            string CategoryDDL = Request["CategoryDDL"];
            string txtSearch = Request["txtSearch"];
            ViewBag.SelectCategory = CategoryDDL;

            ViewBag.txtSearch = txtSearch;
            int pIndex = page ?? 0;
            if (pIndex <= 0) { pIndex = 1; }
            if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 5 || (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 2)
            {
                ViewBag.selectlist = new DAL.Organization().GetAll();
                return View(new DAL.Products().GetPgedListWithORG(txtSearch, pIndex, 10, CategoryDDL));
            }
            else if ((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Role_ID == 1)
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View(new DAL.Products().GetPgedListNew(txtSearch, pIndex, 10, (int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID));
            }
            else
            {
                ViewBag.selectlist = new DAL.Organization().GetAllwithid((int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.Org_ID);
                return View();
            }

        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var dc = DAL.DALBase.GetDataContext();
            Products products = dc.Products.FirstOrDefault(m => m.ID == id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                string uname = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;

                Session["CKFinder:DynamicBaseUrl"] = "/images/" + uname + "/";

                ViewBag.Org_ID = new SelectList(dc.Organization, "Org_ID", "Name");
                ViewBag.ID = new SelectList(dc.Prod_Authentication, "ID", "ID");
                ViewBag.ID = new SelectList(dc.Prod_category, "ID", "Description");
                return View();
            }
        }

        // POST: Products/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (true)
                {
                    if (new DAL.Products().Exict(products.Name, Convert.ToInt32(products.Org_ID)))
                    {
                        ModelState.AddModelError("Name", "产品名称已经被占用");

                    }
                    else if (new DAL.Products().ExictSX(products.Abbr, Convert.ToInt32(products.Org_ID)))
                    {
                        ModelState.AddModelError("Abbr", "缩写已经被占用");
                    }
                    else
                    {
                        if (products.Prod_Category == null || products.Prod_Category == -1)
                        {
                            ViewData.ModelState.AddModelError("Prod_Category", "请选择产品类别");
                        }
                        else if (products.Org_ID == null || products.Org_ID == -1)
                        {
                            ViewData.ModelState.AddModelError("Org_ID", "请选择机构");
                        }
                        else if (products.Name == null)
                        {
                            ViewData.ModelState.AddModelError("Name", "请填写名称");
                        }
                        else if (products.Abbr == null)
                        {
                            ViewData.ModelState.AddModelError("Abbr", "请填写缩写");
                        }

                        else
                        {
                            dc.Products.InsertOnSubmit(products);
                            dc.SubmitChanges();
                            new DAL.Products().UpdateID(products);

                            #region 添加模板流程，信息元，权限
                            try
                            {
                                var modelUser = dc.User.FirstOrDefault(m => m.Org_ID == (int)products.Org_ID);
                                var modelOrg = dc.Organization.FirstOrDefault(m => m.Org_ID == (int)products.Org_ID);
                                new DAL.PublicRegistCommon().AddFlowAndPermission((int)products.Org_ID, modelUser.ID, (int)modelOrg.HangYeID, products.ID);
                            }
                            catch { }
                            #endregion

                            return RedirectToAction("Index");
                        }
                    }
                }

                ViewBag.Org_ID = new SelectList(dc.Organization, "Org_ID", "Name", products.Org_ID);
                ViewBag.AID = new SelectList(dc.Prod_Authentication, "ID", "ID", products.ID);
                ViewBag.ID = new SelectList(dc.Prod_category, "ID", "Description", products.ID);
                return View(products);
            }
        }
        public ActionResult Update(int id)
        {
            LinqModel.Products Products = new DAL.Products().GetModel(id);

            ViewBag.selectlist = new DAL.Region().GetAllSheng();

            ViewBag.error = 0;
            // List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
            // ViewData["Categories"] = new SelectList(gb, "C4", "Name", Products.Org_Code.ToString().Trim().Substring(2, Products.Org_Code.ToString().Trim().IndexOf("-") - 2));
            ViewBag.subID = new DAL.Pro_Category().GetModel(Products.Prod_Category).Sup_Category;
            return View(Products);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update(LinqModel.Products model)
        {

            Common.Argument.RetResult ret = new DAL.Products().Update(model);

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
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Products products = dc.Products.FirstOrDefault(m => m.ID == id);
                if (products == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Org_ID = new SelectList(dc.Organization, "Org_ID", "Name", products.Org_ID);
                ViewBag.AID = new SelectList(dc.Prod_Authentication, "ID", "ID", products.ID);
                ViewBag.ID = new SelectList(dc.Prod_category, "ID", "Description", products.ID);
                return View(products);
            }
        }

        // POST: Products/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Org_ID,Name,Abbr,Prod_Category,Class,Spec,Unit,material,Company,NET,Count,Description,Adv_Code,Max_SN,Photo,BuyUrl,EWMUrl")] Products products)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (ModelState.IsValid)
                {
                    if (products.Prod_Category == null)
                    {
                        ViewData.ModelState.AddModelError("Prod_Category", "请选择产品类别");
                    }
                    else if (products.Org_ID == null || products.Org_ID == -1)
                    {
                        ViewData.ModelState.AddModelError("Org_ID", "请选择机构");
                    }
                    else if (products.Name == null)
                    {
                        ViewData.ModelState.AddModelError("Name", "请填写名称");
                    }
                    else if (products.Abbr == null)
                    {
                        ViewData.ModelState.AddModelError("Abbr", "请填写缩写");
                    }

                    else
                    {
                        //dc.Entry(products).State = EntityState.Modified;
                        dc.Products.Attach(products);
                        dc.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.Org_ID = new SelectList(dc.Organization, "Org_ID", "Name", products.Org_ID);
                ViewBag.ID = new SelectList(dc.Prod_Authentication, "ID", "ID", products.ID);
                ViewBag.ID = new SelectList(dc.Prod_category, "ID", "Description", products.ID);
                return View(products);
            }
        }
        public ActionResult DownloadTXT(int id)
        {
            LinqModel.Products Products = new DAL.Products().GetModel(id);

            ViewBag.selectlist = new DAL.Region().GetAllSheng();

            ViewBag.error = 0;
            // List<LinqModel.GBT475494> gb = new DAL.GB().GetAll();
            // ViewData["Categories"] = new SelectList(gb, "C4", "Name", Products.Org_Code.ToString().Trim().Substring(2, Products.Org_Code.ToString().Trim().IndexOf("-") - 2));

            return View(Products);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DownloadTXT(LinqModel.Products model)
        {
            model = new DAL.Products().GetModel(model.ID);
            int maxn = 0;
            int.TryParse(model.Max_SN.ToString(), out maxn);
            string path = Server.HtmlEncode(Request.PhysicalApplicationPath);

            int dcout = 0;
            int.TryParse(Request["Dcount"], out dcout);

            int nin = maxn + dcout;
            string shijiURL = ConfigurationManager.AppSettings["urlZhuiSu"];//实际的网址
            string txtname = path + "/txt/shangchuan" + model.ID + "" + nin + "" + ".txt";
            StringBuilder jmcontent = new StringBuilder();
            int ccc = 0;

            #region 生成txt信息
            for (int i = maxn + 1; i <= dcout + maxn; i++)
            {
                string content = "";
                string sixstr = "";
                string sss = i + "";
                int sum = 0;
                for (int j = 0; j < sss.Length; j++)
                {
                    sum += Convert.ToInt32(sss.Substring(j, 1));//累加每一个数字
                }

                content = shijiURL + model.Adv_Code + i + sum;
                /////////////////////////md5加密///////////////////////////////
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fromData = System.Text.Encoding.Unicode.GetBytes(content);
                byte[] toData = md5.ComputeHash(fromData);
                string byteStr = null;
                for (int m = 0; m < toData.Length; m++)
                {
                    byteStr += toData[m].ToString("x");
                }
                content = byteStr.Substring(0, 16);
                for (int j = 0; j < content.Length; j++)
                {
                    int n;
                    if (int.TryParse(content.Substring(j, 1), out n))
                    {
                        sixstr += content.Substring(j, 1) + "";//累加每一个数字
                    }

                }

                if (sixstr.Length < 6)
                {
                    int js = sixstr.Length;
                    for (int xx = 0; xx < 6 - js; xx++)
                    {
                        sixstr += "0";
                    }

                }
                sixstr = sixstr.Substring(0, 6);

                jmcontent.AppendLine("1" + model.ID + model.Adv_Code.Substring(1, 1) + i + "," + shijiURL + "?ewm=" + model.Adv_Code + i + "," + sixstr);
                //////////////////////////////////////////////////////////////////
                ccc++;

            }
            #endregion

            new DAL.Products().WriteToFile(txtname, jmcontent.ToString(), true);
            LinqModel.CodeLog cl = new CodeLog();
            cl.DateTime = Common.Argument.Public.GetDateTimeNow();
            cl.Org_ID = model.Org_ID;
            cl.Product_ID = model.ID;
            cl.Count = dcout;
            cl.EndNO = Convert.ToInt32(model.Max_SN) + dcout;
            cl.StartNO = Convert.ToInt32(model.Max_SN) + 1;
            cl.UserName = ((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]]).user.UserName;
            cl.Remark = "";
            new DAL.CodeLog().Create(cl);

            //////////////////////////////////////////
            model.Max_SN = model.Max_SN + dcout;
            Common.Argument.RetResult ret = new DAL.Products().Update(model);

            #region 下载文件
            if (ret.IsSuccess)
            {
                //Response.Write("<script>alert('"+ccc+"条数据生成成功，请点击下载按钮下载！');</script>");
                ///////////////////////////////////下载文件/////////////////////////////////////
                //string fileName = "shangchuan" + model.ID + "" + nin + "" + ".txt";//客户端保存的文件名 
                //string filePath = Server.MapPath("/txt/shangchuan" + model.ID + "" + nin + "" + ".txt");//路径 
                //FileInfo fileInfo = new FileInfo(filePath);
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                //Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                //Response.AddHeader("Content-Transfer-Encoding", "binary");
                //Response.ContentType = "application/octet-stream";
                //Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                //Response.WriteFile(fileInfo.FullName);
                //Response.Flush();
                //Response.End();

                ////////////////////////////////////下载完毕、、///////////////////////////////
                return RedirectToAction("Index", "CodeLog");
                // return null;

            }
            else
            {
                ViewBag.error = 1;
                ViewBag.errorMsg = ret.Msg;
                return View(model);
            }
            #endregion
        }
        public void DLTXT()
        {
            string fileName = "shangchuan.txt";//客户端保存的文件名 
            string filePath = Server.MapPath("/txt/shangchuan.txt");//路径 
            FileInfo fileInfo = new FileInfo(filePath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();

        }




        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Products products = dc.Products.FirstOrDefault(m => m.ID == id);
                if (products == null)
                {
                    return HttpNotFound();
                }
                return View(products);
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {
                Products products = dc.Products.FirstOrDefault(m => m.ID == id);
                dc.Products.DeleteOnSubmit(products);
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
