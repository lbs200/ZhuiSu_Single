using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_UsageInfo : DALBase
    {
        public PagedList<LinqModel.View_Org_UsageInfo> GetList(int orgID, int typeID, string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.View_Org_UsageInfo> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Org_UsageInfo where m.OrgID == orgID && m.Status == 0 select m;
                    if (typeID > 0)
                    {
                        data = data.Where(m => m.Org_UsageType_ID == typeID);
                    }
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.View_Org_UsageInfo>)data.ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.View_Org_UsageInfo GetModel(int id)
        {
            LinqModel.View_Org_UsageInfo model = new LinqModel.View_Org_UsageInfo();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Org_UsageInfo.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.View_Org_UsageInfo(); }
            return model;
        }
        public LinqModel.View_Org_UsageInfo GetModel(string ewm)
        {
            LinqModel.View_Org_UsageInfo model = new LinqModel.View_Org_UsageInfo();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Org_UsageInfo.FirstOrDefault(m => m.Code == ewm);
                }
            }
            catch { model = new LinqModel.View_Org_UsageInfo(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_UsageInfo model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_UsageInfo.Count(m => m.OrgID == model.OrgID && m.Org_UsageType_ID == model.Org_UsageType_ID && m.Name == model.Name && m.Status == 0) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else
                    {
                        dc.Org_UsageInfo.InsertOnSubmit(model);
                        dc.SubmitChanges();
                        var modelOrg = dc.Org_UsageType.FirstOrDefault(m => m.ID == model.Org_UsageType_ID);
                        modelOrg.MaxSN += 1;
                        model.Code = modelOrg.Code + "." + modelOrg.MaxSN;
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Org_UsageInfo model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_UsageInfo.Count(m => m.OrgID == model.OrgID && m.Org_UsageType_ID == model.Org_UsageType_ID && m.Name == model.Name && m.ID != model.ID && m.Status == 0) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else
                    {
                        var temp = dc.Org_UsageInfo.FirstOrDefault(m => m.ID == model.ID);
                        var modelOld = new LinqModel.Org_UsageInfo();
                        modelOld.Code = temp.Code;
                        modelOld.Memo = temp.Memo;
                        modelOld.Name = temp.Name;
                        modelOld.Org_UsageType_ID = temp.Org_UsageType_ID;
                        modelOld.OrgID = temp.OrgID;
                        modelOld.Status = 1;
                        modelOld.TimeAdd = temp.TimeAdd;
                        dc.Org_UsageInfo.InsertOnSubmit(modelOld);
                        temp.Status = 0;
                        temp.TimeAdd = Common.Argument.Public.GetDateTimeNow();
                        temp.Name = model.Name;
                        temp.Memo = model.Memo;
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
                    var model = dc.Org_UsageInfo.FirstOrDefault(m => m.ID == id);
                    model.Status = 2;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
