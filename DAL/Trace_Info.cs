using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Trace_Info : DALBase
    {
        public List<LinqModel.View_Org_Flow> GetList(int Org_ID)
        {
            List<LinqModel.View_Org_Flow> list = new List<LinqModel.View_Org_Flow>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Org_Flow.Where(m => m.Org_ID == Org_ID).OrderBy(m => m.Seq_No).ToList();
                }
            }
            catch { }
            return list;
        }
        public IPagedList<LinqModel.View_Trace_Info> GetList(string flowNum, int orgFlowID, int prodID, string timeS, string timeE, int pInex, int pSize)
        {
            IPagedList<LinqModel.View_Trace_Info> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Trace_Info where m.FlowOver == false && m.Prod_ID == prodID && m.Org_Flow_ID == orgFlowID select m;
                    //var data = from m in dc.View_Trace_Info where m.Prod_ID == prodID  select m;
                    if (!string.IsNullOrEmpty(flowNum))
                    {
                        data = data.Where(m => m.Flow_Num == flowNum);
                    }
                    else
                    {
                        data = data.Where(m => m.Prod_Code_Start == 0);
                        if (!string.IsNullOrEmpty(timeS))
                        {
                            data = data.Where(m => m.Rec_Time >= DateTime.Parse(timeS));
                        }
                        if (!string.IsNullOrEmpty(timeE))
                        {
                            data = data.Where(m => m.Rec_Time < DateTime.Parse(timeE).AddDays(1));
                        }
                    }
                    list = data.OrderByDescending(m => m.Trace_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        public LinqModel.View_Trace_Info GetModel(string flowNum, int orgFlowID, int prodID)
        {
            LinqModel.View_Trace_Info model = new LinqModel.View_Trace_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Trace_Info.FirstOrDefault(m => m.Prod_ID == prodID && m.Org_Flow_ID == orgFlowID && m.Flow_Num == flowNum);
                }
            }
            catch { model = new LinqModel.View_Trace_Info(); }
            return model;
        }
        public LinqModel.View_Trace_Info GetModel(int id)
        {
            LinqModel.View_Trace_Info model = new LinqModel.View_Trace_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Trace_Info.FirstOrDefault(m => m.Trace_ID == id);
                }
            }
            catch { model = new LinqModel.View_Trace_Info(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Trace_Info model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Trace_Info.InsertOnSubmit(model);
                    if (model.Trace_Info_Value.Descendants("upFlowOver").FirstOrDefault().Attribute("value").Value == "true")
                    {
                        var modelUpOver = dc.Trace_Info.FirstOrDefault(m => m.Trace_ID == model.Trace_ID_Up);
                        modelUpOver.FlowOver = true;
                    }
                    dc.SubmitChanges();
                    if (model.Trace_ID_Up == 0)
                    {
                        model.Trace_ID_List = "|" + model.Trace_ID + "|";
                    }
                    else
                    {
                        var modelUp = dc.Trace_Info.FirstOrDefault(m => m.Trace_ID == model.Trace_ID_Up);
                        model.Trace_ID_List = modelUp.Trace_ID_List + model.Trace_ID + "|";
                        //modelUp.NextFlowID = "";
                    }
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public bool IsUsed(int prodID, int ewmStart, int ewmEnd)
        {
            bool result = false;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Trace_Info.Where(m => m.Prod_ID == prodID).Count(m => m.Prod_Code_Start <= ewmStart && m.Prod_Code_End >= ewmStart) > 0)
                    {
                        return true;
                    }
                    if (dc.Trace_Info.Where(m => m.Prod_ID == prodID).Count(m => m.Prod_Code_Start <= ewmEnd && m.Prod_Code_End >= ewmEnd) > 0)
                    {
                        return true;
                    }
                }
            }
            catch { result = true; }
            return result;
        }
        public bool IsOverMax(int prodID, int ewmStart, int ewmEnd)
        {
            bool result = false;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Products.FirstOrDefault(m => m.ID == prodID);
                    if (model.Max_SN == null)
                    {
                        result = true;
                    }
                    else
                    {
                        if (ewmEnd > model.Max_SN)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch { result = true; }
            return result;
        }
        public IPagedList<LinqModel.View_Trace_Info> GetListFlow(bool isOver, int Org_ID, string flowNum, int prodID, int orgFlowID, string timeS, string timeE, int pInex, int pSize)
        {
            IPagedList<LinqModel.View_Trace_Info> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //var listProds = from m in dc.Products where m.Org_ID == Org_ID select m.ID;
                    //var data = from m in dc.View_Trace_Info where m.FlowOver == isOver && m.Prod_Code_Start == 0 && m.Prod_Code_End == 0 && listProds.Contains((int)m.Prod_ID) select m;
                    var data = from m in dc.View_Trace_Info where m.FlowOver == isOver && m.Prod_Code_Start == 0 && m.Prod_Code_End == 0 select m;
                    if (!string.IsNullOrEmpty(flowNum))
                    {
                        data = data.Where(m => m.Flow_Num.Contains(flowNum));
                    }
                    else
                    {
                        if (prodID > 0)
                        {
                            data = data.Where(m => m.Prod_ID == prodID);
                        }
                        if (orgFlowID > 0)
                        {
                            data = data.Where(m => m.Org_Flow_ID == orgFlowID);
                        }
                        if (!string.IsNullOrEmpty(timeS))
                        {
                            data = data.Where(m => m.Rec_Time >= DateTime.Parse(timeS));
                        }
                        if (!string.IsNullOrEmpty(timeE))
                        {
                            data = data.Where(m => m.Rec_Time < DateTime.Parse(timeE).AddDays(1));
                        }
                    }
                    list = data.OrderByDescending(m => m.Trace_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        public IPagedList<LinqModel.View_Trace_Info> GetListFlowEWM(int Org_ID, string ewm, string timeS, string timeE, int pInex, int pSize)
        {
            IPagedList<LinqModel.View_Trace_Info> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var listProds = from m in dc.Products where m.Org_ID == Org_ID select m.ID;
                    var data = from m in dc.View_Trace_Info where m.Prod_Code_Start > 0 && m.Prod_Code_End > 0 && listProds.Contains((int)m.Prod_ID) select m;
                    if (!string.IsNullOrEmpty(ewm))
                    {
                        string[] strs = ewm.Split('.');
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < strs.Length; i++)
                        {
                            if (i < strs.Length - 1)
                                sb.Append(strs[i] + ".");
                        }
                        int num = 0;
                        if (int.TryParse(strs[strs.Length - 1], out num))
                            data = data.Where(m => m.Prod_Code_Before == sb.ToString() && m.Prod_Code_Start <= num && m.Prod_Code_End >= num);
                        else
                            data = data.Where(m => m.Prod_Code_Before == "0");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(timeS))
                        {
                            data = data.Where(m => m.Rec_Time >= DateTime.Parse(timeS));
                        }
                        if (!string.IsNullOrEmpty(timeE))
                        {
                            data = data.Where(m => m.Rec_Time < DateTime.Parse(timeE).AddDays(1));
                        }
                    }
                    list = data.OrderByDescending(m => m.Trace_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        public LinqModel.View_Trace_Info GetModelByEwm(long ewmNum, int prodID)
        {
            LinqModel.View_Trace_Info model = new LinqModel.View_Trace_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Trace_Info.FirstOrDefault(m => m.Prod_ID == prodID && m.Prod_Code_Start <= ewmNum && m.Prod_Code_End >= ewmNum);
                }
            }
            catch { model = new LinqModel.View_Trace_Info(); }
            return model;
        }
        public LinqModel.Trace_Info GetModelByEwm(long ewmNum, int prodID, int orgFlowID)
        {
            LinqModel.Trace_Info model = new LinqModel.Trace_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Trace_Info.FirstOrDefault(m => m.Prod_ID == prodID && m.Org_Flow_ID == orgFlowID && m.Prod_Code_Start == ewmNum && m.Prod_Code_End == ewmNum);
                }
            }
            catch { model = new LinqModel.Trace_Info(); }
            return model;
        }
        public List<LinqModel.Trace_Info_Pics_New> GetList(List<int> traceID,out List<LinqModel.Trace_Info_Pics_New> listCycle)
        {
            List<LinqModel.Trace_Info_Pics_New> list = new List<LinqModel.Trace_Info_Pics_New>();
            listCycle = new List<LinqModel.Trace_Info_Pics_New>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    List<int> listFlows = new List<int>();
                    foreach (var temp in traceID)
                    {
                        var modelTraceInfo = dc.View_Trace_Info.FirstOrDefault(m => m.Trace_ID == temp && m.Prod_Code_Start == 0);
                        #region 添加列表
                        
                        //var model = dc.ExecuteQuery<LinqModel.View_Trace_Info_Pics>("select * from View_Trace_Info_Pics where [Prod_Code_Start]=0 and [Org_Flow_ID]=" + modelTraceInfo.Org_Flow_ID + " and CONVERT(varchar(100), [Up_Time], 23)='" + modelTraceInfo.Rec_Time.ToString("yyyy-MM-dd") + "' ").FirstOrDefault();
                        //if (model != null && model.ID > 0)
                        //    list.Add(model);

                        var model = new LinqModel.Trace_Info_Pics_New();
                        model.Flow_Name = modelTraceInfo.Flow_Name;
                        model.ID = modelTraceInfo.Trace_ID;
                        model.Org_Flow_ID = (int)modelTraceInfo.Org_Flow_ID;
                        model.Prod_Code_Start = modelTraceInfo.Prod_Code_Start;
                        model.Trace_ID = modelTraceInfo.Trace_ID;
                        model.Trace_Info_Value = modelTraceInfo.Trace_Info_Value;
                        if (modelTraceInfo.Trace_Info_Value.Descendants("img").Count() > 0)
                        {
                            var img = modelTraceInfo.Trace_Info_Value.Descendants("img").FirstOrDefault();
                            if (img != null)
                            {
                                model.Up_Time = modelTraceInfo.Rec_Time;
                                model.Pic_Description = "";
                                model.Pic_Path = img.Attribute("InfoValue").Value;
                                model.ID = 0;
                            }
                        }
                        else
                        {
                            //var modelFlowPics = dc.ExecuteQuery<LinqModel.Flow_Pics>("select * from Flow_Pics where  [Org_Flow_ID]=" + modelTraceInfo.Org_Flow_ID + " and CONVERT(varchar(100), [Up_Time], 23)='" + modelTraceInfo.Rec_Time.ToString("yyyy-MM-dd") + "' ").Take(4).ToList();
                            var modelFlowPics = dc.ExecuteQuery<LinqModel.Flow_Pics>("select * from Flow_Pics where  [Org_Flow_ID]=" + modelTraceInfo.Org_Flow_ID + " and CONVERT(varchar(100), [Up_Time], 23)<='" + modelTraceInfo.Rec_Time.ToString("yyyy-MM-dd") + "' and CONVERT(varchar(100), [Up_Time], 23)>='" + modelTraceInfo.Rec_Time.AddDays(-5).ToString("yyyy-MM-dd") + "' ").Take(4).ToList();
                            if (modelFlowPics != null && modelFlowPics.Count > 0)
                            {
                                model.Up_Time = modelFlowPics[0].Up_Time;
                                model.Pic_Description = "";
                                model.Pic_Path = "";
                                foreach (var m in modelFlowPics)
                                {
                                    model.Pic_Description += m.Pic_Description + "$";
                                    model.Pic_Path += m.Pic_Path + "$";
                                }
                                model.ID = modelFlowPics[0].ID;
                            }
                        }
                        #endregion
                        if (!listFlows.Contains((int)modelTraceInfo.Org_Flow_ID))
                        {
                            listFlows.Add((int)modelTraceInfo.Org_Flow_ID);
                            list.Add(model);
                        }
                        else
                        {
                            listCycle.Add(model);
                        }
                    }
                }
            }
            catch { }
            return list;
        }
        public bool Exsist(int prodID)
        {
            bool result = false;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var list = dc.Trace_Info.Where(m => m.Prod_ID == prodID);
                    if (list != null && list.Count() > 0)
                    {
                        result = true;
                    }
                }
            }
            catch { result = false; }
            return result;
        }
        public List<LinqModel.View_Trace_Info> GetListSearchPoint(bool isOver, int prodID, int orgFlowID, string timeS, string timeE, string strText, string strCheck, string strRadio, string strSelect)
        {
            List<LinqModel.View_Trace_Info> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append("select * from View_Trace_Info where FlowOver='" + isOver + "' and Prod_ID=" + prodID + " and Org_Flow_ID=" + orgFlowID + " and Prod_Code_Start=0 and Rec_Time<='" + DateTime.Parse(timeE).AddDays(1).ToString("yyyy-MM-dd") + "' and Rec_Time>='" + timeS + "' ");
                    if (!string.IsNullOrEmpty(strText))
                    {
                        string[] ss = strText.Split('|');
                        foreach (var s in ss)
                        {
                            string[] dd = s.Split(',');
                            if (dd.Length == 2 && !string.IsNullOrEmpty(dd[0]) && !string.IsNullOrEmpty(dd[1]))
                            {
                                sbSql.Append(" and Trace_Info_Value.exist('//info[@InfoID=\"" + dd[0] + "\" and @InfoValue=\"" + dd[1] + "\"]')=1");
                                //sbSql.Append(" and Trace_Info_Value.exist('//info[@InfoID=\"" + dd[0] + "\" and contains(@InfoValue,\"" + dd[1] + "\")]')=1");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(strRadio))
                    {
                        string[] ss = strRadio.Split('|');
                        foreach (var s in ss)
                        {
                            string[] dd = s.Split(',');
                            if (dd.Length == 2 && !string.IsNullOrEmpty(dd[0]) && !string.IsNullOrEmpty(dd[1]))
                            {
                                if (dd[1] != "0")
                                    sbSql.Append(" and Trace_Info_Value.exist('//info[@InfoID=\"" + dd[0] + "\" and @InfoValue=\"" + dd[1] + "\"]')=1");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(strSelect))
                    {
                        string[] ss = strSelect.Split('|');
                        foreach (var s in ss)
                        {
                            string[] dd = s.Split(',');
                            if (dd.Length == 2 && !string.IsNullOrEmpty(dd[0]) && !string.IsNullOrEmpty(dd[1]))
                            {
                                if (dd[1] != "请选择")
                                    sbSql.Append(" and Trace_Info_Value.exist('//info[@InfoID=\"" + dd[0] + "\" and @InfoValue=\"" + dd[1] + "\"]')=1");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(strCheck))
                    {
                        sbSql.Append(" and ( ");
                        string[] ss = strCheck.Split('|');
                        for (int i = 0; i < ss.Length; i++)
                        {
                            string[] dd = ss[i].Split(',');
                            if (dd.Length == 2 && !string.IsNullOrEmpty(dd[0]) && !string.IsNullOrEmpty(dd[1]))
                            {
                                if (i == ss.Length - 1)
                                {
                                    sbSql.Append("  Trace_Info_Value.exist('//info[@InfoID=\"" + dd[0] + "\" and value=\"" + dd[1] + "\"]')=1 ");
                                }
                                else
                                    sbSql.Append("  Trace_Info_Value.exist('//info[@InfoID=\"" + dd[0] + "\" and value=\"" + dd[1] + "\"]')=1 or ");
                            }
                        }

                        sbSql.Append(" )");
                        #region OLD
                        //string[] ss = strCheck.Split('|');
                        //List<string> listCheckID = new List<string>();
                        //List<string> listCheckValue = new List<string>();

                        //for (int i = 0; i < ss.Length; i++)
                        //{
                        //    string[] dd = ss[i].Split(',');
                        //    if (dd.Length == 2 && !string.IsNullOrEmpty(dd[0]) && !string.IsNullOrEmpty(dd[1]))
                        //    {
                        //        if (listCheckID.Contains(dd[0]))
                        //        {
                        //            listCheckValue[listCheckID.IndexOf(dd[0])] += "," + dd[1];
                        //        }
                        //        else
                        //        {
                        //            listCheckID.Add(dd[0]);
                        //            listCheckValue.Add(dd[1]);
                        //        }
                        //    }
                        //}
                        //for (int i = 0; i < listCheckID.Count; i++)
                        //{
                        //    sbSql.Append(" and Trace_Info_Value.exist('//info[@InfoID=\"" + listCheckID[i] + "\" and  @InfoValue=\"" + listCheckValue[i] + "\"]')=1");
                        //    //sbSql.Append(" and Trace_Info_Value.exist('//info[@InfoID=\"" + listCheckID[i] + "\" and  contains(@InfoValue,\"" + listCheckValue[i] + "\")]')=1");
                        //} 
                        #endregion
                    }
                    sbSql.Append(" order by Trace_ID desc");

                    list = dc.ExecuteQuery<LinqModel.View_Trace_Info>(sbSql.ToString()).ToList();
                }
            }
            catch { }
            return list;
        }
        public IPagedList<LinqModel.View_Trace_Info> GetUpSepList(string flowNum, int orgFlowID, string nextOrgFlowID, int prodID, string timeS, string timeE, int pInex, int pSize)
        {
            IPagedList<LinqModel.View_Trace_Info> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Trace_Info where m.FlowOver == false && (m.Prod_ID == prodID && m.Org_Flow_ID == orgFlowID && (m.NextFlowID == null || m.NextFlowID == "")) || (m.NextFlowID == nextOrgFlowID) select m;
                    //var data = from m in dc.View_Trace_Info where m.Prod_ID == prodID  select m;
                    if (!string.IsNullOrEmpty(flowNum))
                    {
                        data = data.Where(m => m.Flow_Num == flowNum);
                    }
                    else
                    {
                        data = data.Where(m => m.Prod_Code_Start == 0);
                        if (!string.IsNullOrEmpty(timeS))
                        {
                            data = data.Where(m => m.Rec_Time >= DateTime.Parse(timeS));
                        }
                        if (!string.IsNullOrEmpty(timeE))
                        {
                            data = data.Where(m => m.Rec_Time < DateTime.Parse(timeE).AddDays(1));
                        }
                    }
                    list = data.OrderByDescending(m => m.Trace_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
    }
}
