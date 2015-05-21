using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqModel;
namespace DAL
{
    public class View_Flow_Users : DALBase
    {
        public List<LinqModel.View_Flow_User> GetAll()
        {
            List<LinqModel.View_Flow_User> list = new List<LinqModel.View_Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Flow_User.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.View_Flow_User> GetAllwithoid(int oid)
        {
            List<LinqModel.View_Flow_User> list = new List<LinqModel.View_Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Flow_User.Where(m=>m.Org_ID==oid).ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.View_Flow_User> GetList(string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.View_Flow_User> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Flow_User select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Flow_Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.View_Flow_User>)data.OrderBy(m => m.Flow_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.View_Flow_User GetModel(int id)
        {
            LinqModel.View_Flow_User model = new LinqModel.View_Flow_User();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Flow_User.FirstOrDefault(m => m.Org_Flow_ID == id);
                }
            }
            catch { model = new LinqModel.View_Flow_User(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.View_Flow_User model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.View_Flow_User.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.View_Flow_User model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.View_Flow_User.FirstOrDefault(m => m.Flow_ID == model.Flow_ID);
                    temp.Abbr = model.Abbr;
                    temp.Flow_Description = model.Flow_Description;
                    temp.Flow_Name = model.Flow_Name;
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
                    var model = dc.View_Flow_User.FirstOrDefault(m => m.Flow_ID == id);
                    dc.View_Flow_User.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
        public PagedList<LinqModel.View_Flow_User> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.View_Flow_User> temp = null;
            List<LinqModel.View_Flow_User> list = new List<LinqModel.View_Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.View_Flow_User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.PName.ToString().Contains(Nmae));
                    }
                    //(int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]])


                    list = data.ToList();
                    temp = (PagedList<LinqModel.View_Flow_User>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.View_Flow_User> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int orgid)
        {
            PagedList<LinqModel.View_Flow_User> temp = null;
            List<LinqModel.View_Flow_User> list = new List<LinqModel.View_Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.View_Flow_User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.PName.ToString().Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == orgid);
                    }
                    //(int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]])


                    list = data.ToList();
                    temp = (PagedList<LinqModel.View_Flow_User>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }

    }
}
