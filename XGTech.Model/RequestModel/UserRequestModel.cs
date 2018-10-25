using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class UserRequestModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string keyword { get; set; }
    }


    public class InsertUserRequestModel
    {
        /// <summary>
        /// 员工工号
        /// </summary>
        public string emp_no { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string real_name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string telphone { get; set; }
        /// <summary>
        /// 员工状态
        /// </summary>
        public string user_status { get; set; }
    }


    public class UpdateUserRequestModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 员工工号
        /// </summary>
        public string emp_no { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string real_name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string telphone { get; set; }
        /// <summary>
        /// 员工状态
        /// </summary>
        public string user_status { get; set; }
    }

    public class UpdatePassWordRequestModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string user_id { get; set; }

        public string pwd { get; set; }

        public string pwd1 { get; set; }
    }
}
