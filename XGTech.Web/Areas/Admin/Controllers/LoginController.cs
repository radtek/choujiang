using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar;
using XGTech.Model;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Model.ResponseModel;
using XGTech.Utility;
using XGTech.Web.Common;
using XGTech.Web.Database;

namespace XGTech.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IsShow(UserLoginRequestModel model)
        {
            string count = GetCookies("admincount");
            if (count == "")
            {
                count = "0";
                SetCookies("admincount", "0", 24);
            }
            if (int.Parse(count) > 2)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "1",
                    msg = "显示"
                };
                return Json(Resoult);
            }
            else
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "显示"
                };
                return Json(Resoult);
            }
        }

        [AllowAnonymous]
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <returns></returns>
        public IActionResult UserLogin(UserLoginRequestModel model)
        {
            string count = GetCookies("admincount");
            if (count == "")
            {
                count = "0";
                SetCookies("admincount", "0", 24);
            }
            string codeT = HttpContext.Session.GetString("memberVerifyCode");
            SqlSugarClient db = DBHelper.GetInstance();
            string code = model.code;
            if (int.Parse(count) > 2)
            {
                if (codeT.ToLower() != code.ToLower())
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        data = "code",
                        msg = "验证码输入错误"
                    };
                    return Json(Resoult);
                }
            }
            string username = model.username;
            string password = model.password;
            string psssMD5 = GetMd5_32byte(password);
            wz_tbl_base_users user = db.Ado.SqlQuery<wz_tbl_base_users>("select * from wz_tbl_base_users where emp_no=@emp_no", new SugarParameter[]{
                               new SugarParameter("@emp_no",username)
                       }).FirstOrDefault();
            if (user == null)
            {
                count = (int.Parse(count) + 1).ToString();
                SetCookies("admincount", count, 24);
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    data = "username",
                    msg = "用户名不存在"
                };
                return Json(Resoult);
            }
            else
            {
                if (user.user_status == 2)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        data = "username",
                        msg = "该用户已停用"
                    };
                    return Json(Resoult);
                }
                else
                {
                    wz_tbl_base_users user1 = db.Ado.SqlQuery<wz_tbl_base_users>("select * from wz_tbl_base_users where emp_no=@emp_no and user_pwd=@password", new SugarParameter[]{
                               new SugarParameter("@emp_no",username),
                               new SugarParameter("@password",psssMD5)
                       }).FirstOrDefault();
                    if (user1 == null)
                    {
                        count = (int.Parse(count) + 1).ToString();
                        SetCookies("admincount", count, 24);
                        var Resoult = new LayUIBaseJsonResoult()
                        {
                            code = "0",
                            data = "password",
                            msg = "密码错误"
                        };
                        return Json(Resoult);
                    }
                    else
                    {
                        user1.user_pwd = "";
                        List<UserRole> rolelist = db.Ado.SqlQuery<UserRole>("select b.role_id,b.role_name from wz_tbl_base_role_user a inner join wz_tbl_base_role b  on a.role_id=b.role_id where a.user_id=@userid", new SugarParameter[]{
                               new SugarParameter("@userid",user1.user_id)
                       }).ToList();
                        var info = new UserInfo
                        {
                            emp_no = user1.emp_no,
                            user_id = user1.user_id,
                            user_name = user1.user_name,
                            role_list = rolelist
                        };
                        String js = JsonHelper.SerializeObject(info);
                        CookiesHelper.SetCookies("SysInfo", js, 24);
                        SetCookies("admincount", "0", 24);
                        var Resoult = new LayUIBaseJsonResoult()
                        {
                            code = "1",
                            msg = "登陆成功"
                        };
                        return Json(Resoult);
                    }
                }
            }

        }

        public IActionResult ForgetPass()
        {
            return View();

        }
        #region 随机生成4位数字
        /// <summary>生成4位随机数字</summary>
        /// <returns></returns>
        public string CreateNum()
        {
            var random = new Random();
            string str = null; //循环的次数 
            for (int i = 0; i < 4; i++)
            {
                int number = random.Next(1, 9);
                str += number;
            }
            return str;
        }

        #endregion

        #region 判断手机号码格式
        /// <summary>
        ///     判断手机号码格式
        /// </summary>
        /// <param name="iphone"></param>
        /// <returns></returns>
        public bool IsIphone(string iphone)
        {
            return Regex.IsMatch(iphone, @"^[1]+[3,4,5,7,8]+\d{9}");
        }

        #endregion

        #region 检测1分钟之内是否重新发短信
        /// <summary>
        ///     检测1分钟之内是否重新发短信
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool CheckSend(string cell)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            DataTable dt = db.Ado.GetDataTable("select id from  Tbl_cellcode where cell='" + cell + "' and type=1 and DATEADD(minute,1,createtime) >GETDATE() ");
            if (dt.Rows.Count > 0)
                return false;
            else
                return true;
        }
        #endregion

        #region 短信验证码验证
        /// <summary>
        ///     短信验证码验证
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellcode"></param>
        /// <returns></returns>
        public bool CheckCellCode(string cell, string cellcode)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            DataTable dt = db.Ado.GetDataTable("select id from Tbl_cellcode where cell='" + cell + "' and type=1 and cellcode='" + cellcode + "'  ");
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region 短信验证码超时验证
        /// <summary>
        ///     短信验证码超时验证
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="cellcode"></param>
        /// <returns></returns>
        public bool CheckCellCodeCao(string cell, string cellcode)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            DataTable dt = db.Ado.GetDataTable("select id from Tbl_cellcode where cell='" + cell + "' and type=1 and cellcode='" + cellcode + "' and DATEADD(second,90,createtime) >GETDATE() ");
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region 验证码是否发送成功
        public bool CheckSendMsg(string num, string cell)
        {
            SendMessageModel data = new SendMessageModel();
            data.PartnerId = 1;
            data.Mobile = Convert.ToInt64(cell);
            data.Content = string.Format("[抽奖后台]你的验证码是：{0}，90秒内有效。发送时间：{1}", num, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            data.Sign = MessageHelper.GetMD5(data.Content, "ebd01a98195c4c029d72c09d3b56a9d4");
            string json = JsonConvert.SerializeObject(data);
            string retmessage = MessageHelper.GetResponseData(json, "http://openapi.net-x.cn/OpenBase/VaNKpF/SmsSend");
            if (string.IsNullOrEmpty(retmessage))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 检测用户数据库中是否存在
        /// <summary>
        ///     检测用户数据库中是否存在
        /// </summary>
        /// <param name="iphone"></param>
        /// <returns></returns>
        public bool CheckUser(string username, string cell)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            DataTable dt = db.Ado.GetDataTable("select user_id from wz_tbl_base_users where emp_no='" + username + "' and telphone='" + cell + "'");
            int id = 0;
            foreach (DataRow item in dt.Rows)
            {
                id = int.Parse(item["user_id"].ToString());
            }
            if (id == 0)
                return false;
            else
                return true;
        }
        #endregion

        #region 发送验证码
        public IActionResult SendMessage(SendMessageRequestModel model)
        {
            try
            {
                SqlSugarClient db = DBHelper.GetInstance();
                string cell = model.cell;
                string cellcode = CreateNum();
                if (CheckUser(model.username, model.cell))
                {
                    if (IsIphone(cell))
                    {
                        if (CheckSend(cell))
                        {
                            if (CheckSendMsg(cellcode, cell))
                            {
                                db.Ado.ExecuteCommand(string.Format("insert into Tbl_cellcode (cell,cellcode,status,type,ip,usetime,createtime) values ('{0}','{1}',0,1,'{2}',getdate(),getdate())", cell, cellcode, Request.Host.Host));
                                var Resoult = new LayUIBaseJsonResoult()
                                {
                                    code = "1",
                                    msg = "验证码发送成功"
                                };
                                return Json(Resoult);
                            }
                            else
                            {
                                var Resoult = new LayUIBaseJsonResoult()
                                {
                                    code = "0",
                                    msg = "验证码发送失败"
                                };
                                return Json(Resoult);
                            }
                        }
                        else
                        {
                            var Resoult = new LayUIBaseJsonResoult()
                            {
                                code = "0",
                                msg = "短信验证码已发出,请耐心等待,1分钟内只能发一条"
                            };
                            return Json(Resoult);
                        }
                    }
                    else
                    {
                        var Resoult = new LayUIBaseJsonResoult()
                        {
                            code = "0",
                            msg = "手机格式不正确"
                        };
                        return Json(Resoult);
                    }
                }
                else
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "当前工号和手机号码不匹配"
                    };
                    return Json(Resoult);
                }
            }
            catch (Exception x)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = x.Message.ToString()
                };
                return Json(Resoult);
            }
        }
        #endregion

        /// <summary>
        /// 忘记密码 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult ChangePassWord(ForgetPassRequestModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            string cell = model.cell;
            string cellcode = model.cellcode;
            string username = model.username;
            string password = model.NewPassword;
            string psssMD5 = GetMd5_32byte(password);
            if (CheckUser(model.username, model.cell))
            {
                if (IsIphone(cell))
                {
                    if (CheckCellCode(cell, cellcode)) //验证验证码是否错误
                    {
                        if (CheckCellCodeCao(cell, cellcode)) //验证验证码是否超时
                        {
                            DataTable dt = db.Ado.GetDataTable("select user_id,user_pwd from wz_tbl_base_users where emp_no='" + username + "' and telphone='" + cell + "'");
                            string user_pwd = string.Empty;
                            foreach (DataRow item in dt.Rows)
                            {
                                user_pwd = item["user_pwd"].ToString();
                            }
                            if (user_pwd.ToUpper() == psssMD5.ToUpper())
                            {
                                var Resoult = new LayUIBaseJsonResoult()
                                {
                                    data = "newpassword",
                                    code = "0",
                                    msg = "新密码不允许与旧密码相同"
                                };
                                return Json(Resoult);
                            }
                            else
                            {
                                int ID = db.Ado.ExecuteCommand(" update wz_tbl_base_users set user_pwd=@user_pwd where emp_no=@emp_no ", new SugarParameter[]{
                               new SugarParameter("@user_pwd",psssMD5),
                               new SugarParameter("@emp_no",username)
                             });
                                if (ID > 0)
                                {
                                    var Resoult = new LayUIBaseJsonResoult()
                                    {
                                        code = "1",
                                        msg = "修改成功"
                                    };
                                    return Json(Resoult);
                                }
                                else
                                {
                                    var Resoult = new LayUIBaseJsonResoult()
                                    {
                                        data = "user_name",
                                        code = "0",
                                        msg = "修改密码失败"
                                    };
                                    return Json(Resoult);
                                }
                            }
                        }
                        else
                        {
                            var Resoult = new LayUIBaseJsonResoult()
                            {
                                data = "cellcode",
                                code = "0",
                                msg = "短信验证码已超时,请重新发送"
                            };
                            return Json(Resoult);
                        }
                    }
                    else
                    {
                        var Resoult = new LayUIBaseJsonResoult()
                        {
                            data = "cellcode",
                            code = "0",
                            msg = "请输入正确的验证码"
                        };
                        return Json(Resoult);
                    }
                }
                else
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        data = "cell",
                        code = "0",
                        msg = "手机格式不正确"
                    };
                    return Json(Resoult);
                }
            }
            else
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "当前工号和手机号码不匹配"
                };
                return Json(Resoult);
            }
        }

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

        #region  cookie 操作
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddYears(minutes)
            });
        }
        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }

        #endregion

    }
}