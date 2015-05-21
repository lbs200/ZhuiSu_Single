using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_BaoZhuang_GuiGe : DALBase
    {
        public PagedList<LinqModel.Org_BaoZhuang_GuiGe> GetAllByOrgID(int orgID, int Org_BaoZhuang_ID, string name, int pInex, int pSize)
        {
            PagedList<LinqModel.Org_BaoZhuang_GuiGe> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Org_BaoZhuang_GuiGe where m.OrgID == orgID && m.Org_BaoZhuang_ID == Org_BaoZhuang_ID select m;
                    if (!string.IsNullOrEmpty(name))
                    {
                        data = data.Where(m => m.NameGuiGe.Contains(name));
                    }
                    list = (PagedList<LinqModel.Org_BaoZhuang_GuiGe>)data.ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Org_BaoZhuang_GuiGe GetModel(int id)
        {
            LinqModel.Org_BaoZhuang_GuiGe model = new LinqModel.Org_BaoZhuang_GuiGe();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_BaoZhuang_GuiGe.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.Org_BaoZhuang_GuiGe(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_BaoZhuang_GuiGe model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_BaoZhuang_GuiGe.Count(m => m.NameGuiGe == model.NameGuiGe && m.OrgID == model.OrgID && m.Org_BaoZhuang_ID == model.Org_BaoZhuang_ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else
                    {
                        dc.Org_BaoZhuang_GuiGe.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Org_BaoZhuang_GuiGe model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_BaoZhuang_GuiGe.Count(m => m.NameGuiGe == model.NameGuiGe && m.OrgID == model.OrgID && m.Org_BaoZhuang_ID == model.Org_BaoZhuang_ID && m.ID != model.ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else
                    {
                        var temp = dc.Org_BaoZhuang_GuiGe.FirstOrDefault(m => m.ID == model.ID);
                        temp.NameGuiGe = model.NameGuiGe;
                        temp.ValueGuiGe = model.ValueGuiGe;
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
                    var model = dc.Org_BaoZhuang_GuiGe.FirstOrDefault(m => m.ID == id);
                    dc.Org_BaoZhuang_GuiGe.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
