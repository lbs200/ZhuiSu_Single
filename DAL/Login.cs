using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Login : DALBase
    {
        /// <summary>
        /// 登陆方法
        /// </summary>
        /// <param name="uname">用户名</param>
        /// <param name="pass">密码</param>
        /// <returns>登陆结果：0=用户名密码错误，1=登陆成功，2=未知错误</returns>
        public static LinqModel.User LoginMethod(string uname, string pass, out int result)
        {
            LinqModel.User model = null;
            result = 2;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.User.FirstOrDefault(m => m.UserName == uname && m.Password == pass);
                    if (model != null && model.ID > 0)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            catch
            {
                result = 2;
            }
            return model;
        }
    }
}
