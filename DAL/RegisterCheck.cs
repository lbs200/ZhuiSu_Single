using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RegisterCheck : DALBase
    {
        
        
        
        public List<LinqModel.RegisterCheck> GetAll()
        {
            
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    return dc.RegisterCheck.ToList();
                }
            }
            catch { }
            return null;
        }
        public LinqModel.RegisterCheck GetModel(int? id)
        {
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    return dc.RegisterCheck.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return null;
        }
        public PagedList<LinqModel.RegisterCheck> GetPgedList(string txtOrgName,int pageIndex, int pageSize)
        {
            PagedList<LinqModel.RegisterCheck> temp = null;
            List<LinqModel.RegisterCheck> list = new List<LinqModel.RegisterCheck>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.RegisterCheck select b;

                    if (!string.IsNullOrEmpty(txtOrgName))
                    {
                        data = data.Where(m => m.UserName.Contains(txtOrgName));
                    }

                    list = data.ToList();
                    temp = (PagedList<LinqModel.RegisterCheck>)list.OrderByDescending(m => m.CreateTime).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        /// <summary>
        /// 检查机构名称是否可用
        /// </summary>
        /// <param name="orgName">机构名称</param>
        /// <returns>布尔值：true 为可用，false 为不可用</returns>
        public bool ExistOrgName(string orgName)
        {
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    bool result_reg = dc.RegisterCheck.FirstOrDefault(m => m.OrgName == orgName) != null;
                    bool result_org = dc.Organization.FirstOrDefault(m => m.Name == orgName) != null;
                    if (!result_reg && !result_org)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 检查用户名是否可用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>布尔值：true 为可用，false 为不可用</returns>
        public bool ExistUserName(string userName)
        {
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    bool result_reg = dc.RegisterCheck.FirstOrDefault(m => m.UserName == userName) != null;
                    bool result_org = dc.User.FirstOrDefault(m => m.UserName == userName) != null;
                    if (!result_reg && !result_org)
                    {
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Common.Argument.RetResult Add(LinqModel.RegisterCheck model)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    dc.RegisterCheck.InsertOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }


        public Common.Argument.RetResult Delete(int id)
        {
            Ret.Msg = "删除成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.RegisterCheck.FirstOrDefault(m => m.ID == id);
                    dc.RegisterCheck.DeleteOnSubmit(model);
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "删除失败，请重试！"); }
            return Ret;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Common.Argument.RetResult UpdateStatusById(int id,int status)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.RegisterCheck.FirstOrDefault(m => m.ID == id);
                    
                    model.CheckStatus = status;
                    dc.SubmitChanges();
                    
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        /// <summary>
        /// 数据转移
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Common.Argument.RetResult CopyToUserAndOrg(int id)
        {
            Ret.Msg = "添加成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var model = dc.RegisterCheck.FirstOrDefault(m => m.ID == id);
                    var orgModel = new LinqModel.Organization()
                    {
                        Name = model.OrgName,
                        Province = model.Province,
                        City = model.City,
                        District = model.District,
                        Address = model.Address,
                        Contact = model.Contact,
                        Tel = model.Tel,
                        E_mail_ = model.Email,
                        Org_URL = model.Org_URL,
                        Sup_Org = model.Sup_Org,
                        Intro = model.Intro,
                        Brand = model.Brand,
                        Cert = model.Cert,
                        Org_Code = model.Org_Code,
                        EWMUrl = model.EWMUrl,
                        HangYeID = model.HangYeID
                    };
                    dc.Organization.InsertOnSubmit(orgModel);
                    dc.SubmitChanges();
                    var user = new LinqModel.User()
                    {
                        UserName = model.UserName,
                        Password = model.UserPwd,
                        Photo = null,
                        Role_ID = 1,
                        User_Code = "自动生成",
                        Type = '2',
                        Org_ID = orgModel.Org_ID
                    };
                    dc.User.InsertOnSubmit(user);
                    dc.SubmitChanges();

                    user.User_Code = user.ID.ToString();
                    dc.SubmitChanges();

                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "添加失败，请重试！"); }
            return Ret;
        }
        /// <summary>
        /// 获得行业编号
        /// </summary>
        /// <param name="C4"></param>
        /// <returns></returns>
        public int GetHangYeId(string C4)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                var gb = dc.GBT475494.FirstOrDefault(m => m.C4 == C4);
                if (gb != null)
                {
                    return gb.ID;
                }
                return -1;
            }

        }
       /// <summary>
        /// 检查用户是否存在
       /// </summary>
       /// <param name="userName"></param>
       /// <param name="userPwd"></param>
       /// <param name="model">返回当前对象</param>
       /// <returns></returns>
        public bool CheckUser(string userName, string userPwd,out LinqModel.RegisterCheck model)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                model = (dc.RegisterCheck.FirstOrDefault(m => m.UserName == userName && m.UserPwd == userPwd));
                return (model != null);
            }
        }

        /// <summary>
        /// 获得地址
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="district"></param>
        /// <returns></returns>
        public string GetAdministrationRegion(string province,string city,string district)
        {
            var address = "";
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                address += dc.AdministrationRegion.FirstOrDefault(m => m.ID.ToString() == province).Province;
                address += dc.AdministrationRegion.FirstOrDefault(m => m.ID.ToString() == city).Province;
                address += dc.AdministrationRegion.FirstOrDefault(m => m.ID.ToString() == district).Province;
            }
            return address;
        }
    }
}
