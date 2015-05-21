using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqModel;
using System.Web;
using System.Web.Mvc;
namespace DAL
{

    public class Flow_User : DALBase
    {
        public List<LinqModel.Products> GetProduct(int oid)
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {


                List<LinqModel.Products> pp = new DAL.Products().GetAllwithORGID(oid)
                  .GroupBy(p => p.Name)
                  .Select(g => g.First())
                  .ToList();
                return pp;


            }
        }
        public List<LinqModel.Products> GetAllProduct()
        {
            using (var dc = DAL.DALBase.GetDataContext())
            {


                List<LinqModel.Products> pp = new DAL.Products().GetAll()
                  .GroupBy(p => p.Name)
                  .Select(g => g.First())
                  .ToList();
                return pp;


            }
        }
        public List<LinqModel.Flow_User> GetAll()
        {
            List<LinqModel.Flow_User> list = new List<LinqModel.Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Flow_User.ToList();
                }
            }
            catch { }
            return list;
        }
        public LinqModel.Flow_User GetModel(int id)
        {
            LinqModel.Flow_User list = new LinqModel.Flow_User();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Flow_User.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public Common.Argument.RetResult Update(LinqModel.Flow_User model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Flow_User.FirstOrDefault(m => m.ID == model.ID);
                    temp.Use_ID = model.Use_ID;
                    temp.Org_Flow_ID = model.Org_Flow_ID;

                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public PagedList<LinqModel.View_Flow_User> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.View_Flow_User> temp = null;
            List<LinqModel.View_Flow_User> list = new List<LinqModel.View_Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.View_Flow_User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.ToString().Contains(Nmae));
                    }
                    //(int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]])


                    list = data.ToList();
                    temp = (PagedList<LinqModel.View_Flow_User>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.View_Flow_User> GetPgedListNew(string Nmae, int pageIndex, int pageSize, int orgid)
        {
            PagedList<LinqModel.View_Flow_User> temp = null;
            List<LinqModel.View_Flow_User> list = new List<LinqModel.View_Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.View_Flow_User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.UserName.ToString().Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID == orgid);
                    }
                    //(int)((Common.Argument.LoginInfo)Session[System.Configuration.ConfigurationManager.AppSettings["LoginSession"]])


                    list = data.ToList();
                    temp = (PagedList<LinqModel.View_Flow_User>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public List<LinqModel.Flow_User> GetAllByUser(int userID)
        {
            List<LinqModel.Flow_User> list = new List<LinqModel.Flow_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Flow_User.Where(m => m.Use_ID == userID).ToList();
                }
            }
            catch { }
            return list;
        }

    }
}
