using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZYNY_ZhuiSu.Models
{
    public class picup
    {
        public bool ValidateImg(string imgName)
        {
            string[] imgType = new string[] { "gif", "jpg", "png", "bmp" };

            int i = 0;
            bool blean = false;
            string message = string.Empty;

            //判断是否为Image类型文件
            while (i < imgType.Length)
            {
                if (imgName.Equals(imgType[i].ToString()))
                {
                    blean = true;
                    break;
                }
                else if (i == (imgType.Length - 1))
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
            return blean;
        }
        public string upLoadImg(string fileName)
        {
            //上传和返回(保存到数据库中)的路径
            string uppath = string.Empty;
            string savepath = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.Count > 0)
            {
                HttpPostedFile imgFile = System.Web.HttpContext.Current.Request.Files[fileName]; 
                if (imgFile != null)
                {
                    //创建图片新的名称
                    string nameImg = Common.Argument.Public.GetDateTimeNow().ToString("yyyyMMddHHmmssfff");
                    //获得上传图片的路径
                    string strPath = imgFile.FileName;
                    //获得上传图片的类型(后缀名)
                    string type = strPath.Substring(strPath.LastIndexOf(".") + 1).ToLower();
                    if (ValidateImg(type))
                    {
                        //拼写数据库保存的相对路径字符串
                        savepath = "..\\UpImgs\\";
                        savepath += nameImg + "." + type;
                        //拼写上传图片的路径
                        uppath = System.Web.HttpContext.Current.Server.MapPath("~/UpImgs/");
                        uppath += nameImg + "." + type;
                        //上传图片
                        imgFile.SaveAs(uppath);
                    }
                    return savepath;
                }
            }
            return "";
        }
        public string Upload(FormContext from)
        {
            var file = System.Web.HttpContext.Current.Request.Files["Filedata"];
            string uploadPath = System.Web.HttpContext.Current.Server.MapPath("~/images/");
            string url = "/images/" + file.FileName;
            if (file != null)
            {
                file.SaveAs(uploadPath + file.FileName);

                return url;//返回保存的地址
            }
            else
            {
                return "0";
            }

        }
    }
}