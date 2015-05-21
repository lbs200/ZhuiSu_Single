using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Ent_Code_Pool : DALBase
    {
        public LinqModel.Ent_Code_Pool GetModel(int Scheme_ID)
        {
            LinqModel.Ent_Code_Pool model = new LinqModel.Ent_Code_Pool();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.Ent_Code_Pool.FirstOrDefault(m => m.Scheme_ID == Scheme_ID);
                }
            }
            catch { model = new LinqModel.Ent_Code_Pool(); }
            return model;
        }
    }
}
