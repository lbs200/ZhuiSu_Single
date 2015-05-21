using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PagedList.Mvc;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LinqModel;
using System.Configuration;

namespace DAL
{
    public class View_User : DALBase
    {
       
       
        public List<LinqModel.View_User> GetAll()
        {
            List<LinqModel.View_User> list = new List<LinqModel.View_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_User.ToList();
                }
            }
            catch { }
            return list;
        }
       public PagedList<LinqModel.View_User> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.View_User> temp = null;
            List<LinqModel.View_User> list = new List<LinqModel.View_User>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.View_User select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.View_User>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
      
        public LinqModel.View_User GetModel(int? id)
        {
            LinqModel.View_User model = new LinqModel.View_User();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.View_User.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { model = new LinqModel.View_User(); }
            return model;
        }
        
        public bool Exist(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                if (dc.View_User.Where(m => m.Name == name) != null)
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
