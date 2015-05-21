using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_UsageType : DALBase
    {
        public PagedList<LinqModel.Org_UsageType> GetList(int orgID, string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.Org_UsageType> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Org_UsageType where m.OrgID == orgID select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.TypeName.Contains(mName));
                    }
                    list = (PagedList<LinqModel.Org_UsageType>)data.ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Org_UsageType GetModel(int id)
        {
            LinqModel.Org_UsageType model = new LinqModel.Org_UsageType();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_UsageType.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.Org_UsageType(); }
            return model;
        }
        public LinqModel.Org_UsageType GetModel(int orgID, string mName)
        {
            LinqModel.Org_UsageType model = new LinqModel.Org_UsageType();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_UsageType.FirstOrDefault(m => m.OrgID == orgID && m.TypeName == mName);
                }
            }
            catch { model = new LinqModel.Org_UsageType(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_UsageType model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_UsageType.Count(m => m.OrgID == model.OrgID && m.TypeName == model.TypeName) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else
                    {
                        model.MaxSN = 0;
                        dc.Org_UsageType.InsertOnSubmit(model);
                        dc.SubmitChanges();
                        var modelOrg = dc.Organization.FirstOrDefault(m => m.Org_ID == model.OrgID);
                        model.Code = modelOrg.Org_Code + ".2" + model.ID;
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Org_UsageType model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_UsageType.Count(m => m.OrgID == model.OrgID && m.TypeName == model.TypeName && m.ID != model.ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else
                    {
                        var temp = dc.Org_UsageType.FirstOrDefault(m => m.ID == model.ID);
                        temp.TypeName = model.TypeName;
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
                    var model = dc.Org_UsageType.FirstOrDefault(m => m.ID == id);
                    dc.Org_UsageType.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}