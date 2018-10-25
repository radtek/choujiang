using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class SendMessageRequestModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string cell { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string cellcode { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 活动ID
        /// </summary>
        public string activityid { get; set; }
    }

    public class CheckLoginRequestModel
    {
        /// <summary>
        /// 活动ID
        /// </summary>
        public string activityid { get; set; }
    }
}
