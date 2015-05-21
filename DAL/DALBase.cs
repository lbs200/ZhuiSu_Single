using LinqModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALBase
    {
        protected static Common.Argument.RetResult Ret;
        //public static LinqModel.DataClassesZYZSDataContext dc;
        public DALBase()
        {
            Ret = new Common.Argument.RetResult();
        }

        public static LinqModel.DataClassesZYZSDataContext GetDataContext()
        {
           return new LinqModel.DataClassesZYZSDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectString"]);
        }
    }
}
