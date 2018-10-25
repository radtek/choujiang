using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.DrawingCore;
using System.IO;

namespace XGTech.Web.Controllers
{
    public class VerifyCodeController : Controller
    {
        public VerifyCodeController()
        {

        }
        private string _sessionName = "verifyCode";

        //
        // GET: /VerifyCode/
        [AllowAnonymous]
        public FileContentResult Index(decimal fontSize = 22.5M, int imageHeight = 42)
        {
            _sessionName = "loginVerifyCode";

            return GraphicsImage(4, fontSize, imageHeight);
        }

        public FileContentResult Member()
        {
            _sessionName = "memberVerifyCode";

            return GraphicsImage(4);
        }

        public FileContentResult Reg()
        {
            _sessionName = "regVerifyCode";

            return GraphicsImage(4);
        }

        public FileContentResult Staff()
        {
            _sessionName = "staffVerifyCode";

            return GraphicsImage(4);
        }

        public FileContentResult Mana()
        {
            _sessionName = "manaVerifyCode";

            return GraphicsImage(4);
        }

        private object[] CreateString(int strlength)
        {
            //定义一个数组存储汉字编码的组成元素
            string[] str = new string[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random ran = new Random();  //定义一个随机数对象
            object[] bytes = new object[strlength];
            for (int i = 0; i < strlength; i++)
            {
                //获取区位码第一位
                int ran1 = ran.Next(11, 14);
                string str1 = str[ran1].Trim();

                //获取区位码第二位并防止数据重复
                ran = new Random(ran1 * unchecked((int)DateTime.Now.Ticks) + i);
                int ran2;
                if (ran1 == 13)
                {
                    ran2 = ran.Next(0, 7);
                }
                else
                {
                    ran2 = ran.Next(0, 16);
                }
                string str2 = str[ran2].Trim();

                //获取区位码第三位
                ran = new Random(ran2 * unchecked((int)DateTime.Now.Ticks) + i);
                int ran3 = ran.Next(10, 16);
                string str3 = str[ran3].Trim();

                //获取区位码第四位
                ran = new Random(ran3 * unchecked((int)DateTime.Now.Ticks) + i);
                int ran4;
                if (ran3 == 10)
                {
                    ran4 = ran.Next(1, 16);
                }
                else if (ran3 == 15)
                {
                    ran4 = ran.Next(0, 15);
                }
                else
                {
                    ran4 = ran.Next(0, 16);
                }
                string str4 = str[ran4].Trim();

                //定义字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str1 + str2, 16);
                byte byte2 = Convert.ToByte(str3 + str4, 16);

                byte[] stradd = new byte[] { byte1, byte2 };
                //将产生的汉字字节放入数组
                bytes.SetValue(stradd, i);
            }
            return bytes;
        }

        private string GetString(int length)
        {
            Encoding gb = Encoding.GetEncoding("gb2312");
            object[] bytes = CreateString(length);

            //根据汉字字节解码出中文汉字
            string str1 = gb.GetString((byte[])Convert.ChangeType(bytes[0], typeof(byte[])));
            string str2 = gb.GetString((byte[])Convert.ChangeType(bytes[1], typeof(byte[])));
            string str3 = gb.GetString((byte[])Convert.ChangeType(bytes[2], typeof(byte[])));
            string str4 = gb.GetString((byte[])Convert.ChangeType(bytes[3], typeof(byte[])));

            string str = str1 + str2 + str3 + str4;
            ///返回前记入Session
            HttpContext.Session.SetString(_sessionName, str);
            return str;
        }

        private FileContentResult GraphicsImage(int charTotal, decimal fontSize = 22.5M, int imageHeight = 42)
        {
            var code = GetRandomCharAndSetSession(charTotal);

            System.DrawingCore.Bitmap image = new System.DrawingCore.Bitmap((int)Math.Ceiling((code.Length * fontSize)), imageHeight);
            System.DrawingCore.Graphics g = System.DrawingCore.Graphics.FromImage(image);  //创建画布

            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(System.DrawingCore.Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 1; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new System.DrawingCore.Pen(System.DrawingCore.Color.Black), x1, y1, x2, y2);
                }

                System.DrawingCore.Font font = new System.DrawingCore.Font("隶书", 20, System.DrawingCore.FontStyle.Underline);
                LinearGradientBrush brush = new LinearGradientBrush
                    (new System.DrawingCore.Rectangle(0, 0, image.Width, image.Height), System.DrawingCore.Color.Blue, System.DrawingCore.Color.DarkRed, 1.2f, true);
         
                g.DrawString(code, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, System.DrawingCore.Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new System.DrawingCore.Pen(System.DrawingCore.Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Gif);
                return File(ms.ToArray(), "image/Gif");
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetRandomCharAndSetSession(int charTotal)
        {
            string allChar = "2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z";
            //string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            int nextMax = allChar.Replace(",", "").Length - 1;
            for (int i = 0; i < charTotal; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                //int t = rand.Next(61);
                int t = rand.Next(nextMax);
                //while (temp == t)
                //{
                //    rand = new Random((t + 1) * temp * ((int)DateTime.Now.Ticks));
                //    t = rand.Next(9);
                //}
                temp = t;
                randomCode += allCharArray[temp];
            }
            HttpContext.Session.SetString(_sessionName, randomCode);
            return randomCode;
        }
    }
}