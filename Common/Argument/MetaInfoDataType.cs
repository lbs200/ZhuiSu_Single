using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Argument
{
    public class MetaInfoDataType
    {
        public string Name { get; set; }
        public string ValueStringFormat { get; set; }

        public MetaInfoDataType(string n, string v)
        {
            Name = n;
            ValueStringFormat = v;
        }

    }
}
