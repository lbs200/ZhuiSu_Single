using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserPermission : DALBase
    {
        public List<LinqModel.View_UserPermission> GetList(int roleID)
        {
            List<LinqModel.View_UserPermission> list = new List<LinqModel.View_UserPermission>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.View_UserPermission.Where(m => m.RoleId == roleID).ToList();
                }
            }
            catch { }
            return list;
        }
      
        public List<LinqModel.UserPermission> GetListR(int roleID)
        {
            List<LinqModel.UserPermission> list = new List<LinqModel.UserPermission>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.UserPermission.Where(m => m.RoleId == roleID).ToList();
                }
            }
            catch { }
            return list;
        }
    }
}
