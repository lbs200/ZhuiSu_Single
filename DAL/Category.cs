using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqModel;
namespace DAL
{

    public class Category : DALBase
    {
        public List<LinqModel.Category> GetAll()
        {
            List<LinqModel.Category> list = new List<LinqModel.Category>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Category.ToList();
                }
            }
            catch { }
            return list;
        }
        public Common.Argument.RetResult Edit(LinqModel.Category user)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    //dc.Entry(user).State = EntityState.Modified;
                    var model = dc.Category.FirstOrDefault(m => m.CategoryID == user.CategoryID);
                    model.CategoryName = user.CategoryName;
                  
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }

        public bool Exist(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                int c = dc.Category.Where(m => m.CategoryName == name).Count();

                if (c > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public LinqModel.Category GetModel(int id)
        {
            LinqModel.Category list = new LinqModel.Category();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Category.FirstOrDefault(m => m.CategoryID == id);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.Category> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.Category> temp = null;
            List<LinqModel.Category> list = new List<LinqModel.Category>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Category select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.CategoryName.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Category>)list.OrderByDescending(m=>m.CategoryID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
    }
}
