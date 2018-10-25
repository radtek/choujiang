using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class DataTotalRequestModel
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
        public string Title { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }
    }
}
