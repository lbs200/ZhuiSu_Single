using LinqModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Role : DALBase
    {
        public List<LinqModel.Roles> GetAll()
        {
            List<LinqModel.Roles> list = new List<LinqModel.Roles>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Roles.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.Roles> GetAllWithJB(int rid)
        {
            List<LinqModel.Roles> list = new List<LinqModel.Roles>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    LinqModel.Roles rr = new DAL.Role().GetModel(rid);
                    list = dc.Roles.Where(m=>m.Jibie.Contains(rr.Jibie)).ToList();
                }
            }
            catch { }
            return list;
        }

        // public PagedList<LinqModel.Roles> GetPgedList(string Nmae, string Provincev, string City, string District, string Contact, string Tel, string Org_Code, int pageIndex, int pageSize)
        public PagedList<LinqModel.Roles> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.Roles> temp = null;
            List<LinqModel.Roles> list = new List<LinqModel.Roles>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Roles select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.RoleName.Contains(Nmae));
                    }

                    list = data.ToList();
                    temp = (PagedList<LinqModel.Roles>)list.OrderByDescending(m=>m.RoleId).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.Roles> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int rid)
        {
            PagedList<LinqModel.Roles> temp = null;
            List<LinqModel.Roles> list = new List<LinqModel.Roles>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Roles select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.RoleName.Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(rid.ToString()))
                    {
                        LinqModel.Roles rr = new DAL.Role().GetModel(rid);
                        data = data.Where(m => m.Jibie.Contains(rr.Jibie));
                    }

                    list = data.ToList();
                    temp = (PagedList<LinqModel.Roles>)list.OrderByDescending(m=>m.RoleId).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public Roles GetModel(int? id)
        {
            Roles model = new Roles();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Roles.FirstOrDefault(m => m.RoleId == id);
                }
            }
            catch { model = new Roles(); }
            return model;
        }
        public Common.Argument.RetResult Create(Roles model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Roles.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Edit(Roles Roles)
        {

            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //dc.Entry(Roles).State = EntityState.Modified;
                    var model = dc.Roles.FirstOrDefault(m => m.RoleId == Roles.RoleId);
                    model.RoleName = Roles.RoleName;
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
                    var model = dc.Roles.FirstOrDefault(m => m.RoleId == id);
                    dc.Roles.DeleteOnSubmit(model);
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
                if (dc.Roles.Where(m => m.RoleName == name) != null)
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
