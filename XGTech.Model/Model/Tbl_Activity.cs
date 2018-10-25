using System;
using System.Linq;
using System.Text;

namespace XGTech.Model.Model
{
    public class Tbl_Activity
    {

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// Desc:活动编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Desc:活动标题 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Desc:活动类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 AType { get; set; }

        /// <summary>
        /// Desc:开始日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Desc:结束日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Desc:开始时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Desc:结束时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public TimeSpan EntTime { get; set; }

        /// <summary>
        /// Desc:活动状态:0.未开始、1.进行中、2.已结束 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Desc:活动背景图 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string BGImg { get; set; }

        /// <summary>
        /// Desc:Banner图 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string BannerImg { get; set; }

        /// <summary>
        /// Desc:消费积分 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 ConsumeScore { get; set; }

        /// <summary>
        /// Desc:分销积分 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 DistributionScore { get; set; }

        /// <summary>
        /// Desc:活动规则 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string ARule { get; set; }

        /// <summary>
        /// Desc:添加时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? AddTime { get; set; }


        public int? PriorityUse { get; set; } = 0;


        public int LotteryNumber { get; set; }

    }


    public class View_Activity {
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// Desc:活动编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Desc:活动标题 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Desc:活动类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 AType { get; set; }

        /// <summary>
        /// Desc:开始日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Desc:结束日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Desc:开始时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Desc:结束时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public TimeSpan EntTime { get; set; }

        /// <summary>
        /// Desc:活动状态:0.未开始、1.进行中、2.已结束 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Desc:活动背景图 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string BGImg { get; set; }

        /// <summary>
        /// Desc:Banner图 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string BannerImg { get; set; }

        /// <summary>
        /// Desc:消费积分 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 ConsumeScore { get; set; }

        /// <summary>
        /// Desc:分销积分 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 DistributionScore { get; set; }

        /// <summary>
        /// Desc:活动规则 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string ARule { get; set; }

        /// <summary>
        /// Desc:添加时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? AddTime { get; set; }


        public int? PriorityUse { get; set; } = 0;


        public int LotteryNumber { get; set; }

        /// <summary>
        /// 商品总价值
        /// </summary>
        public string Pworth { get; set; }

    }
}
