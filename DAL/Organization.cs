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
    public class Organization : DALBase
    {
        public Common.Argument.RetResult Update(LinqModel.Organization model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Organization.FirstOrDefault(m => m.Org_ID == model.Org_ID);
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
                    temp.EWMUrl = model.EWMUrl;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult UpdateID(LinqModel.Organization model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    string shijiURL = ConfigurationManager.AppSettings["ComURL"];//实际的网址
                    var temp = dc.Organization.FirstOrDefault(m => m.Org_ID == model.Org_ID);
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
                    temp.Org_URL = shijiURL + "?oid=" + model.Org_ID;
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
            List<LinqModel.Organization> Itemsdt = new List<LinqModel.Organization>();

            Itemsdt = new DAL.Organization().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                list.Add(new SelectListItem { Text = "无上级", Value = "0" });
                foreach (LinqModel.Organization m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.Org_ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemWithOID(int oid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Organization> Itemsdt = new List<LinqModel.Organization>();

            Itemsdt = new DAL.Organization().GetAll();
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (oid == 0) });
                foreach (LinqModel.Organization m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.Org_ID.ToString(), Selected = (oid == m.Org_ID) });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemwithid(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Organization> Itemsdt = new List<LinqModel.Organization>();
            Itemsdt = new DAL.Organization().GetAllwithid(id); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (id.ToString().Equals("0")) });
                foreach (LinqModel.Organization m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.Org_ID.ToString(), Selected = (m.Org_ID.ToString().Equals(id.ToString())) });
                }
            }
            return list ?? null;
        }
        public List<LinqModel.Organization> GetAll()
        {
            List<LinqModel.Organization> list = new List<LinqModel.Organization>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Organization.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.Organization> GetAllwithid(int id)
        {
            List<LinqModel.Organization> list = new List<LinqModel.Organization>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Organization.Where(m => m.Org_ID == id).ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.Organization> Getorgid(int orgid)
        {
            List<LinqModel.Organization> list = new List<LinqModel.Organization>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Organization.Where(m => m.Org_ID == orgid).ToList();
                }
            }
            catch { }
            return list;
        }
        public int GetMaxID()
        {
            //var slt = dc.Database.SqlQuery<int>("select MAX(Org_ID) from Organization");
            //int AllCount = slt.First();
            //return AllCount;
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                return dc.Organization.Max(m => m.Org_ID);
            }
        }
        // public PagedList<LinqModel.Organization> GetPgedList(string Nmae, string Provincev, string City, string District, string Contact, string Tel, string Org_Code, int pageIndex, int pageSize)
        public PagedList<LinqModel.Organization> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.Organization> temp = null;
            List<LinqModel.Organization> list = new List<LinqModel.Organization>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Organization select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Organization>)list.OrderByDescending(m => m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.Organization> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int suporg)
        {
            PagedList<LinqModel.Organization> temp = null;
            List<LinqModel.Organization> list = new List<LinqModel.Organization>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Organization select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(suporg.ToString()))
                    {
                        data = data.Where(m => m.Sup_Org == suporg || m.Org_ID == suporg);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Organization>)list.OrderByDescending(m => m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.Organization> GetPgedListNewUser(string Nmae, int pageIndex, int pageSize, int uid)
        {
            PagedList<LinqModel.Organization> temp = null;
            List<LinqModel.Organization> list = new List<LinqModel.Organization>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Organization select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }


                    if (!string.IsNullOrEmpty(uid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == uid);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Organization>)list.OrderByDescending(m => m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public LinqModel.Organization GetModel(int? id)
        {
            LinqModel.Organization model = new LinqModel.Organization();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Organization.FirstOrDefault(m => m.Org_ID == id);
                }
            }
            catch { model = new LinqModel.Organization(); }
            return model;
        }
        public bool Exict(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                int c = dc.Organization.Where(m => m.Name == name).Count();

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
        public bool XGExict(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                int c = dc.Organization.Where(m => m.Name == name).Count();

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
        public Common.Argument.RetResult Create(LinqModel.Organization model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Organization.InsertOnSubmit(model);
                    dc.SubmitChanges();
                    new DAL.Organization().UpdateID(model);
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Edit([Bind(Include = "Org_ID,Name,Province,City,District,Address,Contact,Tel,E_mail_,Org_URL,Sup_Org,Intro,Brand,Cert,Org_Code")] LinqModel.Organization organization)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //dc.Entry(organization).State = EntityState.Modified;
                    var model = dc.Organization.FirstOrDefault(m => m.Org_ID == organization.Org_ID);
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
                    var model = dc.Organization.FirstOrDefault(m => m.Org_ID == id);
                    dc.Organization.DeleteOnSubmit(model);
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
                if (dc.Organization.FirstOrDefault(m => m.Name == name) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<LinqModel.OrgAutoCompleteModel> GetAutoComplete()
        {
            List<LinqModel.OrgAutoCompleteModel> list = new List<OrgAutoCompleteModel>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = (from m in dc.Organization select new OrgAutoCompleteModel { name = m.Name, value = m.Org_ID.ToString() }).ToList();
                }
            }
            catch { list = new List<OrgAutoCompleteModel>(); }
            return list;
        }
    }
}
