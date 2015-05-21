using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Org_BaoZhuang_Data : DALBase
    {
        public Common.Argument.RetResult Add(List<LinqModel.Org_BaoZhuang_Data> list)
        {
            Ret.Msg = "操作成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    bool result = false;
                    foreach (var temp in list)
                    {
                        if (temp.EWM_Num > 0)
                        {
                            int count = dc.Org_BaoZhuang_Data.Count(m => m.Prod_Code_Before == temp.Prod_Code_Before && m.EWM_Num >= temp.EWM_Num && m.EWM_Num <= temp.EWM_Num);
                            if (count > 0)
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                    if (result)
                    {
                        Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败，二维码已经被包装使用过！");
                    }
                    else
                    {
                        dc.Org_BaoZhuang_Data.InsertAllOnSubmit(list);
                        dc.SubmitChanges();
                    }
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "操作失败，请重试！"); }
            return Ret;
        }

        public List<LinqModel.Org_BaoZhuang_Data> GetList(int prodID, long ewmParent)
        {
            List<LinqModel.Org_BaoZhuang_Data> list = new List<LinqModel.Org_BaoZhuang_Data>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Org_BaoZhuang_Data.Where(m => m.Prod_ID == prodID && m.EWM_ParentNum == ewmParent && m.EWM_Num != 0).ToList();
                }
            }
            catch { list = new List<LinqModel.Org_BaoZhuang_Data>(); }
            return list;
        }
    }
}
