using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class PrizeSettingRequestModel
    {
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// Desc:奖品名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PName { get; set; }

        /// <summary>
        /// Desc:奖品类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64? PType { get; set; }

        /// <summary>
        /// Desc:奖品详情 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PDetails { get; set; }

        /// <summary>
        /// Desc:奖品图片 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PImg { get; set; }

        /// <summary>
        /// Desc:奖品价值 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public decimal? Pworth { get; set; }

        /// <summary>
        /// Desc:奖品规则 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string PRule { get; set; }

        public int Page { get; set; }

        public int Limit { get; set; }

        public String OperName { get; set; }

        public Int64 OperID { get; set; }
    }
}
