using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;


namespace XGTech.Utility
{
    public class CommonHelper
    {
   
        /// <summary>
        /// 按时间+随机4位数生成索赔单号
        /// </summary>
        public static string AutoClaimNo
        {
            get
            {
                Thread.Sleep(10);
                return "SP" + String.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + CreateRandomNum(4);//24
            }
        }
        /// <summary>
        /// 转移单编号生成规则
        /// </summary>
        public static string AutoZYNumber 
        {
            get 
            {
                Thread.Sleep(10);
                return "ZY" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(2);//24
            }
        }

        /// <summary>
        /// 入库单编号生成规则
        /// </summary>
        public static string AutoRKNumber
        {
            get
            {
                Thread.Sleep(10);
                return "RK" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(2);//24
            }
        }

        public static string AutoEstateCode
        {
            get
            {
                Thread.Sleep(10);
                return String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(4);//24
            }
        }

        /// <summary>
        /// 退回单编号生成规则
        /// </summary>
        public static string AutoTHNumber
        {
            get
            {
                Thread.Sleep(10);
                return "TH" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(2);//24
            }
        }

        /// <summary>
        /// 领用单编号生成规则
        /// </summary>
        public static string AutoZZLYNumber
        {
            get
            {
                Thread.Sleep(10);
                return "ZZCK" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(2);//24
            }
        }

        /// <summary>
        /// 领用单编号生成规则
        /// </summary>
        public static string AutoTZLYNumber
        {
            get
            {
                Thread.Sleep(10);
                return "TZCK" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(2);//24
            }
        }

        /// <summary>
        /// 维修单编号生成规则
        /// </summary>
        public static string AutoVXNumber
        {
            get
            {
                Thread.Sleep(10);
                return "VX" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(2);//24
            }
        }

        /// <summary>
        /// 报废与变卖单编号生成规则
        /// </summary>
        public static string AutoBBNumber
        {
            get
            {
                Thread.Sleep(10);
                return "BB" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(2);//24
            }
        }
        public static string AutoComplantsNo
        {
            get
            {
                Thread.Sleep(10);
                return "TS" + String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) ;//24
            }
        }
        /// <summary>
        /// 合同编号租出生成规则
        /// </summary>
        public static string AutoCZNumber
        {
            get
            {
                Thread.Sleep(10);
                return "CZ" + String.Format("{0:yyyyMMdd}", DateTime.Now) ;//24
            }
        }


        /// <summary>
        /// 合同编号租入生成规则
        /// </summary>
        public static string AutoZRNumber
        {
            get
            {
                Thread.Sleep(10);
                return "ZR" + String.Format("{0:yyyyMMdd}", DateTime.Now) + CreateRandomNum(4);//24
            }
        }

        /// <summary>
        /// 费用编号生成规则
        /// </summary>
        public static string AutoCBLXNumber
        {
            get
            {
                Thread.Sleep(10);
                return "CBLX" + String.Format("{0:yyyyMMdd}", DateTime.Now) + CreateRandomNum(4);//24
            }
        }

        /// <summary>
        /// 库位生成
        /// </summary>
        public static string AutoSLNumber
        {

            get
            {
                Thread.Sleep(10);
                return "kwd"+String.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + CreateRandomNum(4);//24
            }
        }
        public static string CreateRandomNum(int num)//生成数字随机数
        {
            string a = "0123456789";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(1, a.Length - 1)]);
            }

            return sb.ToString();
        }


       


    }
    public static class HttpContext
    {
        private static IHttpContextAccessor _accessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
    }

    public static class StaticHttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }



    /// <summary>
    /// 操作成功返回对象
    /// </summary>
    public class SuccessFul
    {
        public string Data { get; set; }

        public string Success { get; set; }
    }

    /// <summary>
    /// 操作失败返回对象
    /// </summary>
    public class Failure
    {
        public string Msg { get; set; }

        public string Success { get; set; }
    }


}
