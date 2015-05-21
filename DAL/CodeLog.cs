using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public class CodeLog : DALBase
    {
        public List<LinqModel.CodeLog> GetAll()
        {
            List<LinqModel.CodeLog> list = new List<LinqModel.CodeLog>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.CodeLog.ToList();
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.CodeLog> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.CodeLog> temp = null;
            List<LinqModel.CodeLog> list = new List<LinqModel.CodeLog>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.CodeLog select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.CodeLog>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.CodeLog> GetPgedListWithORG(string Nmae, int pageIndex, int pageSize, string oid)
        {
            PagedList<LinqModel.CodeLog> temp = null;
            List<LinqModel.CodeLog> list = new List<LinqModel.CodeLog>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.CodeLog select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(oid))
                    {
                        if (!oid.Contains("-1"))
                        {
                            data = data.Where(m => m.Org_ID.Equals(oid));
                        }

                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.CodeLog>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.CodeLog> GetPgedListWithOID(string Nmae, int pageIndex, int pageSize, int oid)
        {
            PagedList<LinqModel.CodeLog> temp = null;
            List<LinqModel.CodeLog> list = new List<LinqModel.CodeLog>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.CodeLog select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));

                    }
                    if (!string.IsNullOrEmpty(oid.ToString()))
                    {

                        data = data.Where(m => m.Org_ID == oid);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.CodeLog>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.CodeLog> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int orgid)
        {
            PagedList<LinqModel.CodeLog> temp = null;
            List<LinqModel.CodeLog> list = new List<LinqModel.CodeLog>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.CodeLog select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == orgid);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.CodeLog>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public Common.Argument.RetResult Create(LinqModel.CodeLog model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.CodeLog.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        } 
    }
}

