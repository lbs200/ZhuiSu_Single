using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Common.Argument
{
    public class Public
    {
        /// <summary>
        /// 菜单列表（所有）
        /// </summary>
        public static List<LinqModel.View_S_Tree> listSTree { get; set; }
        /// <summary>
        /// 当前登陆用户功能权限列表
        /// </summary>
        public static List<LinqModel.Permission> listPermission { get; set; }
        /// <summary>
        /// 追溯元流程表
        /// </summary>
        //public static List<LinqModel.Meta_Flow> listMeta_Flow { get; set; }
        /// <summary>
        /// 信息元数据
        /// </summary>
        //public static List<LinqModel.Meta_Info> listMeta_Info { get; set; }
        public static List<LinqModel.AdministrationRegion> listAddress = new List<LinqModel.AdministrationRegion>();

        public static List<MetaInfoDataType> listDataType = new List<MetaInfoDataType> { 
            new MetaInfoDataType("单行文本框",""),
            new MetaInfoDataType("整数",""),
            new MetaInfoDataType("浮点数",""),
            new MetaInfoDataType("多行文本框",""),
            new MetaInfoDataType("下拉框",""),
            new MetaInfoDataType("多选框",""),
            new MetaInfoDataType("单选框",""),
            new MetaInfoDataType("日期类型",""),
        new MetaInfoDataType("链接",""),
        new MetaInfoDataType("图片",""),
        new MetaInfoDataType("用途码","")
        };

        public static string GetDataTypeHtml(string dataType, string label, string id, bool required, string dataTypeValue, string remark)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                switch (dataType)
                {
                    case "单行文本框":
                    case "整数":
                    case "浮点数":
                        #region HTML
                        if (required)
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                            if (!string.IsNullOrEmpty(remark))
                            {
                                sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                            }
                            sb.Append("<div class=\"controls\"><input title=\"" + dataType + "\" required=\"" + label + "必填\" type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" value=\"\" /> <span style='color:red;'>*</span></div></div>");
                        }
                        else
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                            if (!string.IsNullOrEmpty(remark))
                            {
                                sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                            }
                            sb.Append("<div class=\"controls\"><input title=\"" + dataType + "\"  type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" value=\"\" /> </div></div>");
                        }
                        #endregion
                        break;
                    case "多行文本框":
                        #region HTML
                        if (required)
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                            if (!string.IsNullOrEmpty(remark))
                            {
                                sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                            }
                            sb.Append("<div class=\"controls\"><textarea title=\"" + dataType + "\" required=\"" + label + "必填\"  class=\"large m-wrap\" rows=\"3\"  id=\"" + id + "\" name=\"" + id + "\" value=\"\" ></textarea> </div></div>");
                        }
                        else
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                            if (!string.IsNullOrEmpty(remark))
                            {
                                sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                            }
                            sb.Append("<div class=\"controls\"><textarea title=\"" + dataType + "\"  class=\"large m-wrap\" rows=\"3\"  id=\"" + id + "\" name=\"" + id + "\" value=\"\" ></textarea> </div></div>");
                        }
                        #endregion
                        break;
                    case "下拉框":
                    case "用途码":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                        if (!string.IsNullOrEmpty(remark))
                        {
                            sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                        }
                        sb.Append("<div class=\"controls\">");
                        if (required)
                        {
                            sb.Append("<select class=\"medium m-wrap\" tabindex=\"1\"  id=\"" + id + "\" name=\"" + id + "\"  required=\"" + label + "必填\"><span style='color:red;'>*</span>");
                        }
                        else
                        {
                            sb.Append("<select class=\"medium m-wrap\" tabindex=\"1\"  id=\"" + id + "\" name=\"" + id + "\" >");
                            sb.Append("<option value=\"请选择\">请选择</option>");
                        }
                        foreach (var mm in dataTypeValue.Split(','))
                        {
                            if (!string.IsNullOrEmpty(mm))
                            {
                                sb.Append("<option value=\"" + mm + "\">" + mm + "</option>");
                            }
                        }
                        sb.Append("</select>");
                        sb.Append("</div></div>");
                        #endregion
                        break;
                    case "多选框":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                        if (!string.IsNullOrEmpty(remark))
                        {
                            sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                        }
                        sb.Append("<div class=\"controls\">");
                        foreach (var mm in dataTypeValue.Split(','))
                        {
                            if (!string.IsNullOrEmpty(mm))
                                sb.Append("<label class=\"checkbox\"><input type=\"checkbox\" value=\"" + mm + "\" name=\"" + id + "\"  id=\"" + id + "\" />" + mm + "</label>");
                        }
                        sb.Append("</div></div>");
                        #endregion
                        break;
                    case "单选框":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                        if (!string.IsNullOrEmpty(remark))
                        {
                            sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                        }
                        sb.Append("<div class=\"controls\">");
                        int counted = 1;
                        foreach (var mm in dataTypeValue.Split(','))
                        {
                            if (!string.IsNullOrEmpty(mm))
                            {
                                if (counted == 1)
                                    sb.Append("<label class=\"radio\"><input type=\"radio\" name=\"" + id + "\"  id=\"" + id + "\" value=\"" + mm + "\" checked />" + mm + "</label>");
                                else
                                    sb.Append("<label class=\"radio\"><input type=\"radio\" name=\"" + id + "\"  id=\"" + id + "\"  value=\"" + mm + "\"  />" + mm + "</label>");
                                counted++;
                            }
                        }
                        sb.Append("</div></div>");
                        #endregion
                        break;
                    case "日期类型":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                        if (!string.IsNullOrEmpty(remark))
                        {
                            sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                        }
                        sb.Append("<div class=\"controls\"><input   type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" value=\"" + Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd") + "\"  readonly=\"readonly\" onclick=\"WdatePicker();\" onfocus=\"WdatePicker()\" /> </div></div>");
                        #endregion
                        break;
                    case "链接":
                        #region HTML
                        if (required)
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + " (名称|链接地址) ：</label>");
                            if (!string.IsNullOrEmpty(remark))
                            {
                                sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                            }
                            sb.Append("<div class=\"controls\"><input title=\"" + dataType + "\"   type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" required=\"" + label + "必填\"  /> </div></div>");
                        }
                        else
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + " (名称|链接地址) ：</label>");
                            if (!string.IsNullOrEmpty(remark))
                            {
                                sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                            }
                            sb.Append("<div class=\"controls\"><input title=\"" + dataType + "\"   type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\"   /> </div></div>");
                        }
                        #endregion
                        break;
                    case "图片":
                        #region HTML

                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label>");
                        if (!string.IsNullOrEmpty(remark))
                        {
                            sb.Append("<div class=\"alert alert-error show\"><button class=\"close\" data-dismiss=\"alert\"></button><span>" + remark + "</span></div>");
                        }
                        sb.Append("<div class=\"controls\">");
                        sb.Append("<input title=\"" + dataType + "\"  type=\"hidden\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" value=\"\" /><br> ");
                        sb.Append("<input type=\"button\" value=\"选择\" onclick=\"BrowseServer('" + id + "');\" /><input type=\"button\" value=\"选择后预览\" onclick=\"Yulan('" + id + "');\" /><br/>");
                        sb.Append("<img id=\"stuPic" + id + "\" width=\"300\" height=\"200\"  style=\"display:none;\" />");
                        sb.Append("</div></div>");

                        #endregion
                        break;
                }

                return sb.ToString();
            }
            catch { return string.Empty; }
        }

        public static string GetDataTypeHtmlSearch(string dataType, string label, string id, bool required, string dataTypeValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                switch (dataType)
                {
                    case "单行文本框":
                    case "整数":
                    case "浮点数":
                        #region HTML
                        if (required)
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\"><input title=\"" + dataType + "\" required=\"" + label + "必填\" type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" value=\"\" /> <span style='color:red;'>*</span></div></div>");
                        }
                        else
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\"><input title=\"" + dataType + "\"  type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" value=\"\" /> </div></div>");
                        }
                        #endregion
                        break;
                    case "多行文本框":
                        #region HTML
                        if (required)
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\"><textarea title=\"" + dataType + "\" required=\"" + label + "必填\"  class=\"large m-wrap\" rows=\"3\"  id=\"" + id + "\" name=\"" + id + "\" value=\"\" ></textarea> </div></div>");
                        }
                        else
                        {
                            sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\"><textarea title=\"" + dataType + "\"  class=\"large m-wrap\" rows=\"3\"  id=\"" + id + "\" name=\"" + id + "\" value=\"\" ></textarea> </div></div>");
                        }
                        #endregion
                        break;
                    case "下拉框":
                    case "用途码":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\">");
                        if (required)
                        {
                            sb.Append("<select class=\"medium m-wrap\" tabindex=\"1\"  id=\"" + id + "\" name=\"" + id + "\"  required=\"" + label + "必填\"><span style='color:red;'>*</span>");
                            sb.Append("<option value=\"请选择\">全部</option>");
                        }
                        else
                        {
                            sb.Append("<select class=\"medium m-wrap\" tabindex=\"1\"  id=\"" + id + "\" name=\"" + id + "\" >");
                            sb.Append("<option value=\"请选择\">全部</option>");
                        }
                        foreach (var mm in dataTypeValue.Split(','))
                        {
                            sb.Append("<option value=\"" + mm + "\">" + mm + "</option>");
                        }
                        sb.Append("</select>");
                        sb.Append("</div></div>");
                        #endregion
                        break;
                    case "多选框":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\">");
                        foreach (var mm in dataTypeValue.Split(','))
                        {
                            sb.Append("<label class=\"checkbox\"><input type=\"checkbox\" value=\"" + mm + "\" name=\"" + id + "\"  id=\"" + id + "\" />" + mm + "</label>");
                        }
                        sb.Append("</div></div>");
                        #endregion
                        break;
                    case "单选框":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\">");
                        sb.Append("<label class=\"radio\"><input type=\"radio\" name=\"" + id + "\"  id=\"" + id + "\" value=\"0\" checked />全部</label>");
                        foreach (var mm in dataTypeValue.Split(','))
                        {
                            sb.Append("<label class=\"radio\"><input type=\"radio\" name=\"" + id + "\"  id=\"" + id + "\"  value=\"" + mm + "\"  />" + mm + "</label>");
                        }
                        sb.Append("</div></div>");
                        #endregion
                        break;
                    case "日期类型":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + "  ：</label><div class=\"controls\"><input   type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" value=\"" + Common.Argument.Public.GetDateTimeNow().ToString("yyyy-MM-dd") + "\"  readonly=\"readonly\" onclick=\"WdatePicker();\" onfocus=\"WdatePicker()\" /> </div></div>");
                        #endregion
                        break;
                    case "链接":
                        #region HTML
                        sb.Append("<div class=\"control-group\"><label class=\"control-label\">" + label + " (名称|链接地址) ：</label><div class=\"controls\"><input  title=\"" + dataType + "\"   type=\"text\" class=\"m-wrap\" id=\"" + id + "\" name=\"" + id + "\" required /> </div></div>");
                        #endregion
                        break;
                }

                return sb.ToString();
            }
            catch { return string.Empty; }
        }

        public static string GetEWMPass(string url, string ewm)
        {
            string result = string.Empty;
            int sum = 0;
            string[] strs = ewm.Split('.');
            string sss = strs[strs.Length - 1];
            for (int j = 0; j < sss.Length; j++)
            {
                sum += Convert.ToInt32(sss.Substring(j, 1));//累加每一个数字
            }
            string content = url + ewm + sum;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(content);
            byte[] toData = md5.ComputeHash(fromData);
            string byteStr = null;
            for (int m = 0; m < toData.Length; m++)
            {
                byteStr += toData[m].ToString("x");
            }
            content = byteStr.Substring(0, 16);
            for (int j = 0; j < content.Length; j++)
            {
                int n;
                if (int.TryParse(content.Substring(j, 1), out n))
                {
                    result += content.Substring(j, 1) + "";//累加每一个数字
                }

            }
            if (result.Length < 6)
            {
                int js = result.Length;
                for (int xx = 0; xx < 6 - js; xx++)
                {
                    result += "0";
                }

            }
            result = result.Substring(0, 6);
            return result;
        }

        public static string GetFirstImages(string htmlText)
        {
            string srcUrl = string.Empty;

            try
            {
                int nStart = htmlText.IndexOf("<img");
                htmlText = htmlText.Substring(nStart);
                srcUrl = htmlText.Substring(0, htmlText.IndexOf("/>") + 2).Replace("\\", "");
                string[] imgAttr = srcUrl.Split(' ');// "将图片属性根据空格分开"  
                foreach (string item in imgAttr)//遍历属性  
                {
                    if (item.IndexOf("src") >= 0)//如果有src属性，把值 保存起来  保存的是完整的src属性  :src=""  
                    {
                        srcUrl = item.ToString();
                        break;
                    }

                }
                return srcUrl = srcUrl.Substring(srcUrl.IndexOf('"') + 1, srcUrl.LastIndexOf('"') - srcUrl.IndexOf('"') - 1);//把保存的src属性中的值取出来 去掉src="和最后一个"  
            }
            catch { srcUrl = string.Empty; }

            return srcUrl;
        }

        public static string CutStr(string str, int length, byte isDi)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            Regex regex = new Regex("[^\x00-\xff]+", RegexOptions.Compiled);

            char[] stringChar = str.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    nLength += 2;
                }
                else
                {
                    nLength = nLength + 1;
                }

                if (nLength <= length)
                {
                    sb.Append(stringChar[i]);
                }
                else
                {
                    break;
                }
            }

            if (isDi == 1)
            {
                if (sb.ToString() != str)
                {
                    sb.Append("...");
                }
            }
            return sb.ToString();
        }

        public static string HtmlClear(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                Htmlstring = "";
            }
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"([\r])[\s]+", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&ldquo;", "“", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&rdquo;", "”", RegexOptions.IgnoreCase);

            Htmlstring = Htmlstring.Replace("<", "");

            Htmlstring = Htmlstring.Replace(">", "");

            Htmlstring = Htmlstring.Replace("\r", "");

            Htmlstring = HtmlEncode(Htmlstring).Trim();

            return Htmlstring;
        }

        public static string HtmlEncode(string EnCodeHtml)
        {
            try
            {
                EnCodeHtml = HttpContext.Current.Server.HtmlEncode(EnCodeHtml);
            }
            catch { }
            return EnCodeHtml;
        }
        public static string HtmlDecode(string DeCodeHtml)
        {
            try
            {
                DeCodeHtml = HttpContext.Current.Server.HtmlDecode(DeCodeHtml);
            }
            catch { }
            return DeCodeHtml;
        }
        public static DateTime GetDateTimeNow()
        {
            return DateTime.Now.AddHours(8);
        }
    }
}
