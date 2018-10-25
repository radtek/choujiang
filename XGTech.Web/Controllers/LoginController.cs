using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlSugar;
using XGTech.Model;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Utility;
using XGTech.Web.Database;

namespace XGTech.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index(int id)
        {
            //todo:
            //判断该活动状态是否启用
            //判断该活动是否在指定有效期内
            var db = DBHelper.GetInstance();
            Tbl_Activity activityModel = db.Queryable<Tbl_Activity>().Where(t1 => t1.ID == id).First();
            if (activityModel == null)
            {
                return Content("非法请求!");
            }
            string userid = GetCookies("login" + id);
            if (userid != "")
            {
                Response.Redirect("/Default/index?id=" + userid);
            }
            return View();
        }

        public IActionResult CheckLogin(CheckLoginRequestModel model)
        {
            string id = string.Empty;
            id = GetCookies("login" + model.activityid);
            if (id == "")
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "未登录"
                };
                return Json(Resoult);
            }
            else
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    data = id,
                    code = "1",
                    msg = "已登陆"
                };
                return Json(Resoult);
            }
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
            DataTable dt = db.Ado.GetDataTable("select id from  Tbl_cellcode where cell='" + cell + "' and type=0 and DATEADD(minute,1,createtime) >GETDATE() ");
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
            DataTable dt = db.Ado.GetDataTable("select id from Tbl_cellcode where cell='" + cell + "'  and type=0 and cellcode='" + cellcode + "'  ");
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
            DataTable dt = db.Ado.GetDataTable("select id from Tbl_cellcode where cell='" + cell + "' and type=0 and cellcode='" + cellcode + "' and DATEADD(second,90,createtime) >GETDATE() ");
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region 检测用户数据库中是否存在
        /// <summary>
        ///     检测用户数据库中是否存在
        /// </summary>
        /// <param name="iphone"></param>
        /// <returns></returns>
        public int CheckGetUserID(string cell, string activityid)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            DataTable dt = db.Ado.GetDataTable("select id from Tbl_user where cell='" + cell + "' and activityid='" + activityid + "'");
            int id = 0;
            foreach (DataRow item in dt.Rows)
            {
                id = int.Parse(item["id"].ToString());
            }
            return id;
        }
        #endregion

        #region 验证码是否发送成功
        public bool CheckSendMsg(string num, string cell)
        {
            SendMessageModel data = new SendMessageModel();
            data.PartnerId = 1;
            data.Mobile = Convert.ToInt64(cell);
            data.Content = string.Format("[抽奖]你的验证码是：{0}，90秒内有效。发送时间：{1}", num, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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

        #region 发送验证码
        public IActionResult SendMessage(SendMessageRequestModel model)
        {
            try
            {
                SqlSugarClient db = DBHelper.GetInstance();
                string cell = model.cell;
                string cellcode = CreateNum();
                if (IsIphone(cell))
                {
                    if (CheckSend(cell))
                    {
                        if (CheckSendMsg(cellcode, cell))
                        {
                            db.Ado.ExecuteCommand(string.Format("insert into Tbl_cellcode (cell,cellcode,status,type,ip,usetime,createtime) values ('{0}','{1}',0,0,'{2}',getdate(),getdate())", cell, cellcode, Request.Host.Host));
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

        #region 登陆
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult LoginIn(SendMessageRequestModel model)
        {
            string logintime = GetCookies("logintime");
            string count = GetCookies("count");
            if (logintime == "" && count == "")
            {
                SetCookies("logintime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 1);
                SetCookies("count", "0", 1);
            }
            else
            {
                DateTime time1 = DateTime.Parse(logintime);
                DateTime time2 = DateTime.Now;
                TimeSpan timeSpan = time2 - time1;
                if (timeSpan.TotalSeconds >= 60)
                {
                    SetCookies("logintime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 1);
                    SetCookies("count", "0", 1);
                }
                else
                {
                    count = (int.Parse(count) + 1).ToString();
                    SetCookies("count", count, 1);
                    if (int.Parse(count) > 4)
                    {
                        var Resoult = new LayUIBaseJsonResoult()
                        {
                            code = "0",
                            msg = "登陆失败"
                        };
                        return Json(Resoult);
                    }
                }
            }

            SqlSugarClient db = DBHelper.GetInstance();
            string cell = model.cell;
            string cellcode = model.cellcode;
            if (IsIphone(cell))
            {
                if (CheckCellCode(cell, cellcode)) //验证验证码是否错误
                {
                    if (CheckCellCodeCao(cell, cellcode)) //验证验证码是否超时
                    {
                        int userid = CheckGetUserID(cell, model.activityid);
                        if (userid > 0)
                        {
                            SetCookies("login" + model.activityid, userid.ToString(), 30);
                        }
                        else
                        {
                            int ID = db.Ado.SqlQuerySingle<int>(" insert into Tbl_user (activityid,cell,type,status,ip,createtime) " +
                             "values (@activityid,@cell ,0,0,@ip,getdate()); select @@identity as ID; ", new SugarParameter[]{
                               new SugarParameter("@activityid",model.activityid),
                               new SugarParameter("@cell",cell),
                               new SugarParameter("@ip",Request.Host.Host)
                               });
                            SetCookies("login" + model.activityid, ID.ToString(), 30);
                        }
                        var Resoult = new LayUIBaseJsonResoult()
                        {
                            code = "1",
                            msg = "登陆成功"
                        };
                        return Json(Resoult);
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
        #endregion

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