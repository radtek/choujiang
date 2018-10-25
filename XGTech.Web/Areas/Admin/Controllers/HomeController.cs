using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using XGTech.Model;
using XGTech.Model.Model;
using XGTech.Model.Model.ViewModel;
using XGTech.Model.RequestModel;
using XGTech.Model.ResponseModel;
using XGTech.Utility;
using XGTech.Web.Common;
using XGTech.Web.Database;

namespace XGTech.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [Area("Admin")]
    public class HomeController : BaseController
    {

        public IActionResult Index()
        {
            ViewData["userName"] = userInfo.emp_no;
            ViewData["nowtime"] = DateTime.Now;

            if (userInfo.role_list.Count<=0)
            {
                ViewBag.MenuList = new List<LeftMenuDTO>();
                return View();
            }

            //获取用户角色
            var db = DBHelper.GetInstance();
            var list = db.Queryable<wz_tbl_base_role_menu, wz_tbl_base_menus, wz_tbl_base_menu_groups>((t1, t2, t3) => new object[]
              {
                JoinType.Left,t1.menu_id==t2.menu_id,
                JoinType.Left,t2.menu_group_id==t3.menu_group_id
              }).Where((t1, t2, t3) => t1.role_id == long.Parse(userInfo.role_list[0].role_id)).OrderBy((t1, t2, t3) => t3.group_sort, OrderByType.Asc)
            .Select((t1, t2, t3) => new LeftMenuDTO { group_name = t3.group_name, menu_name = t2.menu_name, url_absolute_path = t2.url_absolute_path }).ToList();

            ViewBag.MenuList = list;

            return View();
        }

        [AllowAnonymous]
        public IActionResult LoginOut()
        {
            CookiesHelper.DeleteCookies("SysInfo");
            var Resoult = new LayUIBaseJsonResoult()
            {
                code = "1",
                data = new { url = "/Admin/Login/Index" },
                msg = "退出成功"
            };
            return Json(Resoult);

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ModifyPwdView()
        {
            return View();
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ModifyPwd(ModifyPwdRequestModel model)
        {
            var db = DBHelper.GetInstance();

            wz_tbl_base_users user = db.Queryable<wz_tbl_base_users>().Where(t1 => t1.user_id == userInfo.user_id).First();

            //判断旧密码
            if (Common.CommonHelper.GetMd5_32byte(model.oldPwd) != user.user_pwd)
            {
                var Resoult1 = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    data = null,
                    msg = "旧密码不正确"
                };
                return Json(Resoult1);
            }

            //新密码不能和旧密码相同
            if (Common.CommonHelper.GetMd5_32byte(model.affirmPwd) == user.user_pwd)
            {
                var Resoult1 = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    data = null,
                    msg = "新密码不能和旧密码相同"
                };
                return Json(Resoult1);
            }

            //更新密码
            wz_tbl_base_users newUser = new wz_tbl_base_users()
            {
                user_pwd = Common.CommonHelper.GetMd5_32byte(model.affirmPwd),
                user_id = user.user_id
            };

            db.Updateable(newUser).UpdateColumns(t1=>t1.user_pwd).ExecuteCommand();


            CookiesHelper.DeleteCookies("SysInfo");

            var Resoult3 = new LayUIBaseJsonResoult()
            {
                code = "1",
                data = new { url = "/Admin/Login/Index" },
                msg = "修改成功"
            };
            return Json(Resoult3);
        }
    }
}