using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace XGTech.Utility
{
    public static class MessageHelper
    {
        public static string GetMD5(string input, string key)
        {
            byte[] b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(input + key));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                stringBuilder.Append(b[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString().ToLower();
        }
        /// <summary>  
        /// 返回JSon数据  
        /// </summary>  
        /// <param name="JSONData">要处理的JSON数据</param>  
        /// <param name="Url">要提交的URL</param>  
        /// <returns>返回的JSON处理字符串</returns>  
        public static string GetResponseData(string JSONData, string Url)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JSONData);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "application/json";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //声明一个HttpWebRequest请求  
            request.Timeout = 90000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            return strResult;
        }
    }
    public class SendMessageModel
    {
        public int PartnerId { get; set; }
        public long Mobile { get; set; }
        public string Content { get; set; }
        public string Sign { get; set; }

    }
}
