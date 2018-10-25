using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using XGTech.Model;
using XGTech.Model.Model;
using XGTech.Model.Model.ViewModel;
using XGTech.Model.RequestModel;
using XGTech.Utility.Exceptions;
using XGTech.Web.Database;

namespace XGTech.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetUserList(UserRequestModel model)
        {
            var db = DBHelper.GetInstance();
            var pageIndex = model.page;
            var pageSize = model.limit;
            var list = db.Queryable<View_base_user>();
            if (!string.IsNullOrWhiteSpace(model.keyword))
                list = list.Where(p => p.real_name.Contains(model.keyword) || p.emp_no.Contains(model.keyword));
            var totalCount = 0;
            List<View_base_user> listR = list.OrderBy(it => it.user_id, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount);
            var Resoult = new LayUIBaseJsonResoult()
            {
                data = listR,
                count = totalCount,
                code = "0",
                msg = "获取成功"
            };
            return Json(Resoult);
        }
        /// <summary>
        /// 增加页面
        /// </summary>
        /// <returns></returns>
        public IActionResult AddUser()
        {

            return View();
        }
        /// <summary>
        /// 添加用户方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult InsertUser(InsertUserRequestModel model)
        {
            string emp_no = model.emp_no;
            string real_name = model.real_name;
            string telphone = model.telphone;
            string user_status = model.user_status;
            var db = DBHelper.GetInstance();
            wz_tbl_base_users user = db.Ado.SqlQuery<wz_tbl_base_users>("select * from wz_tbl_base_users where emp_no=@emp_no", new SugarParameter[]{
                               new SugarParameter("@emp_no",emp_no)
                       }).FirstOrDefault();

            if (user == null)
            {
                int i = db.Ado.ExecuteCommand(" insert into wz_tbl_base_users (emp_no,real_name,telphone,user_status,user_pwd,create_user_id) values (@emp_no,@real_name,@telphone,@user_status,@user_pwd,@create_user_id); ", new SugarParameter[] {
                 new SugarParameter("@emp_no",emp_no),
                 new SugarParameter("@real_name",real_name),
                 new SugarParameter("@telphone",telphone),
                 new SugarParameter("@user_status",user_status),
                 new SugarParameter("@user_pwd",GetMd5_32byte("sto"+emp_no)),
                 new SugarParameter("@create_user_id",userInfo.user_id),
            });
                if (i > 0)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "1",
                        msg = "保存成功"
                    };
                    return Json(Resoult);
                }
                else
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "保存失败"
                    };
                    return Json(Resoult);
                }
            }
            else
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "该员工工号已存在"
                };
                return Json(Resoult);
            }
        }
        /// <summary>
        /// 更新用户方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult UpdateUser(UpdateUserRequestModel model)
        {
            string user_id = model.user_id;
            string emp_no = model.emp_no;
            string real_name = model.real_name;
            string telphone = model.telphone;
            string user_status = model.user_status;
            var db = DBHelper.GetInstance();
            int i = db.Ado.ExecuteCommand("update [dbo].[wz_tbl_base_users] set emp_no=@emp_no,real_name=@real_name,telphone=@telphone,user_status=@user_status where user_id=@user_id ", new SugarParameter[] {
                 new SugarParameter("@emp_no",emp_no),
                 new SugarParameter("@real_name",real_name),
                 new SugarParameter("@telphone",telphone),
                 new SugarParameter("@user_status",user_status),
                 new SugarParameter("@user_id",user_id)
            });
            if (i > 0)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "1",
                    msg = "保存成功"
                };
                return Json(Resoult);
            }
            else
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "保存失败"
                };
                return Json(Resoult);
            }
        }
        /// <summary>
        /// 修改页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditUser(long id)
        {
            var db = DBHelper.GetInstance();
            wz_tbl_base_users user = db.Ado.SqlQuery<wz_tbl_base_users>("select * from wz_tbl_base_users where user_id=@user_id", new SugarParameter[]{
                               new SugarParameter("@user_id",id)
                       }).FirstOrDefault();
            if (user != null)
            {
                ViewData["emp_no"] = user.emp_no;
                ViewData["real_name"] = user.real_name;
                ViewData["telphone"] = user.telphone;
                ViewData["user_status"] = user.user_status;
            }
            else
            {
                ViewData["user_status"] = "-1";
            }
            return View();
        }

        /// <summary>
        /// 修改密码页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EditPassWord(long id)
        {

            return View();
        }

        public IActionResult UpdatePassWord(UpdatePassWordRequestModel model)
        {
            string user_id = model.user_id;
            string pwd = model.pwd;
            string pwd1 = model.pwd1;
            if (pwd == pwd1)
            {
                var db = DBHelper.GetInstance();
                int i = db.Ado.ExecuteCommand("update wz_tbl_base_users set user_pwd=@user_pwd where user_id=@user_id ", new SugarParameter[] {
                    new SugarParameter("@user_pwd",GetMd5_32byte(pwd)),
                    new SugarParameter("@user_id",user_id)
                    });
                if (i > 0)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "1",
                        msg = "保存成功"
                    };
                    return Json(Resoult);
                }
                else
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "保存失败"
                    };
                    return Json(Resoult);
                }
            }
            else
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "二次密码输入不相符"
                };
                return Json(Resoult);
            }

        }
        public ActionResult RoleManager()
        {
            //获取权限组信息
            var db = DBHelper.GetInstance();

            var rolelist = db.Queryable<wz_tbl_base_role>().Where(s1 => s1.role_status == true && s1.del_flag == true)
                   .Select(s1 => new RoleInfo() { RoleId = s1.role_id, RoleName = s1.role_name }).ToList();

            ViewData["rolelist"] = rolelist;

            return View();
        }

        public ActionResult RoleList(RoleSearchModel model)
        {
            //获取权限组信息
            var db = DBHelper.GetInstance();

            var body = db.Queryable<wz_tbl_base_role, wz_tbl_base_role_user, wz_tbl_base_users>((s1, s2, s3) => new object[] { JoinType.Left, s1.role_id == s2.role_id, JoinType.Left, s2.user_id == s3.user_id }).Where((s1, s2, s3) => s1.role_status == true && s1.del_flag == true);
                
            if (model != null && !String.IsNullOrEmpty(model.Keyword))
            {
                body = body.Where((s1, s2, s3) => s3.emp_no == model.Keyword);
            }


            var rolelist = body.Select(s1 => new RoleInfo() { RoleId = s1.role_id, RoleName = s1.role_name }).ToList();

            //var rolelist = body.Where((s1, s2, s3) => s1.role_status == true && s1.del_flag == true)
            //    .Select(s1 => new RoleInfo() { RoleId = s1.role_id, RoleName = s1.role_name }).ToList();

            //var rolelist = db.Queryable<wz_tbl_base_role>().Where(s1 => s1.role_status == true && s1.del_flag == true)
            //        .Select(s1 => new RoleInfo() { RoleId = s1.role_id, RoleName = s1.role_name }).ToList();

            return Json(new PageList() { data = rolelist });
        }


        public ActionResult GetRoleMenuTreeGrid(long? roleId)
        {
            //获取菜单信息
            var menulist = GetRoleMenuTreeGridTable(roleId);
            return Json(menulist);
        }


        [HttpPost]
        public ActionResult SaveRoleFun(List<MenuFun> funlist, long roleid)
        {
            var db = DBHelper.GetInstance();

            db.Ado.BeginTran();
            //使用删除重新添加的方式
            db.Deleteable<wz_tbl_base_role_menu>().Where(w => w.role_id == roleid).ExecuteCommand();

            var rmlist = funlist.Where(w => w.isLook).Select(s => s.Menu_Id).Distinct().ToList();
            var menulist = db.Queryable<wz_tbl_base_menus>().Where(w => rmlist.Contains(w.menu_id)).ToList();
            List<wz_tbl_base_role_menu> list = new List<wz_tbl_base_role_menu>();
            foreach (var menuid in rmlist)
            {
                wz_tbl_base_role_menu menu = new wz_tbl_base_role_menu();
                menu.menu_id = menuid;
                var menuinfo = menulist.SingleOrDefault(s => s.menu_id == menuid);
                if (menuinfo != null)
                {
                    menu.menu_group_id = menuinfo.menu_group_id.HasValue ? 0 : menuinfo.menu_group_id.Value;
                }
                menu.role_id = roleid;
                menu.net_no = 0;//LoginUser.Current().net_no;
                menu.create_user_id = 0;//LoginUser.Current().user_id;
                menu.create_time = System.DateTime.Now;
                menu.del_flag = true;//有效

                list.Add(menu);
            }

            if (list.Count > 0)
            {
                db.Insertable(list).ExecuteCommand();
            }


            db.Deleteable<wz_tbl_base_role_fun>().Where(w => w.role_id == roleid).ExecuteCommand();
            List<wz_tbl_base_role_fun> rolefunlist = new List<wz_tbl_base_role_fun>();
            var onlyfunlist = funlist.Where(w => !w.isLook).ToList();
            foreach (var fun in onlyfunlist)
            {
                wz_tbl_base_role_fun rolefun = new wz_tbl_base_role_fun();
                rolefun.fun_id = fun.Fun_Id;
                rolefun.role_id = roleid;
                rolefun.create_user_id = 0; //LoginUser.Current().user_id;
                rolefun.create_time = System.DateTime.Now;

                rolefunlist.Add(rolefun);
            }

            if (rolefunlist.Count > 0)
            {
                db.Insertable(rolefunlist).ExecuteCommand();
            }


            //db.Delete<wz_tbl_base_role_menu>(w => w.role_id == roleId);//删除对应菜单
            //db.Delete<wz_tbl_base_role, long>(roleId);//注意主键必需为实体类的第一个属性

            //db.Update<wz_tbl_base_role>("del_flag=0", w => w.role_id == roleId);


            db.Ado.CommitTran();


            return Json(new ComResult() { State = 1, Msg = "保存成功" });
        }


        [HttpPost]
        public ActionResult SaveRole(string roleName, long? roleId)
        {
            return Json(SaveRoleOper(roleName, roleId));
        }


        [HttpPost]
        public ActionResult DeleteRole(long roleId)
        {
            if (roleId==1)
            {
                return Json(new ComResult() { State = 0, Msg = "管理员不能删除!" });
            }

            return Json(DeleteRoleOper(roleId));
        }

        public ActionResult RoleUser(long roleid)
        {
            ViewData["roleid"] = roleid;
            return View();
        }

        [HttpPost]
        public ActionResult RoleUserList(RoleUserQueryModel model)
        {
            //model.net_no = 123;
            return Json(GetRoleUserListOper(model));
        }


        public ActionResult DeleteRoleUser(SaveRoleUser model)
        {
            return Json(DeleteRoleUserOper(model));
        }



        public ActionResult NoRoleUser(long? roleid)
        {
            ViewData["roleid"] = roleid;
            return View();
        }

        [HttpPost]
        public ActionResult NoRoleUserList(QueryDTO model)
        {
            //model.net_no = LoginUser.Current().net_no;
            return Json(GetNoRoleUserListOper(model));
        }


        public ActionResult SaveRoleUser(SaveRoleUser model)
        {
            return Json(SaveRoleUserOper(model));
        }


        #region 数据库操作
        /// <summary>
        /// 往角色添加员工
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ComResult SaveRoleUserOper(SaveRoleUser model)
        {
            using (var db = DBHelper.GetInstance())
            {
                try
                {
                    if (model.userids != null && model.userids.Count > 0)
                    {
                        db.Ado.BeginTran();

                        var roleuserlist = db.Queryable<wz_tbl_base_role_user>().Where(s1 => model.userids.Contains(s1.user_id) && !s1.del_flag).ToList();

                        foreach (var userid in model.userids)
                        {
                            var roleuser = roleuserlist.SingleOrDefault(s => s.user_id == userid);
                            if (roleuser == null)
                            {
                                wz_tbl_base_role_user newroleuser = new wz_tbl_base_role_user();
                                newroleuser.role_id = model.roleid;
                                newroleuser.user_id = userid;
                                newroleuser.net_no = 0; //LoginUser.Current().net_no;
                                newroleuser.create_user_id = 0;//LoginUser.Current().user_id;
                                newroleuser.create_time = System.DateTime.Now;
                                newroleuser.del_flag = true;
                                db.Insertable(newroleuser).ExecuteCommand();
                            }
                            else
                            {
                                roleuser.del_flag = true;
                                roleuser.del_time = null;
                                roleuser.del_user_id = 0;
                                db.Updateable(roleuser).ExecuteCommand();
                            }
                        }
                        //提交事务
                        db.Ado.CommitTran();
                    }
                    return new ComResult() { State = 1, Msg = "操作成功" };
                }
                catch (Exception e)
                {
                    db.Ado.RollbackTran();//回滚
                    return new ComResult() { State = 0, Msg = e.Message };
                }
            }
        }

        /// <summary>
        /// 未分配权限的员工
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PageList GetNoRoleUserListOper(QueryDTO model)
        {
            using (var db = DBHelper.GetInstance())
            {
                //获取有权限的员工

                var roleuserlist = db.Queryable<wz_tbl_base_role_user>().ToList();

                var rolelist = new List<long>();
                roleuserlist.ForEach(a =>
                {
                    rolelist.Add(a.user_id);
                });

                PageList result = new PageList();
                //var sql = db.Queryable<wz_tbl_base_users>()
                //        .Where(s1 => s1.del_flag == true && s1.net_no == model.net_no && !rolelist.Contains(s1.user_id));

                var sql = db.Queryable<wz_tbl_base_users>()
                        .Where(s1 => s1.del_flag == true && !rolelist.Contains(s1.user_id) && s1.user_status==1);

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    sql.Where(s1 => s1.real_name.Contains(model.Keyword));
                }
                if (model.limit > 0)
                {
                    var total = sql.Count();
                    result.count = total;
                    var list = sql.Select<User>(s1 => new User()
                    {
                        user_id = s1.user_id,
                        real_name = s1.real_name,
                        emp_no = s1.emp_no

                    }).OrderBy(s1 => s1.user_id).Skip(model.limit * (model.page - 1))
                             .Take(model.limit).ToList();
                    result.data = list;
                    var pagecout = total % model.limit == 0 ? total / model.limit : (total / model.limit) + 1;
                    result.pageCount = pagecout;

                    result.pageIndex = model.page;
                    result.pageSize = model.limit;
                }
                else
                {
                    var total = sql.Count();
                    result.count = total;
                    result.data = sql.ToList();
                }

                return result;
            }
        }


        public ComResult DeleteRoleUserOper(SaveRoleUser model)
        {
            using (var db = DBHelper.GetInstance())
            {
                try
                {
                    if (model.userids != null && model.userids.Count > 0)
                    {
                        db.Ado.BeginTran();
                        //改成物理删除
                        db.Deleteable<wz_tbl_base_role_user>().Where(w => w.role_id == model.roleid && model.userids.Contains(w.user_id)).ExecuteCommand();
                        //提交事务
                        db.Ado.CommitTran();
                    }
                    return new ComResult() { State = 1, Msg = "操作成功" };
                }
                catch (Exception e)
                {
                    db.Ado.RollbackTran();//回滚
                    return new ComResult() { State = 0, Msg = e.Message };
                }
            }
        }


        public PageList GetRoleUserListOper(RoleUserQueryModel model)
        {
            using (var db = DBHelper.GetInstance())
            {
                PageList result = new PageList();
                //var sql = db.Queryable<wz_tbl_base_role_user, wz_tbl_base_users>((s1, s2) => s1.user_id == s2.user_id)
                //        //.JoinTable<wz_tbl_base_users>((s1, s2) => s1.user_id == s2.user_id)
                //        .Where((s1, s2) => s1.role_id == model.RoleId && s1.del_flag == true && s2.net_no == model.net_no);


                var sql = db.Queryable<wz_tbl_base_role_user, wz_tbl_base_users>((s1, s2) => s1.user_id == s2.user_id)
                       //.JoinTable<wz_tbl_base_users>((s1, s2) => s1.user_id == s2.user_id)
                       .Where((s1, s2) => s1.role_id == model.RoleId && s1.del_flag == true);

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    sql = sql.Where((s1, s2) => s2.real_name.Contains(model.Keyword));
                }
                if (model.limit > 0)
                {
                    var total = 0;

                    var list = sql.OrderBy(s1 => s1.user_id).Select((s1, s2) => new User()
                    {
                        user_id = s2.user_id,
                        real_name = s2.real_name,
                        emp_no = s2.emp_no

                    }).ToPageList(model.page, model.limit, ref total);

                    result.data = list;
                    result.count = total;
                    var pagecout = total % model.limit == 0 ? total / model.limit : (total / model.limit) + 1;
                    result.pageCount = pagecout;

                    result.pageIndex = model.page;
                    result.pageSize = model.limit;
                }
                else
                {
                    var total = sql.Count();
                    result.count = total;
                    result.data = sql.ToList();
                }

                return result;
            }
        }


        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ComResult DeleteRoleOper(long roleId)
        {
            try
            {
                using (var db = DBHelper.GetInstance())
                {
                    if (db.Queryable<wz_tbl_base_role_user>().Any(a => a.role_id == roleId))//分配了员工不能删除
                    {
                        throw new CustomException("该角色已分配了员工");
                    }

                    db.Deleteable<wz_tbl_base_role_menu>().Where(w => w.role_id == roleId).ExecuteCommand();//删除对应菜单
                                                                                                            //db.Delete<wz_tbl_base_role, long>(roleId);//注意主键必需为实体类的第一个属性

                    db.Updateable<wz_tbl_base_role>().UpdateColumns(w => new wz_tbl_base_role() { del_flag = false, del_time = DateTime.Now, del_user_id = 0 }).Where(w => w.role_id == roleId).ExecuteCommand();
                    return new ComResult() { State = 1, Msg = "删除成功" };
                }
            }
            catch (Exception e)
            {
                return new ComResult() { State = 0, Msg = e.Message };
            }
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ComResult SaveRoleOper(string roleName, long? roleId)
        {
            try
            {
                var db = DBHelper.GetInstance();

                var any = db.Queryable<wz_tbl_base_role>().Where(_a => _a.role_name.Equals(roleName, StringComparison.OrdinalIgnoreCase));

                if (roleId.HasValue)
                {
                    any = any.Where(a => a.role_id != roleId.Value);
                }

                if (any.Any(a=>a.del_flag==true))
                {
                    throw new CustomException("该角色名已被使用");
                }

                if (roleId.HasValue)
                {
                    var oldrole = db.Queryable<wz_tbl_base_role>().First(a => a.role_id == roleId.Value);
                    if (oldrole != null)
                    {
                        oldrole.edit_user_id = 0; //LoginUser.Current().user_id;
                        oldrole.create_time = System.DateTime.Now;

                        db.Updateable<wz_tbl_base_role>().UpdateColumns(w => new wz_tbl_base_role() { role_name = roleName }).Where(w => w.role_id == roleId.Value).ExecuteCommand();
                    }
                    else
                    {
                        throw new CustomException("该条数据已被删除");
                    }
                }
                else
                {
                    wz_tbl_base_role newrole = new wz_tbl_base_role();
                    newrole.role_name = roleName;
                    newrole.role_status = true;
                    newrole.del_flag = true;
                    newrole.net_no = 0; //LoginUser.Current().net_no;
                    newrole.create_user_id = 0;//LoginUser.Current().user_id;
                    newrole.create_time = DateTime.Now;
                    db.Insertable(newrole).ExecuteCommand();
                }

                return new ComResult() { State = 1, Msg = "操作成功" };
            }

            catch (Exception e)
            {
                return new ComResult() { State = 0, Msg = e.Message };
            }
        }



        /// <summary>
        /// 权限树形表格
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<TreeGridDTO> GetRoleMenuTreeGridTable(long? roleId)
        {
            using (var db = DBHelper.GetInstance())
            {
                var menugrouplist = db.Queryable<wz_tbl_base_menu_groups>()
                        .Select(s1 => new MenuGroupInfo() { menu_group_id = s1.menu_group_id, menu_group_name = s1.group_name }).ToList();

                List<TreeGridDTO> treelist = new List<TreeGridDTO>();
                if (roleId.HasValue)
                {
                    string funsql = "select a.fun_id,menu_id,fun_name,(CASE WHEN b.fun_id is null then 0 else 1 end) as ischecked from wz_tbl_base_menus_fun a left join wz_tbl_base_role_fun b on a.fun_id=b.fun_id and b.role_id=" + roleId.Value + "";

                    var funlist = db.Ado.SqlQuery<FunDTO>(funsql);

                    string sql = "select a.menu_group_id,a.menu_id,a.menu_name,b.role_id from wz_tbl_base_menus a left join wz_tbl_base_role_menu b on a.menu_id=b.menu_id and b.role_id=" + roleId.Value + " ";

                    var rolemenulist = db.Ado.SqlQuery<RoleMenu>(sql);
                    foreach (var menugroup in menugrouplist)
                    {
                        TreeGridDTO tree = new TreeGridDTO();
                        tree.id = menugroup.menu_group_id.Value;
                        tree.name = menugroup.menu_group_name;
                        tree.pid = 0;
                        tree.isAllselect = true;
                        treelist.Add(tree);

                        var children = rolemenulist.Where(w => w.menu_group_id == menugroup.menu_group_id.Value).Select(s => new { id = s.menu_id.Value, name = s.menu_name, ischecked = s.role_id.HasValue ? true : false }).ToList();
                        foreach (var child in children)
                        {
                            TreeGridDTO childtree = new TreeGridDTO();
                            childtree.id = child.id;
                            childtree.name = child.name;
                            childtree.pid = tree.id;
                            childtree.funs = funlist.Where(w => w.menu_id == child.id).ToList();
                            childtree.funs.Insert(0, new FunDTO { fun_name = "查看", fun_id = 0, isLook = true, ischecked = child.ischecked });
                            childtree.isAllselect = childtree.funs.Where(w => w.ischecked).Count() == childtree.funs.Count();
                            treelist.Add(childtree);

                            if (!childtree.isAllselect)
                            {
                                tree.isAllselect = false;
                            }
                        }

                    }
                    return treelist;
                }
                else
                {
                    var funlist = db.Queryable<wz_tbl_base_menus_fun>()
                        .Select(s1 => new FunDTO() { fun_id = s1.fun_id, menu_id = s1.menu_id, fun_name = s1.fun_name }).ToList();

                    var rolemenulist = db.Queryable<wz_tbl_base_menus>()
                        .Select(s1 => new RoleMenu() { menu_group_id = s1.menu_group_id, menu_id = s1.menu_id, menu_name = s1.menu_name }).ToList();

                    foreach (var menugroup in menugrouplist)
                    {
                        TreeGridDTO tree = new TreeGridDTO();
                        tree.id = menugroup.menu_group_id.Value;
                        tree.name = menugroup.menu_group_name;
                        tree.pid = 0;
                        tree.isAllselect = true;
                        treelist.Add(tree);

                        var children = rolemenulist.Where(w => w.menu_group_id == menugroup.menu_group_id.Value).Select(s => new TreeDTO { id = s.menu_id.Value, name = s.menu_name }).ToList();
                        foreach (var child in children)
                        {
                            TreeGridDTO childtree = new TreeGridDTO();
                            childtree.id = child.id;
                            childtree.name = child.name;
                            childtree.pid = tree.id;
                            childtree.funs = funlist.Where(w => w.menu_id == child.id).ToList();
                            childtree.funs.Insert(0, new FunDTO { fun_name = "查看", fun_id = 0, isLook = true, ischecked = false });
                            childtree.isAllselect = childtree.funs.Where(w => w.ischecked).Count() == childtree.funs.Count();
                            treelist.Add(childtree);

                            if (!childtree.isAllselect)
                            {
                                tree.isAllselect = false;
                            }
                        }
                    }
                    return treelist;
                }
            }
        }
        #endregion


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