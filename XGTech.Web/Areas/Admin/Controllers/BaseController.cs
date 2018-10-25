using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XGTech.Model.Model;
using XGTech.Model.ResponseModel;
using XGTech.Utility;
using XGTech.Web.Common;

namespace XGTech.Web.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public UserInfo userInfo { get; set; }

        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ifNeedLogin = true;
            //判断是否允许不登录访问
            foreach (var filter in context.ActionDescriptor.FilterDescriptors)
            {
                //如果方法名包含AllowAnonymous标签，则不需要登陆，相应的用户属性不可用
                if (filter.Filter.GetType() == typeof(AllowAnonymousFilter))
                {
                    ifNeedLogin = false;
                }
            }

            if (ifNeedLogin)
            {
//#if DEBUG
//                userInfo = new UserInfo()
//                {
//                    user_id = 1,
//                    user_name = "Admin",
//                    role_list = new List<UserRole>()
//                    {
//                        new UserRole(){ role_id="1",role_name="管理员" }
//                    }
//                };
//#else
                    string SysInfo = CookiesHelper.GetCookies("SysInfo");
                    if (SysInfo == "")
                    {
                        context.Result = new RedirectResult("~/Admin/Login/Index");
                    }
                    else
                    {
                        userInfo = JsonHelper.DeserializeJsonToObject<UserInfo>(CookiesHelper.GetCookies("SysInfo"));
                    }
//#endif

            }

            await base.OnActionExecutionAsync(context, next);
        }


    }

}
