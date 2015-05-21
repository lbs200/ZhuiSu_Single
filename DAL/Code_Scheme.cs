using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Code_Scheme : DALBase
    {
        public List<LinqModel.Code_Scheme> GetAll()
        {
            List<LinqModel.Code_Scheme> list = new List<LinqModel.Code_Scheme>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Code_Scheme.ToList();
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Code_Scheme GetModel(int id)
        {
            LinqModel.Code_Scheme model = new LinqModel.Code_Scheme();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Code_Scheme.FirstOrDefault(m => m.Scheme_ID == id);
                }
            }
            catch { model = new LinqModel.Code_Scheme(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Code_Scheme model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Code_Scheme.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

    }
}
