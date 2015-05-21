using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Flow_Pics : DALBase
    {
        public IPagedList<LinqModel.Flow_Pics> GetList(int orgFlowID, string timeS, string timeE, int pInex, int pSize)
        {
            IPagedList<LinqModel.Flow_Pics> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Flow_Pics where m.Org_Flow_ID == orgFlowID select m;

                    if (!string.IsNullOrEmpty(timeS))
                    {
                        data = data.Where(m => m.Up_Time >= DateTime.Parse(timeS));
                    }
                    if (!string.IsNullOrEmpty(timeE))
                    {
                        data = data.Where(m => m.Up_Time < DateTime.Parse(timeE).AddDays(1));
                    }
                    list = data.OrderByDescending(m => m.Up_Time).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public Common.Argument.RetResult Add(LinqModel.Flow_Pics model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Flow_Pics.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public LinqModel.Flow_Pics GetModel(int id)
        {
            LinqModel.Flow_Pics model = new LinqModel.Flow_Pics();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Flow_Pics.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.Flow_Pics(); }
            return model;
        }
        public Common.Argument.RetResult Del(int id)
        {
            Ret.Msg = "删除成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Flow_Pics.FirstOrDefault(m => m.ID == id);
                    dc.Flow_Pics.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }

        public List<LinqModel.View_Flow_Pics> GetListOrgTopCount(int orgID, int count)
        {
            List<LinqModel.View_Flow_Pics> list = new List<LinqModel.View_Flow_Pics>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Flow_Pics where (from n in dc.Org_Flow where n.Org_ID == orgID select n.Org_Flow_ID).Contains(m.Org_Flow_ID) select m;
                    list = data.OrderByDescending(m => m.Up_Time).Take(count).ToList();
                }
            }
            catch { }
            return list;
        }

    }
}
