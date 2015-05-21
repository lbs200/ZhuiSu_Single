using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Argument
{
    /// <summary>
    /// 登录成功初始化信息数据对象
    /// </summary>
    [Serializable]
    public class LoginInfo
    {
        /// <summary>
        /// 当前登陆用户信息
        /// </summary>
        public LinqModel.User user { get; set; }
        /// <summary>
        /// 当前登陆用户功能权限列表
        /// </summary>
        public List<LinqModel.View_UserPermission> listPermissionUser { get; set; }
       
        /// <summary>
        /// 菜单列表（当前登陆用户）
        /// </summary>
        public List<LinqModel.View_S_Tree> listSTreeUser { get; set; }

        public List<LinqModel.Flow_User> listFlowUser { get; set; }
    }
}
