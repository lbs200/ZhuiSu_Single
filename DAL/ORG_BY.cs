using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqModel;
namespace DAL
{

    public class ORG_BY : DALBase
    {
        public List<LinqModel.ORG_BY> GetAll()
        {
            List<LinqModel.ORG_BY> list = new List<LinqModel.ORG_BY>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_BY.ToList();
                }
            }
            catch { }
            return list;
        }
        public LinqModel.ORG_BY GetModel(int id)
        {
            LinqModel.ORG_BY list = new LinqModel.ORG_BY();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_BY.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public LinqModel.ORG_BY GetModelOID(int oid)
        {
            LinqModel.ORG_BY list = new LinqModel.ORG_BY();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_BY.FirstOrDefault(m => m.Org_ID == oid);
                }
            }
            catch { }
            return list;
        }
        public LinqModel.ORG_BY GetModelWithOID(int oid)
        {
            LinqModel.ORG_BY list = new LinqModel.ORG_BY();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.ORG_BY.FirstOrDefault(m => m.Org_ID == oid);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.ORG_BY> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int suporg)
        {
            PagedList<LinqModel.ORG_BY> temp = null;
            List<LinqModel.ORG_BY> list = new List<LinqModel.ORG_BY>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ORG_BY select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.BYContent.Contains(Nmae));
                    }

                    if (!string.IsNullOrEmpty(suporg.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == suporg);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ORG_BY>)list.OrderByDescending(m => m.Org_ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.ORG_BY> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.ORG_BY> temp = null;
            List<LinqModel.ORG_BY> list = new List<LinqModel.ORG_BY>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.ORG_BY select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.BYContent.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.ORG_BY>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
    }
}
