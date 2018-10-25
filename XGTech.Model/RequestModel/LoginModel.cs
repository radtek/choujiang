using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class UserLoginRequestModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }
    }


    public class ForgetPassRequestModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 新登录密码
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        public string cellcode { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string cell { get; set; }
    }
}
