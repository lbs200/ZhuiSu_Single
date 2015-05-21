using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DAL
{
    public class Permission : DALBase
    {
        public List<LinqModel.Permission> GetAll()
        {
            List<LinqModel.Permission> list = new List<LinqModel.Permission>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Permission.ToList();
                }
            }
            catch { }
            return list;
        }

        public List<LinqModel.Permission> GetAllCategory(string cid)
        {
            List<LinqModel.Permission> list = new List<LinqModel.Permission>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Permission.Where(model => model.CategoryID.ToString().Contains(cid)).ToList();
                }
            }
            catch { }
            return list;
        }


        public List<SelectListItem> GetDropdownlistItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Category> Itemsdt = new List<LinqModel.Category>();
            Itemsdt = new DAL.Category().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                foreach (LinqModel.Category m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.CategoryName, Value = m.CategoryID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemWithid(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Category> Itemsdt = new List<LinqModel.Category>();
            Itemsdt = new DAL.Category().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                foreach (LinqModel.Category m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.CategoryName, Value = m.CategoryID.ToString() , Selected = (m.CategoryID.ToString().Equals(id.ToString())) });
                }
            }
            return list ?? null;
        }
        
        public List<SelectListItem> GetAllPP()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Permission> Itemsdt = new List<LinqModel.Permission>();
            Itemsdt = new DAL.Permission().GetAll(); ;
            if (Itemsdt != null)
            {
                foreach (LinqModel.Permission m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.PermissionName, Value = m.PermissionID.ToString() });
                }
            }
            return list ?? null;
        }

        public List<SelectListItem> GetAllPPwithselected(int selectId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Permission> Itemsdt = new List<LinqModel.Permission>();
            Itemsdt = new DAL.Permission().GetAll(); 
            if (Itemsdt != null)
            {
                //list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (selectId.ToString().Equals("0")) });
                foreach (LinqModel.Permission m in Itemsdt)
                {
                   
                    list.Add(new SelectListItem { Selected = (m.PermissionID.ToString().Equals(selectId.ToString())), Text = m.PermissionName, Value = m.PermissionID.ToString() });
                    
                }
               
            }
            return list ?? null;
        }
        public PagedList<LinqModel.Permission> GetList(string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.Permission> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Permission select m;
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.PermissionName.Contains(mName));
                    }
                    list = (PagedList<LinqModel.Permission>)data.OrderBy(m => m.PermissionID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.Permission> GetListBD(string cat, string mName, int pInex, int pSize)
        {
            PagedList<LinqModel.Permission> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.Permission select m;
                    if (!string.IsNullOrEmpty(cat))
                    {
                        data = data.Where(m => m.CategoryID.ToString().Equals(cat));
                    }
                    if (!string.IsNullOrEmpty(mName))
                    {
                        data = data.Where(m => m.PermissionName.Contains(mName));
                    }
                    list = (PagedList<LinqModel.Permission>)data.OrderByDescending(m => m.PermissionID).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }
        public LinqModel.Permission GetModel(int id)
        {
            LinqModel.Permission model = new LinqModel.Permission();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Permission.FirstOrDefault(m => m.PermissionID == id);
                }
            }
            catch { model = new LinqModel.Permission(); }
            return model;
        }
        public Common.Argument.RetResult Add(LinqModel.Permission model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.Permission.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }

        public Common.Argument.RetResult Update(LinqModel.Permission model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Permission.FirstOrDefault(m => m.PermissionID == model.PermissionID);
                    temp.PermissionName = model.PermissionName;
                    temp.CategoryID = model.CategoryID;
                    temp.Controller = model.Controller;
                    temp.Action = model.Action;
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
                    var model = dc.Permission.FirstOrDefault(m => m.PermissionID == id);
                    dc.Permission.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
    }
}
