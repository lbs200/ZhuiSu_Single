using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Argument
{
    public class Parament
    {
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password = string.Empty;
        /// <summary>
        /// 链接字符串
        /// </summary>
        public static string ConnectString = @"Data Source=192.168.2.61;Initial Catalog=fwzsdajianew;Persist Security Info=True;User ID=fwzsdajianew;Password=fwzsdajianew";
        
        /// <summary>
        /// 字典变化最大
        /// </summary>
        public static long DictionaryLogMaxID = 0;
        /// <summary>
        /// 企业最大数量
        /// </summary>
        public static int EnterpriseCodeMainMax = 999;
        /// <summary>
        /// 产品码位数
        /// </summary>
        public static int codeBit = 18;
        /// <summary>
        /// 码段
        /// </summary>
        public static string CodeSegment = "gr96WZVvoGM=";
    }
}
