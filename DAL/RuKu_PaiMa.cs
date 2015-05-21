using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RuKu_PaiMa : DALBase
    {
        public IPagedList<LinqModel.RuKu_PaiMa> GetList(int orgID,string orgName, string timeS, string timeE, int pInex, int pSize)
        {
            IPagedList<LinqModel.RuKu_PaiMa> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.RuKu_PaiMa where m.Org_ID == orgID select m;
                    if (!string.IsNullOrEmpty(orgName))
                    {
                        data = data.Where(m => m.From_Org_Name.Contains(orgName));
                    }

                    if (!string.IsNullOrEmpty(timeS))
                    {
                        data = data.Where(m => m.TimeAdd >= DateTime.Parse(timeS));
                    }
                    if (!string.IsNullOrEmpty(timeE))
                    {
                        data = data.Where(m => m.TimeAdd < DateTime.Parse(timeE).AddDays(1));
                    }
                    list = data.OrderByDescending(m => m.ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public Common.Argument.RetResult Add(LinqModel.RuKu_PaiMa model)
        {
            Ret.Msg = "操作成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.RuKu_PaiMa.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败！"); }
            return Ret;
        }

        public LinqModel.RuKu_PaiMa GetModel(int id)
        {
            LinqModel.RuKu_PaiMa model = new LinqModel.RuKu_PaiMa();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.RuKu_PaiMa.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.RuKu_PaiMa(); }
            return model;
        }
    }
}
