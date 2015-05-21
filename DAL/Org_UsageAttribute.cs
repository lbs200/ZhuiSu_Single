using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_UsageAttribute : DALBase
    {
        public List<LinqModel.Org_UsageAttribute> GetList(int Org_UsageInfo_ID)
        {
            List<LinqModel.Org_UsageAttribute> list = new List<LinqModel.Org_UsageAttribute>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Org_UsageAttribute where m.Org_UsageInfo_ID == Org_UsageInfo_ID && m.Status == 0 select m;
                   
                    list = data.ToList();
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Org_UsageAttribute GetModel(int id)
        {
            LinqModel.Org_UsageAttribute model = new LinqModel.Org_UsageAttribute();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_UsageAttribute.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.Org_UsageAttribute(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_UsageAttribute model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_UsageAttribute.Count(m => m.Org_UsageInfo_ID == model.Org_UsageInfo_ID && m.AttributeName == model.AttributeName && m.Status == 0) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else
                    {
                        dc.Org_UsageAttribute.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Org_UsageAttribute model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Org_UsageAttribute.Count(m => m.Org_UsageInfo_ID == model.Org_UsageInfo_ID && m.AttributeName == model.AttributeName && m.ID != model.ID && m.Status == 0) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else
                    {
                        var temp = dc.Org_UsageAttribute.FirstOrDefault(m => m.ID == model.ID);
                        var modelOld = new LinqModel.Org_UsageAttribute();
                        modelOld.AttributeName = temp.AttributeName;
                        modelOld.AttributeValue = temp.AttributeValue;
                        modelOld.Org_UsageInfo_ID = temp.Org_UsageInfo_ID;
                        modelOld.Status = 1;
                        modelOld.TimeAdd = temp.TimeAdd;
                        dc.Org_UsageAttribute.InsertOnSubmit(modelOld);
                        temp.Status = 0;
                        temp.TimeAdd = Common.Argument.Public.GetDateTimeNow();
                        temp.AttributeName = model.AttributeName;
                        temp.AttributeValue = model.AttributeValue;
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
                    var model = dc.Org_UsageAttribute.FirstOrDefault(m => m.ID == id);
                    model.Status = 2;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
