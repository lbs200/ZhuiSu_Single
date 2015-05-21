using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqModel;
using System.IO;
using System.Configuration;
namespace DAL
{

    public class Products : DALBase
    {
        public List<LinqModel.Products> GetAll()
        {
            List<LinqModel.Products> list = new List<LinqModel.Products>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Products.ToList();
                }
            }
            catch { }
            return list;
        }
        /// <summary>
        /// 由产品编号获得视频路径
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="url">传出路径</param>
        /// <returns>是否有路径</returns>
        public bool VideoUrlById(int prodId,out string url)
        {
            url = null;
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                var product = dc.Products.FirstOrDefault(m => m.ID == prodId);
                if (product != null)
                {
                    if (product.VideoUrl != null && product.VideoUrl != string.Empty)
                    {
                        url = product.VideoUrl;
                        return true;
                    }
                }
                else
                {
                    url = null;
                }
            }
            return false;
        }
        /// <summary>
        /// 根据产品编号和二维码判断产品是否存在
        /// </summary>
        /// <param name="ewm"></param>
        /// <param name="proid"></param>
        /// <returns></returns>
        public bool Exist(int proID, string ewm, int orgID)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                var product = dc.Products.FirstOrDefault(m => m.ID == proID && m.Org_ID == orgID);
                if (product != null)
                {
                    return ewm.StartsWith(product.Adv_Code);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool Exict(string name,int oid)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                int c = dc.Products.Where(m => m.Name == name&m.Org_ID==oid).Count();

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
        public bool ExictSX(string SX,int oid)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                int c = dc.Products.Where(m => m.Abbr.Equals(SX) & m.Org_ID == oid).Count();

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
        public Common.Argument.RetResult UpdateID(LinqModel.Products model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    
                    var orgm = dc.Organization.FirstOrDefault(m => m.Org_ID == model.Org_ID);
                    var temp = dc.Products.FirstOrDefault(m => m.ID == model.ID);
                    temp.Abbr = model.Abbr;
                    temp.Adv_Code = orgm.Org_Code + orgm.Org_Code.Substring(1, 1) + "1" + model.ID.ToString() + orgm.Org_Code.Substring(1, 1);
                    temp.Class = "0";
                    temp.Company = model.Company;
                    temp.Count = model.Count;
                    temp.Description = model.Description;
                    temp.material = model.material;
                    temp.Name = model.Name;
                    temp.NET = model.NET;
                    temp.Org_ID = model.Org_ID;
                    temp.Prod_Category = model.Prod_Category;
                    temp.Spec = "0";
                    temp.Unit = "0";
                    temp.Photo =model.Photo;
                    temp.VideoUrl = model.VideoUrl;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public void WriteToFile(string name, string content, bool isCover)
        {
            FileStream fs = null;
            try
            {
                if (!isCover && System.IO.File.Exists(name))
                {
                    fs = new FileStream(name, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                    sw.WriteLine(content);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    System.IO.File.WriteAllText(name, content, Encoding.UTF8);
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

        }
        public List<LinqModel.Products> GetAllwithORGID(int orgiid)
        {
            List<LinqModel.Products> list = new List<LinqModel.Products>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Products.Where(m=>m.Org_ID==orgiid).ToList();
                }
            }
            catch { }
            return list;
        }
        public bool Exist(string name)
        {
            using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
            {
                if (dc.Products.Where(m => m.Name == name) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Common.Argument.RetResult Update(LinqModel.Products model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.Products.FirstOrDefault(m => m.ID == model.ID);
                    temp.Name = model.Name;
                    temp.Abbr = model.Abbr;
                    temp.Adv_Code = model.Adv_Code;

                    temp.Class = model.Class;
                    temp.Company = model.Company;

                    temp.Count = model.Count;
                    temp.Description = model.Description;
                    temp.material = model.material;
                    temp.NET = model.NET;
                    temp.Org_ID = model.Org_ID;
                    temp.Photo = model.Photo;
                    temp.Prod_Category = model.Prod_Category;
                    temp.Spec = model.Spec;
                    temp.Unit = model.Unit;
                    temp.Max_SN = model.Max_SN;
                    temp.CodeScheme = model.CodeScheme;
                    temp.EWMUrl = model.EWMUrl;
                    temp.BuyUrl = model.BuyUrl;
                    temp.VideoUrl = model.VideoUrl;
                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public LinqModel.Products GetModel(int id)
        {
            LinqModel.Products list = new LinqModel.Products();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.Products.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public PagedList<LinqModel.Products> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.Products> temp = null;
            List<LinqModel.Products> list = new List<LinqModel.Products>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Products select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Products>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.Products> GetPgedListWithORG(string Nmae, int pageIndex, int pageSize,string oid)
        {
            PagedList<LinqModel.Products> temp = null;
            List<LinqModel.Products> list = new List<LinqModel.Products>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Products select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(oid))
                    {
                        if(!oid.Contains("-1"))
                        {
                            data = data.Where(m => m.Org_ID.Equals(oid));
                        }
                        
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Products>)list.OrderByDescending(m => m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.Products> GetPgedListWithOID(string Nmae, int pageIndex, int pageSize,int oid)
        {
            PagedList<LinqModel.Products> temp = null;
            List<LinqModel.Products> list = new List<LinqModel.Products>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Products select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                        
                    }
                    if (!string.IsNullOrEmpty(oid.ToString()))
                    {
                       
                        data = data.Where(m => m.Org_ID == oid);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Products>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public PagedList<LinqModel.Products> GetPgedListNew(string Nmae, int pageIndex, int pageSize,int orgid)
        {
            PagedList<LinqModel.Products> temp = null;
            List<LinqModel.Products> list = new List<LinqModel.Products>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.Products select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Name.Contains(Nmae));
                    }
                    if (!string.IsNullOrEmpty(orgid.ToString()))
                    {
                        data = data.Where(m => m.Org_ID==orgid);
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.Products>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
    }
}
