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

    public class ORG_Photo : DALBase
    {
        public List<LinqModel.ORG_Photo> GetAll()
        {
            List<LinqModel.ORG_Photo> list = new List<LinqModel.ORG_Photo>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_Photo.ToList();
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.ORG_Photo> GetAllWithOID(int OID)
        {
            List<LinqModel.ORG_Photo> list = new List<LinqModel.ORG_Photo>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_Photo.Where(m=>m.Org_ID==OID).ToList();
                    list.OrderBy(m=>m.Sque);
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.ORG_Photo> GetAllWithOIDTU(int OID)
        {
            List<LinqModel.ORG_Photo> list = new List<LinqModel.ORG_Photo>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_Photo.Where(m => m.Org_ID == OID&&m.Sque!=1).ToList();
                    list.OrderBy(m => m.Sque);
                }
            }
            catch { }
            return list;
        }
        //public List<SelectListItem> GetDropdownlistItem()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    List<LinqModel.ContCatogary> Itemsdt = new List<LinqModel.ORG_Photo>();

        //    Itemsdt = new DAL.ORG_Photo().GetAll(); ;
        //    if (Itemsdt != null)
        //    {
        //        list.Add(new SelectListItem { Text = "请选择", Value = "-1" });
        //        list.Add(new SelectListItem { Text = "无上级", Value = "0" });
        //        foreach (LinqModel.ORG_Photo m in Itemsdt)
        //        {
        //            list.Add(new SelectListItem { Text = m.Title, Value = m.ID.ToString() });
        //        }
        //    }
        //    return list ?? null;
        //}
        //public List<SelectListItem> GetDropdownlistItemwithid(int id)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    List<LinqModel.ORG_Photo> Itemsdt = new List<LinqModel.ORG_Photo>();
        //    Itemsdt = new DAL.ORG_Photo().GetAll(); ;
        //    if (Itemsdt != null)
        //    {
        //        list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (id.ToString().Equals("0")) });
        //        foreach (LinqModel.ORG_Photo m in Itemsdt)
        //        {
        //            list.Add(new SelectListItem { Text = m.Title, Value = m.ID.ToString(), Selected = (m.ID.ToString().Equals(id.ToString())) });
        //        }
        //    }
        //    return list ?? null;
        //}
        public LinqModel.ORG_Photo GetModel(int id)
        {
            LinqModel.ORG_Photo list = new LinqModel.ORG_Photo();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_Photo.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.ORG_Photo> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int suporg)
        {
            PagedList<LinqModel.ORG_Photo> temp = null;
            List<LinqModel.ORG_Photo> list = new List<LinqModel.ORG_Photo>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ORG_Photo select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Remark.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(suporg.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == suporg);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ORG_Photo>)list.OrderByDescending(m => m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.ORG_Photo> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.ORG_Photo> temp = null;
            List<LinqModel.ORG_Photo> list = new List<LinqModel.ORG_Photo>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ORG_Photo select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Remark.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ORG_Photo>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
    }
}
