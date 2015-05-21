using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqModel
{
    public class Trace_Info_Pics_New
    {
        public string Pic_Path { get; set; }

        public string Pic_Description { get; set; }

        public System.DateTime Up_Time { get; set; }

        public int Org_Flow_ID { get; set; }

        public int ID { get; set; }

        public int Trace_ID { get; set; }

        public string Flow_Name { get; set; }

        public System.Xml.Linq.XElement Trace_Info_Value { get; set; }

        public System.Nullable<long> Prod_Code_Start { get; set; }

        public string FlowCycles { get; set; }
    }
}
