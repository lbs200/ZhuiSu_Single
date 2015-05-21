using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Code_Usage : DALBase
    {
        public List<LinqModel.Code_Usage> GetAll()
        {
            List<LinqModel.Code_Usage> list = new List<LinqModel.Code_Usage>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Code_Usage.ToList();
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Code_Usage GetModel(int id)
        {
            LinqModel.Code_Usage model = new LinqModel.Code_Usage();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Code_Usage.FirstOrDefault(m => m.Usage_ID == id);
                }
            }
            catch { model = new LinqModel.Code_Usage(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Code_Usage model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Code_Usage.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

    }
}
