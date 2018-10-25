using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XGTech.BLL;
using XGTech.IBLL;
using XGTech.Web.Database;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures
{
    public static class HtmlHelperEx
    {
        public static bool HasFun(string funCode)
        {
            long userid = 1;

            bool hasFun = false;

            var db = DBHelper.GetInstance();

            String sql = "select count(0) from wz_tbl_base_role_user u join wz_tbl_base_role_fun a on u.role_id=a.role_id join wz_tbl_base_menus_fun b on a.fun_id=b.fun_id where u.user_id="+ userid + " and b.fun_code='"+ funCode + "'";

            object obj=db.Ado.GetScalar(sql);

            if (Convert.ToInt32(obj) > 0)
            {
                hasFun = true;
            }

            return hasFun;
            
            //return true;
            //IRoleService service = new RoleService();
            //bool hasFun = false;

            //var userid = LoginUser.Current().user_id;
            //try
            //{
            //    hasFun = service.HasFun(funCode, userid);
            //}
            //catch { }
            //return hasFun;
        }
    }

}
