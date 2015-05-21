using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Group_Detail : DALBase
    {
        public List<LinqModel.View_Group_Detail> GetAll()
        {
            List<LinqModel.View_Group_Detail> list = new List<LinqModel.View_Group_Detail>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_Group_Detail.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.View_Group_Detail> GetList(string mName, int groupID, int pInex, int pSize)
        {
            PagedList<LinqModel.View_Group_Detail> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Group_Detail select m;
                    if(groupID>0)
                    {
                        data = data.Where(m => m.Group_ID == groupID);
                    }
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.Info_Name.Contains(mName));
                    }
                    list = (PagedList<LinqModel.View_Group_Detail>)data.ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

        public LinqModel.View_Group_Detail GetModel(int id)
        {
            LinqModel.View_Group_Detail model = new LinqModel.View_Group_Detail();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_Group_Detail.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.View_Group_Detail(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Group_Detail model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    if (dc.Group_Detail.Count(m => m.Group_ID == model.Group_ID && m.Info_ID == model.Info_ID) > 0)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，该组已存在此信息元！");
                    }
                    else
                    {
                        dc.Group_Detail.InsertOnSubmit(model);
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
                    var model = dc.Group_Detail.FirstOrDefault(m => m.ID == id);
                    dc.Group_Detail.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
