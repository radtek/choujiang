using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XGTech.Model.ResponseModel
{
    public class ActivityManageListResponseModel
    {
        [DisplayName("创建日期")]
        public DateTime AddTime { get; set; }
        [DisplayName("活动编号")]
        public string Number { get; set; }
        [DisplayName("活动标题")]
        public string Title { get; set; }
        [DisplayName("活动类型")]
        public string AType { get; set; }
        [DisplayName("开始时间")]
        public string StartTime { get; set; }
        [DisplayName("结束时间")]
        public string EntTime { get; set; }
        [DisplayName("活动状态")]
        public string Status { get; set; }
        

    }
}
