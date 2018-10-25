using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace XGTech.Utility
{
    public class EncodeHepler
    {
        /// <summary>
        /// 加密用的key
        /// </summary>
        public static string key = "314159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914127372458700660631558817488152092096282925409171536436789259036001133053054882046652138414695194151160";

        /// <summary>
        /// 加密连接字符串
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static string EncConStr(string constr, string encConStrKey)
        {
            string ret = Enc(encConStrKey, constr);
            return ret;
        }
        /// <summary>
        /// 解密连接字符串
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static string DecConStr(string constr, string encConStrKey)
        {
            string src = Dec(encConStrKey, constr);
            return src;
        }
        /// <summary>
        /// 加密数据库密码
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static string EncDataBasePass(string pass, string encDataBasePassword)
        {
            string ret = Enc(encDataBasePassword, pass);
            return ret;
        }
        /// <summary>
        /// 解密数据库密码
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static string DecDataBasePass(string pass, string encDataBasePassword)
        {
            string src = Dec(encDataBasePassword, pass);
            return src;
        }
        /// <summary>
        /// 加密用户密码
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static string EncUserPass(string pass, string encLoginKey)
        {
            string ret = Enc(encLoginKey, pass);
            return ret;
        }
        /// <summary>
        /// 解密用户密码
        /// </summary>
        /// <param name="constr"></param>
        /// <returns></returns>
        public static string DecUserPass(string pass, string encLoginKey)
        {
            string src = Dec(encLoginKey, pass);
            return src;
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="RandSeed">密钥</param>
        /// <param name="src">需要加密的字符串</param>
        /// <returns></returns>
        public static string Enc(string RandSeed, string src)
        {
            int baseval = GetBaseKeyIndex(RandSeed);
            string val = "";
            for (int i = 0; i < src.Length; i++)
            {
                val += (char)(src[i] + (char)int.Parse(new string(EncodeHepler.key[baseval + i], 1)));
            }
            string pass = EncodeBase64("utf-8", val);
            return pass;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="RandSeed">密钥</param>
        /// <param name="str">需要解密的字符串</param>
        /// <returns></returns>
        public static string Dec(string RandSeed, string str)
        {
            int baseval = GetBaseKeyIndex(RandSeed);
            string src = DecodeBase64("utf-8", str);
            string val = "";
            for (int i = 0; i < src.Length; i++)
            {
                val += (char)(src[i] - (char)int.Parse(new string(EncodeHepler.key[baseval + i], 1)));
            }
            return val;
        }

        public static int GetBaseKeyIndex(string RandSeed)
        {
            string passkey = RandSeed;
            int val = 0;
            for (int i = 0; i < passkey.Length; i++)
            {
                byte c = (byte)passkey[i];
                val += c;
            }
            val = val % 179;
            return val;
        }

        /// <summary>
        /// Base64编码（utf-8）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncodeBase64(string code)
        {
            return EncodeBase64("utf-8", code);
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="code_type">编码方式</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string EncodeBase64(string code_type, string code)
        {
            string encode = string.Empty;
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        }

        /// <summary>
        /// Base64解码（utf-8）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code)
        {
            return DecodeBase64("utf-8", code);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="code_type">编码方式</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns>加密后的字符串（小写）</returns>
        public static string GetMD5(string input)
        {
            string str = input;
            byte[] b = System.Text.Encoding.ASCII.GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret.ToLower();
        }

        /// <summary>
        /// Hashtable序列化为字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string EncodeHashtableToBase64(Hashtable input, string encode)
        {
            Hashtable ht = (Hashtable)input;
            if (ht == null || ht.Count == 0) return "";
            //    IDictionaryEnumerator ide = ht.GetEnumerator();
            //    ide.Reset();
            StringBuilder temp = new StringBuilder();
            // while (ide.MoveNext())
            foreach (DictionaryEntry ide in ht)
            {
                temp.Append(ide.Key.ToString());
                temp.Append("[-]");
                temp.Append(ide.Value.ToString());
                temp.Append("[-]");
            }
            string base64 = EncodeBase64(encode, temp.ToString());
            return base64;
        }

        /// <summary>
        /// 字符串反序列化为Hashtable
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static Hashtable DecodeBase64ToHashtable(string base64, string encode)
        {
            string temp = DecodeBase64(encode, base64);
            string[] split = new string[] { "[-]" };
            string[] arr = temp.Split(split, StringSplitOptions.None);
            Hashtable ht = new Hashtable();
            for (int i = 1; i < arr.Length; i += 2)
            {
                if (arr[i - 1].Length > 0)
                    ht[arr[i - 1]] = arr[i];
            }
            return ht;
        }
    }
}
