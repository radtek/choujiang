using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XGTech.Model.ResponseModel
{
    public class PrizeSettingResponseModel
    {

    }

    
    public class PrizeSettingExportResponseModel
    {
        /// <summary>
        /// Desc:奖品名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        [DisplayName("奖品名称")]
        public string PName { get; set; }

        /// <summary>
        /// Desc:奖品类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        [DisplayName("奖品类型")]
        public String PType { get; set; }

        /// <summary>
        /// Desc:奖品详情 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        [DisplayName("奖品详情")]
        public string PDetails { get; set; }

        /// <summary>
        /// Desc:奖品价值 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        [DisplayName("奖品价值")]
        public decimal? Pworth { get; set; }

        /// <summary>
        /// Desc:奖品规则 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        [DisplayName("奖品规则")]
        public string PRule { get; set; }
    }
}
