using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqModel;
using System.Web.Mvc;
namespace DAL
{

    public class ContCatogary : DALBase
    {
        public List<LinqModel.ContCatogary> GetAll()
        {
            List<LinqModel.ContCatogary> list = new List<LinqModel.ContCatogary>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ContCatogary.Where(m=>m.Assessment==1).ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.ContCatogary> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int suporg)
        {
            PagedList<LinqModel.ContCatogary> temp = null;
            List<LinqModel.ContCatogary> list = new List<LinqModel.ContCatogary>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ContCatogary select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(suporg.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == suporg);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ContCatogary>)list.OrderByDescending(m => m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public List<LinqModel.ContCatogary> GetAllWithOID(int OID)
        {
            List<LinqModel.ContCatogary> list = new List<LinqModel.ContCatogary>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ContCatogary.Where(m => m.Org_ID == OID&m.Assessment==1).ToList();
                    list.OrderBy(m => m.Remark);
                }
            }
            catch { }
            return list;
        }
        public List<SelectListItem> GetDropdownlistItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.ContCatogary> Itemsdt = new List<LinqModel.ContCatogary>();

            Itemsdt = new DAL.ContCatogary().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "请选择", Value = "-1" });
                list.Add(new SelectListItem { Text = "无上级", Value = "0" });
                foreach (LinqModel.ContCatogary m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemWithOID(int oid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.ContCatogary> Itemsdt = new List<LinqModel.ContCatogary>();

            Itemsdt = new DAL.ContCatogary().GetAllWithOID(oid); 
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                list.Add(new SelectListItem { Text = "无上级", Value = "0" });
                foreach (LinqModel.ContCatogary m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemWithOIDNew()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.ContCatogary> Itemsdt = new List<LinqModel.ContCatogary>();

            Itemsdt = new DAL.ContCatogary().GetAll();
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "---请选择---", Value = "-1" });
                list.Add(new SelectListItem { Text = "无上级", Value = "0" });
                foreach (LinqModel.ContCatogary m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> GetDropdownlistItemwithid(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.ContCatogary> Itemsdt = new List<LinqModel.ContCatogary>();
            Itemsdt = new DAL.ContCatogary().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (id.ToString().Equals("0")) });
                foreach (LinqModel.ContCatogary m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Name, Value = m.ID.ToString(), Selected = (m.ID.ToString().Equals(id.ToString())) });
                }
            }
            return list ?? null;
        }
        public LinqModel.ContCatogary GetModel(int id)
        {
            LinqModel.ContCatogary list = new LinqModel.ContCatogary();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ContCatogary.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.ContCatogary> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.ContCatogary> temp = null;
            List<LinqModel.ContCatogary> list = new List<LinqModel.ContCatogary>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ContCatogary select b;
                    
                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ContCatogary>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.ContCatogary> GetPgedListWithORG(string Nmae, int pageIndex, int pageSize,string oid)
        {
            PagedList<LinqModel.ContCatogary> temp = null;
            List<LinqModel.ContCatogary> list = new List<LinqModel.ContCatogary>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ContCatogary select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(oid))
                    {
                        if (!oid.Contains("-1"))
                        {
                            data = data.Where(m => m.Org_ID.Equals(oid));
                        }

                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ContCatogary>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
    }
}
