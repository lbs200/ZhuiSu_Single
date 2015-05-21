using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class Org_Flow : DALBase
    {
        public List<LinqModel.Org_Flow> GetAllOneC(int orgid)
        {
            List<LinqModel.Org_Flow> list = new List<LinqModel.Org_Flow>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Org_Flow.Where(m => m.Org_ID == orgid).ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.Org_Flow> GetAllC()
        {
            List<LinqModel.Org_Flow> list = new List<LinqModel.Org_Flow>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Org_Flow.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.View_Org_Flow> GetList(string mName, int orgID, int prodID, int pInex, int pSize)
        {
            PagedList<LinqModel.View_Org_Flow> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Org_Flow where m.Org_ID == orgID select m;
                    if (prodID > 0)
                    {
                        data = data.Where(m => m.Prod_ID == prodID);
                    }
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Flow_Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.View_Org_Flow>)data.OrderBy(m => m.Seq_No).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.View_Org_Flow_User> GetList(int userID, int orgID, int prodID, int pInex, int pSize)
        {
            PagedList<LinqModel.View_Org_Flow_User> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Org_Flow_User where m.Org_ID == orgID && m.Use_ID == userID select m;
                    if (prodID > 0)
                    {
                        data = data.Where(m => m.Prod_ID == prodID);
                    }

                    list = (PagedList<LinqModel.View_Org_Flow_User>)data.OrderBy(m => m.Seq_No).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.View_Org_Flow> GetListAndFlow(string mName, int orgID, int prodID, int pInex, int pSize, out List<string> listFlow)
        {
            PagedList<LinqModel.View_Org_Flow> list = null;
            listFlow = new List<string>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = dc.View_Org_Flow.Where(m => m.Org_ID == orgID && m.Prod_ID == prodID).OrderBy(m => m.Seq_No).ToList();

                    var dataStart = data.Where(m => m.Sup_Flow_ID == 0).ToList();
                    for (int i = 0; i < dataStart.Count; i++)
                    {

                        listFlow.Add("," + dataStart[i].Org_Flow_ID + ",");

                        FindNextFlow(listFlow, i, dataStart[i].Org_Flow_ID, data);
                    }
                    for (int i = 0; i < listFlow.Count; i++)
                    {
                        string[] tempID = listFlow[i].Split(',');
                        listFlow[i] = "";
                        foreach (var ss in tempID)
                        {
                            if (!string.IsNullOrEmpty(ss))
                            {
                                listFlow[i] += data.FirstOrDefault(m => m.Org_Flow_ID == int.Parse(ss)).Flow_Name + "&nbsp;→ &nbsp; ";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Flow_Name.Contains(mName)).ToList();
                    }
                    list = (PagedList<LinqModel.View_Org_Flow>)data.OrderBy(m => m.Seq_No).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        private void FindNextFlow(List<string> str, int p, int flowID, List<LinqModel.View_Org_Flow> data)
        {
            if (data.Count(m => m.Sup_Flow_ID == flowID) > 0)
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //LinqModel.View_Org_Flow tt = data.FirstOrDefault(m => m.Sup_Flow_ID == flowID);
                    List<LinqModel.View_Org_Flow> listTemp = data.Where(m => m.Sup_Flow_ID == flowID).ToList();
                    string xx = str[p];
                    for (int i = 0; i < listTemp.Count; i++)
                    {
                        if (i > 0)
                        {
                            str.Add(xx);
                            if (!str[str.Count - 1].Contains("," + listTemp[i].Org_Flow_ID.ToString() + ","))
                            {
                                str[str.Count - 1] += listTemp[i].Org_Flow_ID + ",";
                                FindNextFlow(str, str.Count - 1, listTemp[i].Org_Flow_ID, data);
                            }
                        }
                        else if (!str[p].Contains("," + listTemp[i].Org_Flow_ID.ToString() + ","))
                        {
                            str[p] += listTemp[i].Org_Flow_ID + ",";
                            FindNextFlow(str, p, listTemp[i].Org_Flow_ID, data);
                        }
                    }
                }
            }
        }
        public LinqModel.Org_Flow GetModel(int id)
        {
            LinqModel.Org_Flow model = new LinqModel.Org_Flow();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == id);
                }
            }
            catch { model = new LinqModel.Org_Flow(); }
            return model;
        }
        public LinqModel.View_Org_Flow GetModelView(int id)
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

        public string[] GetNextFlow(int id)
        {
            string[] result = new string[2];
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    result[0] = dc.View_Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == id).Flow_Name;
                    result[1] = "";
                    List<LinqModel.View_Org_Flow> list = dc.View_Org_Flow.Where(m => m.Sup_Flow_ID == id).ToList();
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i + 1 == list.Count)
                            {
                                result[1] += list[i].Flow_Name;
                            }
                            else
                            {
                                result[1] += list[i].Flow_Name + "，";
                            }
                        }
                    }
                    else
                    { result[1] = "无"; }
                }
            }
            catch { result = new string[2]; }
            return result;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_Flow model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    int maxOrder = 0;
                    if (dc.Org_Flow.Count(m => m.Org_ID == model.Org_ID && m.Prod_ID == model.Prod_ID) > 0)
                    {
                        maxOrder = dc.Org_Flow.Where(m => m.Org_ID == model.Org_ID && m.Prod_ID == model.Prod_ID).Max(m => m.Seq_No);
                    }
                    model.Seq_No = maxOrder + 1;

                    var listEnd = dc.Org_Flow.Where(m => m.Org_ID == model.Org_ID && m.Prod_ID == model.Prod_ID && !(from n in dc.Org_Flow where n.Org_ID == model.Org_ID && n.Prod_ID == model.Prod_ID select n.Sup_Flow_ID).Contains(m.Org_Flow_ID)).ToList();

                    if (listEnd != null && listEnd.Count > 0)
                    {
                        model.Sup_Flow_ID = listEnd[0].Org_Flow_ID;
                    }

                    dc.Org_Flow.InsertOnSubmit(model);
                    dc.SubmitChanges();
                    new DAL.Org_Flow_Cycle_Info().DelAll((int)model.Prod_ID);
                    Ret.Msg = "添加成功！";
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult UpdateNext(int orgID, int id, int nextID)
        {
            Ret.Msg = "设置成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == nextID);
                    model.Sup_Flow_ID = id;
                    dc.SubmitChanges();
                    new DAL.Org_Flow_Cycle_Info().DelAll((int)model.Prod_ID);
                    Ret.Msg = "设置成功！";
                } 
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "设置失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Del(int id)
        {
            Ret.Msg = "删除成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == id);
                    dc.Org_Flow.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                    new DAL.Org_Flow_Cycle_Info().DelAll((int)model.Prod_ID);
                    Ret.Msg = "删除成功！";
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult SepUp(int id, int orgID, int prodID)
        {
            Ret.Msg = "上移成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == id);
                    int maxSep = dc.Org_Flow.Where(m => m.Org_ID == model.Org_ID && m.Prod_ID == prodID).Min(m => m.Seq_No);
                    if (model.Seq_No == maxSep)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "上移失败，此流程已经是顶级流程！");
                    }
                    else
                    {
                        if (model.Seq_No == dc.Org_Flow.Where(m => m.Org_ID == model.Org_ID && m.Prod_ID == prodID).Max(m => m.Seq_No))
                        {
                            int sep = model.Seq_No;
                            var model2 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No - 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            var model3 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No - 2 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            model.Seq_No -= 1;
                            model2.Seq_No = sep;
                            model.Sup_Flow_ID = model3.Org_Flow_ID;
                            model2.Sup_Flow_ID = model.Org_Flow_ID;
                        }
                        else
                        {
                            var model2 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No - 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            var model3 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No + 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            int sep1 = model.Seq_No;
                            int sep2 = model2.Seq_No;
                            int sep3 = model3.Seq_No;
                            model.Seq_No = sep2;
                            model2.Seq_No = sep1;
                            int sup1 = model.Sup_Flow_ID;
                            int sup2 = model2.Sup_Flow_ID;
                            model.Sup_Flow_ID = sup2;
                            model2.Sup_Flow_ID = model.Org_Flow_ID;
                            model3.Sup_Flow_ID = model2.Org_Flow_ID;
                        }
                    }
                    dc.SubmitChanges();
                    new DAL.Org_Flow_Cycle_Info().DelAll((int)model.Prod_ID);
                    Ret.Msg = "上移成功！";
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "上移失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult SepDown(int id, int orgID, int prodID)
        {
            Ret.Msg = "下移成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == id);
                    int minSep = dc.Org_Flow.Where(m => m.Org_ID == model.Org_ID && m.Prod_ID == prodID).Max(m => m.Seq_No);
                    if (model.Seq_No == minSep)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "下移失败，此流程已经是底级流程！");
                    }
                    else
                    {
                        if (model.Seq_No == dc.Org_Flow.Where(m => m.Org_ID == model.Org_ID && m.Prod_ID == prodID).Min(m => m.Seq_No))
                        {
                            var model2 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No + 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            var model3 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No + 2 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            model.Seq_No += 1;
                            model2.Seq_No -= 1;
                            model2.Sup_Flow_ID = model.Sup_Flow_ID;
                            model3.Sup_Flow_ID = model.Org_Flow_ID;
                            model.Sup_Flow_ID = model2.Org_Flow_ID;
                        }
                        else if (model.Seq_No == dc.Org_Flow.Where(m => m.Org_ID == model.Org_ID && m.Prod_ID == prodID).Max(m => m.Seq_No) - 1)
                        {
                            var model2 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No + 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            var model3 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No - 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            model.Seq_No += 1;
                            model2.Seq_No -= 1;
                            model.Sup_Flow_ID = model2.Org_Flow_ID;
                            model2.Sup_Flow_ID = model3.Org_Flow_ID;
                        }
                        else
                        {
                            var model2 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No + 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            var model3 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No + 2 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            var model4 = dc.Org_Flow.Where(m => m.Seq_No == model.Seq_No - 1 && m.Org_ID == model.Org_ID && m.Prod_ID == prodID).First();
                            int sep1 = model.Seq_No;
                            int sep2 = model2.Seq_No;
                            int sep3 = model3.Seq_No;
                            model.Seq_No = sep2;
                            model2.Seq_No = sep1;
                            model.Sup_Flow_ID = model2.Org_Flow_ID;
                            model2.Sup_Flow_ID = model4.Org_Flow_ID;
                            model3.Sup_Flow_ID = model.Org_Flow_ID;
                        }
                    }
                    dc.SubmitChanges();
                    new DAL.Org_Flow_Cycle_Info().DelAll((int)model.Prod_ID);
                    Ret.Msg = "下移成功！";
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "下移失败，请重试！"); }
            return Ret;
        }
        public List<SelectListItem> Getallshengddlwiths(int sid, int oid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.View_Flow_User> Itemsdt = new List<LinqModel.View_Flow_User>();
            Itemsdt = new DAL.View_Flow_Users().GetAllwithoid(oid); ;
            if (Itemsdt != null)
            {

                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                foreach (LinqModel.View_Flow_User m in Itemsdt)
                {
                    list.Add(new SelectListItem { Selected = (m.Org_Flow_ID == sid), Text = m.Flow_Name, Value = m.Org_Flow_ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> Getalls(int sid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.View_Flow_User> Itemsdt = new List<LinqModel.View_Flow_User>();
            Itemsdt = new DAL.View_Flow_Users().GetAll(); ;
            if (Itemsdt != null)
            {

                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                foreach (LinqModel.View_Flow_User m in Itemsdt)
                {
                    list.Add(new SelectListItem { Selected = (m.Org_Flow_ID == sid), Text = m.Flow_Name, Value = m.Org_Flow_ID.ToString() });
                }
            }
            return list ?? null;
        }
        public string GetNextFlowIDs(int id)
        {
            string result = "|";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    List<LinqModel.Org_Flow> list = dc.Org_Flow.Where(m => m.Sup_Flow_ID == id).ToList();
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            result += list[i].Org_Flow_ID + "|";
                        }
                    }
                }
            }
            catch { result = string.Empty; }
            return result;
        }
    }
}
