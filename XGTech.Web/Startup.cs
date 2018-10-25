using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using StarNet.Multimedia.IO.Helper.Excels;
using System;
using XGTech.BLL;
using XGTech.Config;
using XGTech.IBLL;
using XGTech.Utility;
using XGTech.Web.Common;
using XGTech.Web.Infrastructure;
using XGTech.Web.Models;

namespace XGTech.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSession();
            services.AddMvc();
            //services.AddTransient<IUserService, UserService>();
            services.AddHttpContextAccessor();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            ////后台管理员cookie服务
            //.AddCookie(AdminAuthorizeAttribute.AdminAuthenticationScheme, options =>
            //{
            //    options.LoginPath = "/admin/Login/Index";//登录路径
            //    options.LogoutPath = "/admin/Login/LogOff";//退出路径
            //    options.AccessDeniedPath = new PathString("/Error/Forbidden");//拒绝访问页面
            //    options.Cookie.Path = "/";
            //})

            //授权支持，并添加使用Cookie的方式，配置登录页面和没有权限时的跳转页面
            //添加认证Cookie信息 
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            // {
            //     options.LoginPath = new PathString("/login");
            //     options.AccessDeniedPath = new PathString("/login");
            // });


            //读取配置文件
            services.AddOptions();
            services.Configure<DefaultOptions>(Configuration.GetSection("DefaultOptions"));

            //全局过滤
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<Models.AuthorizationAttribute>();
            //});

            // Add framework services.
            services.AddMvc(o=> 
            {
                o.Filters.Add(typeof(GlobalExceptions));
            })
                //全局配置Json序列化处理
                .AddJsonOptions(options =>
                {
                    //忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    //不使用驼峰样式的key
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                }
                );

            services.AddAutoMapper();

            services.AddSingleton<IExcelWriter, EPPlusExcelWriter>();
            services.AddExcelFilePersistAndReadServices();
            services.AddSingleton<IExcelUploader, ExcelUploader>();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseSession();

            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            SiteConfig.SetAppSetting(Configuration.GetSection("DefaultOptions"));

            //页面重定向
        //    var rewrite = new RewriteOptions()
        //.Add(new MoviesRedirectRule(
        //    matchPaths: new string[] { "/films", "/features", "/albums" },
        //    newPath: ""));


            //app.UseRewriter(rewrite);

            //app.Run(async (context) =>
            //{
            //    var path = context.Request.Path;
            //    var query = context.Request.QueryString;
            //    await context.Response.WriteAsync($"New URL: {path}{query}");
            //});
            //用户登录
            app.UseAuthentication();
            app.UseStaticHttpContext();
            app.UseStaticFiles();
           
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "areas",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Admin}/{action=Index}/{id?}");
            });
           

        }
    }
}
