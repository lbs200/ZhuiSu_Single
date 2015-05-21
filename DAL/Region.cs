using LinqModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL
{
    public class Region : DALBase
    {

        public List<LinqModel.AdministrationRegion> GetAll()
        {
            List<LinqModel.AdministrationRegion> list = new List<LinqModel.AdministrationRegion>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.AdministrationRegion.ToList();
                }
            }
            catch { }
            return list;
        }
        public LinqModel.AdministrationRegion GetModel(int id)
        {
            LinqModel.AdministrationRegion list = new AdministrationRegion();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.AdministrationRegion.FirstOrDefault(m => m.ID == id);
                }
            }
            catch { }
            return list;
        }
        public List<LinqModel.AdministrationRegion> GetAllSheng()
        {
            List<LinqModel.AdministrationRegion> list = new List<LinqModel.AdministrationRegion>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.AdministrationRegion.Where(m => m.City == "0").ToList();
                }
            }
            catch { }
            return list;
        }
        public List<SelectListItem> Getallshengddl()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.AdministrationRegion> Itemsdt = new List<LinqModel.AdministrationRegion>();
            Itemsdt = new DAL.Region().GetAllSheng(); ;
            if (Itemsdt != null)
            {
                foreach (LinqModel.AdministrationRegion m in Itemsdt)
                {
                    list.Add(new SelectListItem { Text = m.Province, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> Getallshengddlwiths(int sid)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.AdministrationRegion> Itemsdt = new List<LinqModel.AdministrationRegion>();
            Itemsdt = new DAL.Region().GetAllSheng(); ;
            if (Itemsdt != null)
            {
                list.Add(new SelectListItem() { Text = "请选择", Value = "-1" });
                list.Add(new SelectListItem() { Text = "无上级", Value = "0",Selected=(sid==0) });
                foreach (LinqModel.AdministrationRegion m in Itemsdt)
                {
                    list.Add(new SelectListItem { Selected = (m.ID.Equals(sid.ToString())), Text = m.Province, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public List<SelectListItem> Getallshengddlwithshi(int sid, string sheng)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<LinqModel.AdministrationRegion> Itemsdt = new List<LinqModel.AdministrationRegion>();
            Itemsdt = new DAL.Region().GetAllCategory(sheng); ;
            if (Itemsdt != null)
            {
                foreach (LinqModel.AdministrationRegion m in Itemsdt)
                {
                    list.Add(new SelectListItem { Selected = (m.ID.Equals(sid.ToString())), Text = m.Province, Value = m.ID.ToString() });
                }
            }
            return list ?? null;
        }
        public Common.Argument.RetResult Update(LinqModel.AdministrationRegion model)
        {
            Ret.Msg = "修改成功！";
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var temp = dc.AdministrationRegion.FirstOrDefault(m => m.ID == model.ID);
                    temp.City = model.City;
                    temp.District = model.District;
                    temp.Postal_Code = model.Postal_Code;
                    temp.Province = model.Province;

                    dc.SubmitChanges();
                }
            }
            catch { Ret.SetArgument(Common.Argument.CmdResultError.Other, "", "修改失败，请重试！"); }
            return Ret;
        }
        public PagedList<LinqModel.AdministrationRegion> GetPgedList(string Nmae, int pageIndex, int pageSize)
        {
            PagedList<LinqModel.AdministrationRegion> temp = null;
            List<LinqModel.AdministrationRegion> list = new List<LinqModel.AdministrationRegion>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    var data = from b in dc.AdministrationRegion select b;

                    if (!string.IsNullOrEmpty(Nmae))
                    {
                        data = data.Where(m => m.Province.Contains(Nmae));
                    }


                    list = data.ToList();
                    temp = (PagedList<LinqModel.AdministrationRegion>)list.OrderByDescending(m=>m.ID).ToPagedList(pageIndex, pageSize);
                }
            }
            catch { }
            return temp;
        }
        public List<LinqModel.AdministrationRegion> GetAllCategory(string cid)
        {
            List<LinqModel.AdministrationRegion> list = new List<LinqModel.AdministrationRegion>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.AdministrationRegion.Where(model => model.City.ToString().Contains(cid)).ToList();
                }
            }
            catch { }
            return list;
        }

        public List<LinqModel.AdministrationRegion> GetAllCityBySub(int subID)
        {
            List<LinqModel.AdministrationRegion> list = new List<LinqModel.AdministrationRegion>();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    list = dc.AdministrationRegion.Where(m => m.City == subID.ToString()).ToList();
                }
            }
            catch { }
            return list;
        }
    }
}
