using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PublicRegistCommon : DALBase
    {
        /// <summary>
        /// 按行业类型添加企业流程、流程明细、管理员录入追溯信息权限
        /// </summary>
        /// <param name="orgID">企业ID</param>
        /// <param name="userID">管理员用户ID</param>
        /// <param name="hangyeID">行业ID</param>
        /// <returns></returns>
        public bool AddFlowAndPermission(int orgID,int userID,int hangyeID)
        {
            bool result = false;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                }
            }
            catch { result = false; }
            return result;
        }
    }
}
