using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GB : DALBase
    {
        public List<LinqModel.GBT475494> GetAll()
        {
            List<LinqModel.GBT475494> list = new List<LinqModel.GBT475494>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.GBT475494.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.GBT475494> GetAllkg()
        {
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var pp = (List<LinqModel.GBT475494>)(from m in dc.GBT475494
                                                         select new
                                                         {
                                                             ID = m.ID,
                                                             C1 = m.C1.ToString().Trim(),
                                                             C2 = m.C2.ToString().Trim(),
                                                             C3 = m.C3.ToString().Trim(),
                                                             C4 = m.C4.ToString().Trim(),
                                                             Name = m.Name.ToString().Trim(),
                                                             Remark = m.Remark.ToString().Trim(),

                                                         });
                    return pp.ToList();
                }
            }
            catch
            { return null; }



        }
        public LinqModel.GBT475494 GetModel(int id)
        {
            LinqModel.GBT475494 list = new LinqModel.GBT475494();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.GBT475494.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public Common.Argument.RetResult Update(LinqModel.GBT475494 model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.GBT475494.FirstOrDefault(m => m.ID == model.ID);
                    temp.C1 = model.C1;
                    temp.C2 = model.C2;
                    temp.C3 = model.C3;
                    temp.C4 = model.C4;
                    temp.Name = model.Name;
                    temp.Remark = model.Remark;


                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public PagedList<LinqModel.GBT475494> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.GBT475494> temp = null;
            List<LinqModel.GBT475494> list = new List<LinqModel.GBT475494>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.GBT475494 select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.GBT475494>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
    }
}
