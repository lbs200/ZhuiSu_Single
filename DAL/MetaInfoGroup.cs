using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MetaInfoGroup : DALBase
    {
        public List<LinqModel.MetaInfoGroup> GetAll()
        {
            List<LinqModel.MetaInfoGroup> list = new List<LinqModel.MetaInfoGroup>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.MetaInfoGroup.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.MetaInfoGroup> GetList(string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.MetaInfoGroup> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.MetaInfoGroup select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Group_Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.MetaInfoGroup>)data.OrderByDescending(m => m.Group_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.MetaInfoGroup GetModel(int id)
        {
            LinqModel.MetaInfoGroup model = new LinqModel.MetaInfoGroup();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.MetaInfoGroup.FirstOrDefault(m => m.Group_ID == id);
                }
            }
            catch { model = new LinqModel.MetaInfoGroup(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.MetaInfoGroup model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.MetaInfoGroup.Count(m => m.Group_Name == model.Group_Name) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else
                    {
                        dc.MetaInfoGroup.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.MetaInfoGroup model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.MetaInfoGroup.Count(m => m.Group_Name == model.Group_Name && m.Group_ID != model.Group_ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else
                    {
                        var temp = dc.MetaInfoGroup.FirstOrDefault(m => m.Group_ID == model.Group_ID);
                        temp.Group_Description = model.Group_Description;
                        temp.Group_Name = model.Group_Name;
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
                    var model = dc.MetaInfoGroup.FirstOrDefault(m => m.Group_ID == id);
                    dc.MetaInfoGroup.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
