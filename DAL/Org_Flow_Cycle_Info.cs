using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_Flow_Cycle_Info : DALBase
    {
        public List<LinqModel.Org_Flow_Cycle_Info> GetByProdID(int prodid)
        {
            List<LinqModel.Org_Flow_Cycle_Info> list = new List<LinqModel.Org_Flow_Cycle_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Org_Flow_Cycle_Info.Where(m => m.Prod_ID == prodid).ToList();
                }
            }
            catch { }
            return list;
        }
        public Common.Argument.RetResult Add(LinqModel.Org_Flow_Cycle_Info model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var mS = dc.Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == model.CycleStart);
                    var mE = dc.Org_Flow.FirstOrDefault(m => m.Org_Flow_ID == model.CycleEnd);
                    if (dc.Org_Flow_Cycle_Info.Count(m => m.Prod_ID == model.Prod_ID && (m.CycleEnd == model.CycleStart || m.CycleEnd == model.CycleEnd || m.CycleStart == model.CycleStart || m.CycleStart == model.CycleEnd)) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，循环设置节点已为开始节点或结束节点！");
                    }
                    else if (mS.Seq_No > mE.Seq_No)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，开始节点不能在结束节点完成后执行！");
                    }
                    else
                    {
                        dc.Org_Flow_Cycle_Info.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Del(int id)
        {
            Ret.Msg = "删除成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Flow_Cycle_Info.FirstOrDefault(m => m.Org_Flow_Cycle_Info_ID == id);
                    dc.Org_Flow_Cycle_Info.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
        public void DelAll(int prodID)
        {
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Flow_Cycle_Info.Where(m => m.Prod_ID == prodID);
                    dc.Org_Flow_Cycle_Info.DeleteAllOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { }
        }
        public bool IsFlowEnd(int flowID)
        {
            bool result = true;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Org_Flow_Cycle_Info.Where(m => m.CycleEnd == flowID);
                    if (model != null && model.Count() > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch { result = false; }
            return result;
        }
        public LinqModel.Org_Flow_Cycle_Info GetModel(int prodid, int orgFlowID)
        {
            LinqModel.Org_Flow_Cycle_Info model = new LinqModel.Org_Flow_Cycle_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_Flow_Cycle_Info.FirstOrDefault(m => m.Prod_ID == prodid && m.CycleEnd == orgFlowID);
                }
            }
            catch { model = new LinqModel.Org_Flow_Cycle_Info(); }
            return model;
        }
        public LinqModel.Org_Flow_Cycle_Info GetModel(int id)
        {
            LinqModel.Org_Flow_Cycle_Info model = new LinqModel.Org_Flow_Cycle_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Org_Flow_Cycle_Info.FirstOrDefault(m => m.Org_Flow_Cycle_Info_ID == id);
                }
            }
            catch { model = new LinqModel.Org_Flow_Cycle_Info(); }
            return model;
        }
        public List<LinqModel.View_Org_Flow_Cycle_Info> GetListByCycleEnd(int prodid, int cycleEndid)
        {
            List<LinqModel.View_Org_Flow_Cycle_Info> list = new List<LinqModel.View_Org_Flow_Cycle_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Org_Flow_Cycle_Info.Where(m => m.Prod_ID == prodid && m.CycleEnd == cycleEndid).ToList();
                }
            }
            catch { }
            return list;
        }
    }
}
