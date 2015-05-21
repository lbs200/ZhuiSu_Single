using LinqModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class S_Tree : DALBase
    {
        public List<System.Web.Mvc.SelectListItem> GetDropdownlistItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.View_S_Tree> Itemsdt = new List<LinqModel.View_S_Tree>();
            Itemsdt = new DAL.S_Tree().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = false });
                foreach (LinqModel.View_S_Tree m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Text, Value = m.NodeID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> Getallshengddlwiths(int sid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.View_S_Tree> Itemsdt = new List<LinqModel.View_S_Tree>();
            Itemsdt = new DAL.S_Tree().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (0 == sid) });
                foreach (LinqModel.View_S_Tree m in Itemsdt)
                {
                    list.Add(new SelectListItem { Selected = (m.NodeID == sid), Text = m.Text, Value = m.NodeID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<LinqModel.View_S_Tree> GetAll()
        {
            List<LinqModel.View_S_Tree> list = new List<LinqModel.View_S_Tree>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_S_Tree.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.S_Tree> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.S_Tree> temp = null;
            List<LinqModel.S_Tree> list = new List<LinqModel.S_Tree>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.S_Tree select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Text.Contains(Nmae));
                    }

                    list = data.ToList();
                    temp = (PagedList<LinqModel.S_Tree>)list.OrderByDescending(m=>m.NodeID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public LinqModel.S_Tree GetModel(int? id)
        {
            LinqModel.S_Tree model = new LinqModel.S_Tree();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.S_Tree.FirstOrDefault(m => m.NodeID == id);
                }
            }
            catch { model = new LinqModel.S_Tree(); }
            return model;
        }
        public Common.Argument.RetResult Create(LinqModel.S_Tree model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.S_Tree.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        public Common.Argument.RetResult Edit([Bind(Include = "NodeID,Text,ParentID,ParentPath,Url,comment,PermissionID,ImageUrl")] LinqModel.S_Tree S_Tree)
        {

            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //dc.Entry(S_Tree).State = EntityState.Modified;
                    var model = dc.S_Tree.FirstOrDefault(m => m.NodeID == S_Tree.NodeID);
                    model.comment = S_Tree.comment;
                    model.ImageUrl = S_Tree.ImageUrl;
                    model.ParentID = S_Tree.ParentID;
                    model.ParentPath = S_Tree.ParentPath;
                    model.PermissionID = S_Tree.PermissionID;
                    model.Text = S_Tree.Text;
                    model.Url = S_Tree.Url;
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
                    var model = dc.S_Tree.FirstOrDefault(m => m.NodeID == id);
                    dc.S_Tree.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
        public bool Exist(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                if (dc.S_Tree.Where(m => m.Text == name) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
