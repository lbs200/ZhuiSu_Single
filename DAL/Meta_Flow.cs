using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Meta_Flow : DALBase
    {
        public List<LinqModel.Meta_Flow> GetAll()
        {
            List<LinqModel.Meta_Flow> list = new List<LinqModel.Meta_Flow>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Meta_Flow.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.Meta_Flow> GetList(string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.Meta_Flow> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Meta_Flow select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Flow_Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.Meta_Flow>)data.OrderBy(m => m.Flow_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Meta_Flow GetModel(int id)
        {
            LinqModel.Meta_Flow model = new LinqModel.Meta_Flow();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Meta_Flow.FirstOrDefault(m => m.Flow_ID == id);
                }
            }
            catch { model = new LinqModel.Meta_Flow(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Meta_Flow model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Meta_Flow.Count(m => m.Flow_Name == model.Flow_Name) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else if (dc.Meta_Flow.Count(m => m.Abbr == model.Abbr) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，前缀编码重复！");
                    }
                    else
                    {
                        dc.Meta_Flow.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Meta_Flow model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Meta_Flow.Count(m => m.Flow_Name == model.Flow_Name && m.Flow_ID != model.Flow_ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else if (dc.Meta_Flow.Count(m => m.Abbr == model.Abbr && m.Flow_ID != model.Flow_ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，前缀编码重复！");
                    }
                    else
                    {
                        var temp = dc.Meta_Flow.FirstOrDefault(m => m.Flow_ID == model.Flow_ID);
                        temp.Abbr = model.Abbr;
                        temp.Flow_Description = model.Flow_Description;
                        temp.Flow_Name = model.Flow_Name;
                        dc.SubmitChanges();
                    }
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
                    var model = dc.Meta_Flow.FirstOrDefault(m => m.Flow_ID == id);
                    dc.Meta_Flow.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
