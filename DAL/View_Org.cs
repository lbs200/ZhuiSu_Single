using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PagedList.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LinqModel;
using System.Configuration;

namespace DAL
{
    public class View_Org : DALBase
    {
        public Common.Argument.RetResult Update(LinqModel.View_Org model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.View_Org.FirstOrDefault(m => m.Org_ID == model.Org_ID);
                    temp.Name = model.Name;
                    temp.Address = model.Address;
                    temp.Brand = model.Brand;
                    temp.Cert = model.Cert;
                    temp.City = model.City;
                    temp.Contact = model.Contact;
                    temp.District = model.District;
                    temp.E_mail_ = model.E_mail_;
                    temp.Intro = model.Intro;
                    temp.Org_Code = model.Org_Code;
                    temp.Org_URL = model.Org_URL;
                    temp.Province = model.Province;
                    temp.Sup_Org = model.Sup_Org;
                    temp.Tel = model.Tel;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult UpdateID(LinqModel.View_Org model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    string shijiURL = ConfigurationManager.AppSettings["ComURL"];//实际的网址
                    var temp = dc.View_Org.FirstOrDefault(m => m.Org_ID == model.Org_ID);
                    temp.Name = model.Name;
                    temp.Address = model.Address;
                    temp.Brand = model.Brand;
                    temp.Cert = model.Cert;
                    temp.City = model.City;
                    temp.Contact = model.Contact;
                    temp.District = model.District;
                    temp.E_mail_ = model.E_mail_;
                    temp.Intro = model.Intro;
                    temp.Org_Code = model.Org_Code + model.Org_ID;
                    temp.Org_URL = shijiURL+"?oid="+model.Org_ID;
                    temp.Province = model.Province;
                    temp.Sup_Org = model.Sup_Org;
                    temp.Tel = model.Tel;

                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public List<SelectListItem> GetDropdownlistItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.View_Org> Itemsdt = new List<LinqModel.View_Org>();

            Itemsdt = new DAL.View_Org().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "无上级", Value = "0" });
                foreach (LinqModel.View_Org m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.Org_ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemwithid(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.View_Org> Itemsdt = new List<LinqModel.View_Org>();
            Itemsdt = new DAL.View_Org().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (id.ToString().Equals("0")) });
                foreach (LinqModel.View_Org m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.Org_ID.ToString(), Selected = (m.Org_ID.ToString().Equals(id.ToString())) });
                }
            }
            return list ?? null;
        }
        public List<LinqModel.View_Org> GetAll()
        {
            List<LinqModel.View_Org> list = new List<LinqModel.View_Org>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Org.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.View_Org> Getorgid(int orgid)
        {
            List<LinqModel.View_Org> list = new List<LinqModel.View_Org>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Org.Where(m=>m.Org_ID==orgid).ToList();
                }
            }
            catch { }
            return list;
        }
        public int GetMaxID()
        {
            //var slt = dc.Database.SqlQuery<int>("select MAX(Org_ID) from View_Org");
            //int AllCount = slt.First();
            //return AllCount;
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                return dc.View_Org.Max(m => m.Org_ID);
            }
        }
        // public PagedList<LinqModel.View_Org> GetPgedList(string Nmae, string Provincev, string City, string District, string Contact, string Tel, string Org_Code, int pageIndex, int pageSize)
        public PagedList<LinqModel.View_Org> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.View_Org> temp = null;
            List<LinqModel.View_Org> list = new List<LinqModel.View_Org>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.View_Org select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.View_Org>)list.OrderByDescending(m=>m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.View_Org> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int suporg)
        {
            PagedList<LinqModel.View_Org> temp = null;
            List<LinqModel.View_Org> list = new List<LinqModel.View_Org>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.View_Org select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(suporg.ToString()))
                    {
                        data = data.Where(m => m.Sup_Org == suporg);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.View_Org>)list.OrderByDescending(m=>m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public LinqModel.View_Org GetModel(int? id)
        {
            LinqModel.View_Org model = new LinqModel.View_Org();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Org.FirstOrDefault(m => m.Org_ID == id);
                }
            }
            catch { model = new LinqModel.View_Org(); }
            return model;
        }
        public bool Exict(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                int c = dc.View_Org.Where(m => m.Name == name).Count();

                if (c > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Common.Argument.RetResult Create(LinqModel.View_Org model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.View_Org.InsertOnSubmit(model);
                    dc.SubmitChanges();
                    new DAL.View_Org().UpdateID(model);
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Edit([Bind(Include = "Org_ID,Name,Province,City,District,Address,Contact,Tel,E_mail_,Org_URL,Sup_Org,Intro,Brand,Cert,Org_Code")] LinqModel.View_Org organization)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //dc.Entry(organization).State = EntityState.Modified;
                    var model = dc.View_Org.FirstOrDefault(m => m.Org_ID == organization.Org_ID);
                    model.Address = organization.Address;
                    model.Brand = organization.Brand;
                    model.Cert = organization.Cert;
                    model.City = organization.City;
                    model.Contact = organization.Contact;
                    model.District = organization.District;
                    model.E_mail_ = organization.E_mail_;
                    model.Intro = organization.Intro;
                    model.Name = organization.Name;
                    model.Org_Code = organization.Org_Code;
                    model.Org_URL = organization.Org_URL;
                    model.Province = organization.Province;
                    model.Sup_Org = organization.Sup_Org;
                    model.Tel = organization.Tel;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Del(int id)
        {
            Ret.Msg = "删除成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.View_Org.FirstOrDefault(m => m.Org_ID == id);
                    dc.View_Org.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
        public bool Exist(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                if (dc.View_Org.Where(m => m.Name == name) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
