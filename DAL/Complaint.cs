using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Complaint : DALBase
    {
        public IPagedList<LinqModel.View_Complaint> GetList(List<int> prodIDs, string timeS, string timeE, string ewm, int pInex, int pSize)
        {
            IPagedList<LinqModel.View_Complaint> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Complaint select m;
                    if (!string.IsNullOrEmpty(ewm))
                    {
                        data = data.Where(m => m.Prod_Code == ewm);
                    }
                    else
                    {
                        if (prodIDs.Count > 0)
                        {
                            data = data.Where(m => prodIDs.Contains((int)m.Prod_ID));
                        }
                        if (!string.IsNullOrEmpty(timeS))
                        {
                            data = data.Where(m => m.Complaint_Date >= DateTime.Parse(timeS));
                        }
                        if (!string.IsNullOrEmpty(timeE))
                        {
                            data = data.Where(m => m.Complaint_Date < DateTime.Parse(timeE).AddDays(1));
                        }
                    }
                    list = data.OrderByDescending(m => m.Complaint_Date).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public Common.Argument.RetResult Add(LinqModel.Complaint model)
        {
            Ret.Msg = "投诉成功，请等待处理结果！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Complaint.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "投诉失败，请重试！"); }
            return Ret;
        }

        public LinqModel.View_Complaint GetModel(int id)
        {
            LinqModel.View_Complaint model = new LinqModel.View_Complaint();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Complaint.FirstOrDefault(m => m.Complaint_ID == id);
                }
            }
            catch { model = new LinqModel.View_Complaint(); }
            return model;
        }
    }
}
