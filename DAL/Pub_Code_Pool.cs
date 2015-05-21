using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Pub_Code_Pool : DALBase
    {
        public PagedList<LinqModel.View_Pub_Code_Pool> GetList(string eName, int pInex, int pSize)
        {
            PagedList<LinqModel.View_Pub_Code_Pool> list = null;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from m in dc.View_Pub_Code_Pool select m;
                    if (!string.IsNullOrEmpty(eName))
                    {
                        data = data.Where(m => m.Name.Contains(eName));
                    }
                    list = (PagedList<LinqModel.View_Pub_Code_Pool>)data.OrderByDescending(m => m.Max_SN).ToPagedList(pInex, pSize);
                }
            }
            catch { }
            return list;
        }

    }
}
