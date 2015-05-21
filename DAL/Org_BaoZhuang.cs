using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_BaoZhuang : DALBase
    {
        public PagedList<LinqModel.Org_BaoZhuang> GetAllByOrgID(int orgID, string name, int pInex, int pSize)
        {
            PagedList<LinqModel.Org_BaoZhuang> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Org_BaoZhuang where m.OrgID == orgID select m;
                    if (!string.IsNullOrEmpty(name))
                    {
                        data = data.Where(m => m.Name.Contains(name));
                    }
                    list = (PagedList<LinqModel.Org_BaoZhuang>)data.ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Org_BaoZhuang GetModel(int id)
        {
            LinqModel.Org_BaoZhuang model = new LinqModel.Org_BaoZhuang();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_BaoZhuang.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.Org_BaoZhuang(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_BaoZhuang model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_BaoZhuang.Count(m => m.Name == model.Name && m.OrgID == model.OrgID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else
                    {
                        dc.Org_BaoZhuang.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Org_BaoZhuang model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_BaoZhuang.Count(m => m.Name == model.Name && m.OrgID == model.OrgID && m.ID != model.ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else
                    {
                        var temp = dc.Org_BaoZhuang.FirstOrDefault(m => m.ID == model.ID);
                        temp.Name = model.Name;
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
                    var model = dc.Org_BaoZhuang.FirstOrDefault(m => m.ID == id);
                    dc.Org_BaoZhuang.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
