﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using XGTech.Model.Model;
using XGTech.Web.Database;

namespace XGTech.Web.Common
{
    public class CommonHelper
    {

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static String GetUserName(Int64 userid)
        {
            var db = DBHelper.GetInstance();

            var model = db.Queryable<Tbl_user>().Where(t1 => t1.id == userid).Single();

            if (model != null && !String.IsNullOrEmpty(model.cell))
            {
                return model.cell;
            }

            return String.Empty;
        }

        public static string GetMd5_32byte(string str)
        {
            string pwd = string.Empty;

            //实例化一个md5对像
            MD5 md5 = MD5.Create();

            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }

            return pwd;
        }

    }
}
