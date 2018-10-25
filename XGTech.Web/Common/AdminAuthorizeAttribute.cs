using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XGTech.Web.Common
{
    public class AdminAuthorizeAttribute: Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            //匿名标识 无需验证
            //if (filterContext.Filters.Any(e => (e as AllowAnonymous) != null))
            //    return;
            //var adminInfo = GlobalContext.AdminInfo;//此处应为获取的登录用户
            //if (adminInfo == null)
            //{
            //    if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            //    {
            //        filterContext.Result = new JsonResult("未登录");
            //    }
            //    else
            //    {
            //        filterContext.Result = new ContentResult() { Content = "未登录" };
            //    }
            //    return;
            //}
            ////对应action方法或者Controller上若存在NonePermissionAttribute标识，即表示为管理员的默认权限,只要登录就有权限
            //var isNone = filterContext.Filters.Any(e => (e as NonePermissionAttribute) != null);
            //if (isNone)
            //    return;

            ////获取请求的区域，控制器，action名称
            //var area = filterContext.RouteData.DataTokens["area"]?.ToString();
            //var controller = filterContext.RouteData.Values["controller"]?.ToString();
            //var action = filterContext.RouteData.Values["action"]?.ToString();
            //var isPermit = false;
            ////校验权限
            //isPermit = ServiceFactory.CheckAdminPermit(adminInfo.Id, area, controller, action);
            //if (isPermit)
            //    return;
            ////此action方法的父辈权限判断，只要有此action对应的父辈权限，皆有权限访问
            //var pAttrs = filterContext.Filters.Where(e => (e as ParentPermissionAttribute) != null).ToList();
            //if (pAttrs.Count > 0)
            //{
            //    foreach (ParentPermissionAttribute pattr in pAttrs)
            //    {
            //        if (!string.IsNullOrEmpty(pattr.Area))
            //            area = pattr.Area;
            //        isPermit = ServiceFactory.CheckAdminPermit(adminInfo.Id, area, pattr.Controller, pattr.Action);
            //        if (isPermit)
            //            return;
            //    }
            //}
            //if (!isPermit)
            //{
            //    filterContext.Result = new ContentResult() { Content = "无权限访问" };
            //    return;
            //}
        }

    }
}
