using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_Info : DALBase
    {
        public List<LinqModel.View_Org_Info> GetList(int Org_Flow_ID)
        {
            List<LinqModel.View_Org_Info> list = new List<LinqModel.View_Org_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Org_Info.Where(m => m.Org_Flow_ID == Org_Flow_ID).ToList();
                }
            }
            catch { list = new List<LinqModel.View_Org_Info>(); }
            return list;
        }

        public Common.Argument.RetResult Add(int org_flow_id, string ids)
        {
            Ret.Msg = "操作成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var listOld = dc.Org_Info.Where(m => m.Org_Flow_ID == org_flow_id);
                    if (string.IsNullOrEmpty(ids))
                    {
                        dc.Org_Info.DeleteAllOnSubmit(listOld);
                    }
                    else
                    {

                        List<int> listID = new List<int>();
                        string str = ids.Replace("][", "|").Replace("[", "").Replace("]", "");
                        foreach (var m in str.Split('|'))
                        {
                            LinqModel.Org_Info model = new LinqModel.Org_Info();
                            model.Info_ID = int.Parse(m.Split(',')[0]);
                            model.Org_Flow_ID = org_flow_id;
                            model.Public = int.Parse(m.Split(',')[1]);
                            //model.Data_Type_Value = "";
                            if (listOld.Count(x => x.Org_Flow_ID == org_flow_id && x.Info_ID == model.Info_ID) <= 0)
                            {
                                dc.Org_Info.InsertOnSubmit(model);
                            }
                            listID.Add(model.Info_ID);
                        }
                        dc.Org_Info.DeleteAllOnSubmit(listOld.Where(m => !listID.Contains(m.Info_ID)));
                    }
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_Info model)
        {
            Ret.Msg = "操作成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var modelMetaInfo = dc.Meta_Info.FirstOrDefault(m => m.Info_ID == model.Info_ID);
                    if (modelMetaInfo.Data_Type == "用途码")
                    {
                        if (dc.Org_UsageType.Count(m => m.TypeName == modelMetaInfo.Info_Name) > 0)
                        {
                            dc.Org_Info.InsertOnSubmit(model);
                            dc.SubmitChanges();
                        }
                        else
                        {
                            Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败，请先添加企业的用途码类型信息，用途码类型名称必须与信息元名称相同！");
                        }
                    }
                    else
                    {
                        dc.Org_Info.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Update(LinqModel.Org_Info model)
        {
            Ret.Msg = "操作成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Org_Info.FirstOrDefault(m => m.ID == model.ID);
                    temp.Data_Type_Value = model.Data_Type_Value;
                    temp.Search_Point = model.Search_Point;
                    temp.Required = model.Required;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败，请重试！"); }
            return Ret;
        }

        public LinqModel.View_Org_Info GetModel(int id)
        {
            var result = new LinqModel.View_Org_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    result = dc.View_Org_Info.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return result;
        }

        public LinqModel.Org_Info GetModelM(int id)
        {
            var result = new LinqModel.Org_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    result = dc.Org_Info.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return result;
        }

        public List<LinqModel.View_Org_Info> GetSearchPoint(int orgFlowID)
        {
            List<LinqModel.View_Org_Info> list = new List<LinqModel.View_Org_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Org_Info.Where(m => m.Org_Flow_ID == orgFlowID && m.Search_Point == true).ToList();
                }
            }
            catch { list = new List<LinqModel.View_Org_Info>(); }
            return list;
        }

        public Common.Argument.RetResult Del(int id)
        {
            Ret.Msg = "操作成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Info.FirstOrDefault(m => m.ID == id);
                    dc.Org_Info.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败，请重试！"); }
            return Ret;
        }
    }
}
