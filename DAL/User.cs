using LinqModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class User : DALBase
    {

        public List<SelectListItem> Getallshengddlwiths(int sid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Organization> Itemsdt = new List<LinqModel.Organization>();
            Itemsdt = new DAL.Organization().GetAll(); ;
            if (Itemsdt != null)
            {
                foreach (LinqModel.Organization m in Itemsdt)
                {
                    list.Add(new SelectListItem { Selected = (m.Org_ID.Equals(sid.ToString())), Text = m.Name, Value = m.Org_ID.ToString() });
                }
            }
            return list ?? null;
        }
        public Common.Argument.RetResult UpdateID(LinqModel.User model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.User.FirstOrDefault(m => m.ID == model.ID);
                    temp.Org_ID = model.Org_ID;
                    temp.Password = model.Password;
                    temp.Photo = model.Photo;
                    temp.Role_ID = model.Role_ID;
                    temp.Type = model.Type;
                    temp.User_Code = model.ID.ToString();
                    temp.UserName = model.UserName;
                   

                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public List<LinqModel.User> GetAll()
        {
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.User.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.User> GetAllOneC(int orgid)
        {
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.User.Where(m => m.Org_ID == orgid).ToList();
                }
            }
            catch { }
            return list;
        }
        public List<SelectListItem> GetDropdownlistItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Organization> Itemsdt = new List<LinqModel.Organization>();
            Itemsdt = new DAL.Organization().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "请选择", Value = "-1" });
                
                foreach (LinqModel.Organization m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.Org_ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemwithid(int oid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Organization> Itemsdt = new List<LinqModel.Organization>();
            Itemsdt = new DAL.Organization().GetAllwithid(oid); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "请选择", Value = "-1" });

                foreach (LinqModel.Organization m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.Org_ID.ToString(), Selected = (oid==m.Org_ID) });
                }
            }
            return list ?? null;
        }
        /// <summary>
        /// 根据用户名、机构、角色联合查询用户列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="orgId"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagedList<LinqModel.User> GetPgedList(string userName, int orgId, int roleId, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.User> temp = null;
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.User select b;

                    if (!string.IsNullOrEmpty(userName))
                    {
                        data = data.Where(m => m.UserName.Contains(userName));
                    }
                    if (orgId > 0)
                    {
                        data = data.Where(m => m.Org_ID == orgId);
                    }
                    if (roleId > 0)
                    {
                        data = data.Where(m => m.Role_ID == roleId);
                    }
                    list = data.ToList();
                    temp = (PagedList<LinqModel.User>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        /// <summary>
        /// 根据用户名、机构、角色联合查询用户列表
        /// </summary>
        /// <param name="Nmae"></param>
        /// <param name="orgId"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orgid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public PagedList<LinqModel.User> GetPgedListNewUser(string Nmae, int orgId, int roleId, int pageIndex, int pageSize, int orgid, int uid)
        {
            PagedList<LinqModel.User> temp = null;
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }
                    if (orgId > 0)
                    {
                        data = data.Where(m => m.Org_ID == orgId);
                    }
                    if (roleId > 0)
                    {
                        data = data.Where(m => m.Role_ID == roleId);
                    }
                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == orgid);
                    }
                    if (!string.IsNullOrEmpty(uid.ToString()))
                    {
                        data = data.Where(m => m.ID == uid);
                    }

                    list = data.ToList();
                    temp = (PagedList<LinqModel.User>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        /// <summary>
        /// 根据用户名、机构、角色联合查询用户列表
        /// </summary>
        /// <param name="Nmae"></param>
        /// <param name="orgId"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public PagedList<LinqModel.User> GetPgedListNew(string Nmae, int orgId, int roleId, int pageIndex, int pageSize, int orgid)
        {
            PagedList<LinqModel.User> temp = null;
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }
                    if (orgId > 0)
                    {
                        data = data.Where(m => m.Org_ID == orgId);
                    }
                    if (roleId > 0)
                    {
                        data = data.Where(m => m.Role_ID == roleId);
                    }
                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == orgid);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.User>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }

        // public PagedList<LinqModel.User> GetPgedList(string Nmae, string Provincev, string City, string District, string Contact, string Tel, string Org_Code, int pageIndex, int pageSize)
        public PagedList<LinqModel.User> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.User> temp = null;
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }
                    //if (!string.IsNullOrEmpty(Provincev))
                    //{
                    //    data = data.Where(m => m.Province == Provincev);
                    //}
                    //if (!string.IsNullOrEmpty(City))
                    //{
                    //    data = data.Where(m => m.City == City);
                    //}
                    //if (!string.IsNullOrEmpty(District))
                    //{
                    //    data = data.Where(m => m.District == District);
                    //}
                    //if (!string.IsNullOrEmpty(Contact))
                    //{
                    //    data = data.Where(m => m.Name == Contact);
                    //}
                    //if (!string.IsNullOrEmpty(Tel))
                    //{
                    //    data = data.Where(m => m.Tel == Tel);
                    //}
                    //if (!string.IsNullOrEmpty(Org_Code))
                    //{
                    //    data = data.Where(m => m.Name == Org_Code);
                    //}

                    list = data.ToList();
                    temp = (PagedList<LinqModel.User>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.User> GetPgedListNewUser(string Nmae, int pageIndex, int pageSize, int orgid,int uid)
        {
            PagedList<LinqModel.User> temp = null;
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == orgid);
                    }
                    if (!string.IsNullOrEmpty(uid.ToString()))
                    {
                        data = data.Where(m => m.ID == uid);
                    }

                    list = data.ToList();
                    temp = (PagedList<LinqModel.User>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.User> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int orgid)
        {
            PagedList<LinqModel.User> temp = null;
            List<LinqModel.User> list = new List<LinqModel.User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == orgid);
                    }
                   

                    list = data.ToList();
                    temp = (PagedList<LinqModel.User>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public LinqModel.User GetModel(int? id)
        {
            LinqModel.User model = new LinqModel.User();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.User.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.User(); }
            return model;
        }
        public Common.Argument.RetResult Create(LinqModel.User model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.User.InsertOnSubmit(model);
                    dc.SubmitChanges();
                    new DAL.User().UpdateID(model);
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Edit(LinqModel.User user)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //dc.Entry(user).State = EntityState.Modified;
                    var model = dc.User.FirstOrDefault(m => m.ID == user.ID);
                    model.Org_ID = user.Org_ID;
                    model.Password = user.Password;
                    model.Photo = user.Photo;
                    model.Role_ID = user.Role_ID;
                    model.Type = user.Type;
                    model.User_Code = user.User_Code;
                    model.UserName = user.UserName;
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
                    var model = dc.User.FirstOrDefault(m => m.ID == id);
                    dc.User.DeleteOnSubmit(model);
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
                int c = dc.User.Where(m => m.UserName == name).Count();

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
    }
}
