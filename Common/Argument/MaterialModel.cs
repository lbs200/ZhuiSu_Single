using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Argument
{
    public class MaterialModel
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public long PID { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string PName { get; set; }              
        /// <summary>
        /// 产品全称
        /// </summary>
        public string PFullName { get; set; }
        /// <summary>
        /// 产品介绍
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string DictionaryType { get; set; }
        /// <summary>
        /// 字段类型ID
        /// </summary>
        public string DictionaryType_ID { get; set; }
        /// <summary>
        /// 产品属性
        /// </summary>
        public string MaterialInfo { get; set; }
    }
}
