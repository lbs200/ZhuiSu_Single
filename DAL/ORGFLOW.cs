using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ORGFLOW : DALBase
    {
        public List<LinqModel.ORGFLOW> GetAll()
        {
            List<LinqModel.ORGFLOW> list = new List<LinqModel.ORGFLOW>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORGFLOW.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.ORGFLOW> GetList(string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.ORGFLOW> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.ORGFLOW select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Flow_Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.ORGFLOW>)data.OrderBy(m => m.Flow_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.ORGFLOW GetModel(int id)
        {
            LinqModel.ORGFLOW model = new LinqModel.ORGFLOW();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.ORGFLOW.FirstOrDefault(m => m.Org_Flow_ID == id);
                }
            }
            catch { model = new LinqModel.ORGFLOW(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.ORGFLOW model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.ORGFLOW.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.ORGFLOW model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.ORGFLOW.FirstOrDefault(m => m.Flow_ID == model.Flow_ID);
                    temp.Abbr = model.Abbr;

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
                    var model = dc.ORGFLOW.FirstOrDefault(m => m.Flow_ID == id);
                    dc.ORGFLOW.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
        public PagedList<LinqModel.ORGFLOW> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.ORGFLOW> temp = null;
            List<LinqModel.ORGFLOW> list = new List<LinqModel.ORGFLOW>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ORGFLOW select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.PName.ToString().Contains(Nmae));
                    }
                    //(int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]])


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ORGFLOW>)list.OrderByDescending(m=>m.Flow_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.ORGFLOW> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int orgid)
        {
            PagedList<LinqModel.ORGFLOW> temp = null;
            List<LinqModel.ORGFLOW> list = new List<LinqModel.ORGFLOW>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ORGFLOW select b;

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
                    temp = (PagedList<LinqModel.ORGFLOW>)list.OrderByDescending(m=>m.Flow_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }


    }
}
