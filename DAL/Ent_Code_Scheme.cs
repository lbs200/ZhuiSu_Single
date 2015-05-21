using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Ent_Code_Scheme : DALBase
    {
        public List<LinqModel.Ent_Code_Scheme> GetAll()
        {
            List<LinqModel.Ent_Code_Scheme> list = new List<LinqModel.Ent_Code_Scheme>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Ent_Code_Scheme.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.Ent_Code_Scheme> GetList(int org_ID, int pInex, int pSize)
        {
            PagedList<LinqModel.Ent_Code_Scheme> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Ent_Code_Scheme select m;
                    if (org_ID > 0)
                    {
                        data = data.Where(m => m.Org_ID == org_ID);
                    }
                    list = (PagedList<LinqModel.Ent_Code_Scheme>)data.OrderBy(m => m.Scheme_ID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Ent_Code_Scheme GetModel(int id)
        {
            LinqModel.Ent_Code_Scheme model = new LinqModel.Ent_Code_Scheme();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Ent_Code_Scheme.FirstOrDefault(m => m.Scheme_ID == id);
                }
            }
            catch { model = new LinqModel.Ent_Code_Scheme(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Ent_Code_Scheme model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Ent_Code_Scheme.InsertOnSubmit(model);
                    dc.SubmitChanges();
                    #region 添加编码池数据
                    LinqModel.Ent_Code_Pool modelPool = new LinqModel.Ent_Code_Pool();
                    modelPool.Fix_Code_Seg = model.Code_Before;
                    modelPool.Max_SN = 0;
                    modelPool.Org_ID = model.Org_ID;
                    modelPool.Pool_Description = model.Code_Before;
                    modelPool.Scheme_ID = model.Scheme_ID;
                    dc.Ent_Code_Pool.InsertOnSubmit(modelPool);
                    #endregion
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Ent_Code_Scheme model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Ent_Code_Scheme.FirstOrDefault(m => m.Scheme_ID == model.Scheme_ID);
                    if (temp.Code_Before != model.Code_Before)
                    {
                        var tempPool = dc.Ent_Code_Pool.FirstOrDefault(m => m.Scheme_ID == temp.Scheme_ID);
                        tempPool.Fix_Code_Seg = temp.Code_Before;
                        tempPool.Pool_Description = temp.Code_Before;
                    }
                    temp.Org_ID = model.Org_ID;
                    temp.P_S_separator = model.P_S_separator;
                    temp.Separator = model.Separator;
                    temp.SN_Position = model.SN_Position;
                    temp.Total_Seg = model.Total_Seg;
                    temp.Code_Before = model.Code_Before;
                    temp.Date_Required = model.Date_Required;

                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Del(int id)
        {
            Ret.Msg = "删除成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.Ent_Code_Scheme.FirstOrDefault(m => m.Scheme_ID == id);
                    dc.Ent_Code_Scheme.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
