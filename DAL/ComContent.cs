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

    public class ComContent : DALBase
    {
        public List<LinqModel.ComContent> GetAll()
        {
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ComContent.Where(m=>m.Assessment==1).ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.ComContent> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int suporg)
        {
            PagedList<LinqModel.ComContent> temp = null;
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ComContent select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Title.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(suporg.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == suporg);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ComContent>)list.OrderByDescending(m => m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public List<LinqModel.ComContent> GetAllWithOID(int OID)
        {
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ComContent.Where(m => m.Org_ID == OID&m.Assessment==1).ToList();
                    list.OrderByDescending(m => m.ID);
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.ComContent> GetAllWithOIDTop(int OID, int count)
        {
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ComContent.Where(m => m.Org_ID == OID & m.Assessment == 1).OrderByDescending(m => m.ID).Take(count).ToList();
                    
                }
            }
            catch { }
            return list;
        }
        public LinqModel.View_ComContent GetGYWithOID(int OID)
        {
            LinqModel.View_ComContent list = new LinqModel.View_ComContent();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {

                    list = dc.View_ComContent.FirstOrDefault(m => m.Org_ID == OID & m.Name.Equals("关于我们"));


                }
            }
            catch { }
            return list;
        }
        public List<SelectListItem> GetDropdownlistItem()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.ComContent> Itemsdt = new List<LinqModel.ComContent>();

            Itemsdt = new DAL.ComContent().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "请选择", Value = "-1" });
                list.Add(new SelectListItem { Text = "无上级", Value = "0" });
                foreach (LinqModel.ComContent m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Title, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public PagedList<LinqModel.ComContent> GetPgedListWithOID(string Nmae, int pageIndex, int pageSize, int oid)
        {
            PagedList<LinqModel.ComContent> temp = null;
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ComContent select b;
                    data = data.Where(m => m.Assessment == 1);
                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Contents.Contains(Nmae) & m.Assessment == 1);

                    }
                    if (!string.IsNullOrEmpty(oid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == oid & m.Assessment == 1);

                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ComContent>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.ComContent> GetPgedListWithOIDCCID(string Nmae, int pageIndex, int pageSize, int oid, int ccid)
        {
            PagedList<LinqModel.ComContent> temp = null;
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ComContent select b;
                    data = data.Where(m => m.Assessment == 1);
                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Contents.Contains(Nmae)&m.Assessment==1);

                    }
                    if (!string.IsNullOrEmpty(oid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == oid & m.Assessment == 1);

                    }
                    if (!string.IsNullOrEmpty(ccid.ToString()))
                    {
                        data = data.Where(m => m.CategoryID == ccid&m.Assessment==1);

                    }
                   
                    


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ComContent>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public List<SelectListItem> GetDropdownlistItemwithid(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.ComContent> Itemsdt = new List<LinqModel.ComContent>();
            Itemsdt = new DAL.ComContent().GetAll(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem { Text = "无上级", Value = "0", Selected = (id.ToString().Equals("0")) });
                foreach (LinqModel.ComContent m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Title, Value = m.ID.ToString(), Selected = (m.ID.ToString().Equals(id.ToString())) });
                }
            }
            return list ?? null;
        }
        public LinqModel.ComContent GetModel(int id)
        {
            LinqModel.ComContent list = new LinqModel.ComContent();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ComContent.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.ComContent> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.ComContent> temp = null;
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ComContent select b;
                    //data = data.Where(m=>m.Assessment==1);
                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Title.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ComContent>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.ComContent> GetPgedListWithORG(string Nmae, int pageIndex, int pageSize, string oid)
        {
            PagedList<LinqModel.ComContent> temp = null;
            List<LinqModel.ComContent> list = new List<LinqModel.ComContent>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ComContent select b;
                    //data = data.Where(m=>m.Assessment==1);
                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Title.Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(oid))
                    {
                        if (!oid.Contains("-1"))
                        {
                            data = data.Where(m => m.Org_ID.Equals(oid));
                        }

                    }

                    list = data.ToList();
                    temp = (PagedList<LinqModel.ComContent>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
    }
}
