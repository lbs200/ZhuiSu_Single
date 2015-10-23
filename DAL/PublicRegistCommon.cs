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
        /// <param name="prodID">产品ID</param>
        /// <returns></returns>
        public bool AddFlowAndPermission(int orgID, int userID, int hangyeID, int prodID)
        {
            bool result = false;
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var modelHangye = dc.Organization.FirstOrDefault(m => m.HangYeID == hangyeID);
                    if (modelHangye != null && modelHangye.Org_ID > 0)
                    {
                        //获取流程列表
                        List<LinqModel.Org_Flow> listFlow = dc.Org_Flow.Where(m => m.Org_ID == modelHangye.Org_ID).OrderBy(m => m.Seq_No).ToList();
                        if (listFlow != null && listFlow.Count > 0)
                        {
                            for (int i = 0; i < listFlow.Count; i++)
                            {
                                //获取该流程下的信息元
                                List<LinqModel.Org_Info> listInfo = dc.Org_Info.Where(m => m.Org_Flow_ID == listFlow[i].Org_Flow_ID).ToList();
                                #region 添加企业追溯流程信息
                                var modelTempFlow = new LinqModel.Org_Flow();
                                modelTempFlow.Current_State = listFlow[i].Current_State;
                                modelTempFlow.Flow_ID = listFlow[i].Flow_ID;
                                modelTempFlow.Org_ID = orgID;
                                modelTempFlow.Prod_ID = prodID;
                                modelTempFlow.Seq_No = listFlow[i].Seq_No;
                                if (listFlow[i].Sup_Flow_ID != 0 && i != 0)
                                {
                                    var mtemp = dc.Org_Flow.FirstOrDefault(m => m.Org_ID == orgID && m.Prod_ID == prodID && m.Flow_ID == listFlow[i - 1].Flow_ID);//获取上一级ID
                                    modelTempFlow.Sup_Flow_ID = mtemp.Org_Flow_ID;
                                }
                                else
                                {
                                    modelTempFlow.Sup_Flow_ID = 0;
                                }
                                dc.Org_Flow.InsertOnSubmit(modelTempFlow);//添加企业追溯流程信息
                                dc.SubmitChanges();
                                #endregion
                                #region 添加权限信息
                                var modelFlowUser = new LinqModel.Flow_User();
                                modelFlowUser.Org_Flow_ID = modelTempFlow.Org_Flow_ID;
                                modelFlowUser.Use_ID = userID;
                                dc.Flow_User.InsertOnSubmit(modelFlowUser);
                                dc.SubmitChanges();
                                #endregion
                                List<LinqModel.Org_Info> listInsert = new List<LinqModel.Org_Info>();
                                #region 添加企业流程信息元
                                for (int j = 0; j < listInfo.Count; j++)
                                {
                                    if (listInfo[j].Info_ID == 1)
                                    {
                                        listInfo[j].Org_Flow_ID = listFlow[i].Org_Flow_ID;
                                        var modelTempInfo = new LinqModel.Org_Info();
                                        modelTempInfo.Data_Type_Value = listInfo[j].Data_Type_Value;
                                        modelTempInfo.Info_ID = listInfo[j].Info_ID;
                                        modelTempInfo.Org_Flow_ID = modelTempFlow.Org_Flow_ID;
                                        modelTempInfo.Public = listInfo[j].Public;
                                        modelTempInfo.Required = listInfo[j].Required;
                                        modelTempInfo.Search_Point = listInfo[j].Search_Point;
                                        listInsert.Add(modelTempInfo);
                                    }
                                }
                                if (listInsert.Count > 0)
                                {
                                    dc.Org_Info.InsertAllOnSubmit(listInsert);//添加企业流程信息元
                                    dc.SubmitChanges();
                                } 
                                #endregion
                                i++;
                            }
                        }
                    }
                    else
                    {
                        result = false;//没有此行业的模板
                    }
                }
            }
            catch { result = false; }
            return result;
        }
    }
}
