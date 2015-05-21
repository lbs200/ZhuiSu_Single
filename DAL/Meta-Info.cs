using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList.Mvc;

namespace DAL
{
    public class Meta_Info : DALBase
    {
        public List<LinqModel.Meta_Info> GetAll()
        {
            List<LinqModel.Meta_Info> list = new List<LinqModel.Meta_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Meta_Info.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.Meta_Info> GetAllWithOut(List<int> listID, string txtSearch)
        {
            List<LinqModel.Meta_Info> list = new List<LinqModel.Meta_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Meta_Info where !listID.Contains(m.Info_ID) select m;
                    if (!string.IsNullOrEmpty(txtSearch))
                    {
                        data = data.Where(m => m.Info_Name.Contains(txtSearch));
                    }
                    list = data.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.Meta_Info> GetAllByGroupWithOut(int groupID, List<int> listID, string txtSearch)
        {
            List<LinqModel.Meta_Info> list = new List<LinqModel.Meta_Info>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Meta_Info where !listID.Contains(m.Info_ID) select m;
                    if (groupID > 0)
                    {
                        data = data.Where(m => (from x in dc.Group_Detail where x.Group_ID == groupID select x.Info_ID).Contains(m.Info_ID));
                    }
                    if (!string.IsNullOrEmpty(txtSearch))
                    {
                        data = data.Where(m => m.Info_Name.Contains(txtSearch));
                    }
                    list = data.ToList();
                }
            }
            catch { }
            return list;
        }
        public IPagedList<LinqModel.Meta_Info> GetList(string mName, int pInex, int pSize)
        {
            IPagedList<LinqModel.Meta_Info> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Meta_Info select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Info_Name.Contains(mName));
                    }
                    list = data.ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.Meta_Info GetModel(int id)
        {
            LinqModel.Meta_Info model = new LinqModel.Meta_Info();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Meta_Info.FirstOrDefault(m => m.Info_ID == id);
                }
            }
            catch { model = new LinqModel.Meta_Info(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Meta_Info model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Meta_Info.Count(m => m.Info_Name == model.Info_Name) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，名称重复！");
                    }
                    else
                    {
                        dc.Meta_Info.InsertOnSubmit(model);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Meta_Info model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Meta_Info.Count(m => m.Info_Name == model.Info_Name && m.Info_ID != model.Info_ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，名称重复！");
                    }
                    else
                    {
                        var temp = dc.Meta_Info.FirstOrDefault(m => m.Info_ID == model.Info_ID);
                        temp.Abbr = model.Abbr;
                        temp.Data_Type = model.Data_Type;
                        temp.Info_Description = model.Info_Description;
                        temp.Info_Name = model.Info_Name;
                        temp.Mask = model.Mask;
                        temp.Public = model.Public;
                        temp.Required = model.Required;
                        temp.Scheme_ID = model.Scheme_ID;
                        temp.Search_Point = model.Search_Point;
                        dc.SubmitChanges();
                    }
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
                    var model = dc.Meta_Info.FirstOrDefault(m => m.Info_ID == id);
                    dc.Meta_Info.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
