using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XGTech.Model.ResponseModel
{
    public class LotteryRecordListResponseModel
    {
        [DisplayName("活动编号")]
        public string Number { get; set; }

        [DisplayName("活动标题")]
        public string Title { get; set; }

        [DisplayName("抽奖日期")]
        public DateTime AddTime { get; set; }

        [DisplayName("用户名称")]
        public string UserName { get; set; }

        [DisplayName("奖品名称")]
        public string PName { get; set; }

        [DisplayName("奖品价值")]
        public decimal Pworth { get; set; }

    }


    public class LotteryTotalListResponseModel
    {
        /// <summary>
        /// 活动编号
        /// </summary>
        [DisplayName("活动编号")]
        public string number { get; set; }
        /// <summary>
        /// 活动标题
        /// </summary>
        [DisplayName("活动标题")]
        public string title { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [DisplayName("用户名")]
        public string username { get; set; }
        /// <summary>
        /// 抽奖次数
        /// </summary>
        [DisplayName("抽奖次数")]
        public int lottery_count { get; set; }
        /// <summary>
        /// 中奖次数
        /// </summary>
        [DisplayName("中奖次数")]
        public int win_count { get; set; }
        /// <summary>
        /// 奖品价值
        /// </summary>
        [DisplayName("奖品价值")]
        public decimal totalworth { get; set; }
        
    }
}
