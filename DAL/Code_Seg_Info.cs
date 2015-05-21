using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Code_Seg_Info : DALBase
    {
        public List<LinqModel.Code_Seg_Info> GetAll()
        {
            List<LinqModel.Code_Seg_Info> list = new List<LinqModel.Code_Seg_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Code_Seg_Info.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.Code_Seg_Info> GetList(int Scheme_ID)
        {
            List<LinqModel.Code_Seg_Info> list = new List<LinqModel.Code_Seg_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Code_Seg_Info.Where(m => m.Scheme_ID == Scheme_ID).OrderBy(m => m.Seg_No).ToList();
                }
            }
            catch { }
            return list;
        }
        public LinqModel.Code_Seg_Info GetModel(int id)
        {
            LinqModel.Code_Seg_Info model = new LinqModel.Code_Seg_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Code_Seg_Info.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.Code_Seg_Info(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Code_Seg_Info model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Code_Seg_Info.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Update(LinqModel.Code_Seg_Info model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Code_Seg_Info.FirstOrDefault(m => m.ID == model.ID);
                    temp.Meaning = model.Meaning;
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
                    var model = dc.Code_Seg_Info.FirstOrDefault(m => m.ID == id);
                    dc.Code_Seg_Info.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}