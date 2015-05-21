using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class Pro_Category : DALBase
    {
        public LinqModel.Prod_category GetModel(int? id)
        {
            LinqModel.Prod_category model = new LinqModel.Prod_category();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Prod_category.FirstOrDefault(m => m.ID == id);
                    //model = dc.Prod_category.Find(Convert.ToInt32(string.IsNullOrEmpty(id.ToString()) ? "0" : id.ToString()));
                }
            }
            catch { model = new LinqModel.Prod_category(); }
            return model;
        }
        public List<SelectListItem> Getallshengddlwiths(int sid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Prod_category> Itemsdt = new List<LinqModel.Prod_category>();
            Itemsdt = new DAL.Pro_Category().GetAll(); ;
            if (Itemsdt != null)
            {
                foreach (LinqModel.Prod_category m in Itemsdt)
                {
                    list.Add(new SelectListItem { Selected = (m.ID.Equals(sid.ToString())), Text = m.Description, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<LinqModel.Prod_category> GetAll()
        {
            List<LinqModel.Prod_category> list = new List<LinqModel.Prod_category>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Prod_category.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<SelectListItem> GetDropdownlistItemwithid(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Prod_category> Itemsdt = new List<LinqModel.Prod_category>();
            Itemsdt = new DAL.Pro_Category().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (id.ToString().Equals("0")) });
                foreach (LinqModel.Prod_category m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Description, Value = m.ID.ToString(), Selected = (m.ID.ToString().Equals(id.ToString())) });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Prod_category> Itemsdt = new List<LinqModel.Prod_category>();
            Itemsdt = new DAL.Pro_Category().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "请选择", Value = "*", Selected = false });
                foreach (LinqModel.Prod_category m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Description, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetChildListItem(int parent)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.Prod_category> Itemsdt = new List<LinqModel.Prod_category>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    Itemsdt = dc.Prod_category.Where(m => m.Sup_Category == parent).ToList();
                    if (Itemsdt != null)
                    {
                        list.Add(new SelectListItem { Text = "顶级", Value = "0", Selected = false });
                        foreach (LinqModel.Prod_category m in Itemsdt)
                        {
                            list.Add(new SelectListItem { Text = m.Description, Value = m.ID.ToString() });
                        }
                    }
                }
            }
            catch { }
            return list ?? null;
        }
        public Common.Argument.RetResult Update(LinqModel.Prod_category model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Prod_category.FirstOrDefault(m => m.ID == model.ID);
                    temp.Description = model.Description;

                    temp.Sup_Category = model.Sup_Category;




                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public PagedList<LinqModel.Prod_category> GetPgedList(int sub, string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.Prod_category> temp = null;
            List<LinqModel.Prod_category> list = new List<LinqModel.Prod_category>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Prod_category select b;
                    if (sub > 0)
                    {
                        data = data.Where(m => m.Sup_Category == sub);
                    }
                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Description.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Prod_category>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public List<LinqModel.Prod_category> GetListSub()
        {
            List<LinqModel.Prod_category> list = new List<LinqModel.Prod_category>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Prod_category where b.Sup_Category == 0 select b;
                    list = data.ToList();
                }
            }
            catch { }
            return list;
        }

        public List<LinqModel.Prod_category> GetListChildren(int subID)
        {
            List<LinqModel.Prod_category> list = new List<LinqModel.Prod_category>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Prod_category where b.Sup_Category == subID select b;
                    list = data.ToList();
                }
            }
            catch { }
            return list;
        }
    }
}
