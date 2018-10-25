using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class ActivityManageListRequestModel
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
        /// 活动标题
        /// </summary>
        public string title { get; set; }

        public int? AType { get; set; }

        public int? Status { get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        public int datetype { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
    }
}
