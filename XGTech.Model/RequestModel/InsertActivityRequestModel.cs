using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class InsertActivityRequestModel
    {
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
        /// 优先使用
        /// </summary>
        public int? PriorityUse { get; set; }

        /// <summary>
        /// 奖项集合
        /// </summary>
        public string ATypes { get; set; }
        /// <summary>
        /// 奖品集合ID
        /// </summary>
        public string PNames { get; set; }
        /// <summary>
        /// 数量集合
        /// </summary>
        public string Inventorys { get; set; }
        /// <summary>
        /// 中奖率集合
        /// </summary>
        public string Probabilitys { get; set; }

        public int LotteryNumber { get; set; }

    }

    public class UpdateActivityRequestModel
    {
        public long ActivityID { get; set; }
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
        /// 优先使用
        /// </summary>
        public int? PriorityUse { get; set; }

        /// <summary>
        /// 活动奖品关联ID
        /// </summary>
        public string PrizeID { get; set; }
        /// <summary>
        /// 奖项集合
        /// </summary>
        public string ATypes { get; set; }
        /// <summary>
        /// 奖品集合ID
        /// </summary>
        public string PNames { get; set; }
        /// <summary>
        /// 数量集合
        /// </summary>
        public string Inventorys { get; set; }
        /// <summary>
        /// 中奖率集合
        /// </summary>
        public string Probabilitys { get; set; }

        public int LotteryNumber { get; set; }
    }
}
