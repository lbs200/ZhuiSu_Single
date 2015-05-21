using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqModel;
namespace DAL
{
    public class View_Org_Flow : DALBase
    {
        public List<LinqModel.View_Org_Flow> GetAll()
        {
            List<LinqModel.View_Org_Flow> list = new List<LinqModel.View_Org_Flow>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Org_Flow.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.View_Org_Flow> GetList(string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.View_Org_Flow> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Org_Flow select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Flow_Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.View_Org_Flow>)data.OrderBy(m => m.Flow_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.View_Org_Flow GetModel(int id)
        {
            LinqModel.View_Org_Flow model = new LinqModel.View_Org_Flow();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == id);
                }
            }
            catch { model = new LinqModel.View_Org_Flow(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.View_Org_Flow model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.View_Org_Flow.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.View_Org_Flow model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.View_Org_Flow.FirstOrDefault(m => m.Flow_ID == model.Flow_ID);
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
                    var model = dc.View_Org_Flow.FirstOrDefault(m => m.Flow_ID == id);
                    dc.View_Org_Flow.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
