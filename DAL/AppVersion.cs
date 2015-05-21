using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AppVersion : DALBase
    {
        public LinqModel.AppVersion GetNew()
        {
            LinqModel.AppVersion model = new LinqModel.AppVersion();
            try
            {
                using (LinqModel.DataClassesZYZSDataContext dc = GetDataContext())
                {
                    model = dc.AppVersion.OrderByDescending(m => m.ID).FirstOrDefault();
                }
            }
            catch { model = new LinqModel.AppVersion(); }
            return model;
        }
    }
}
