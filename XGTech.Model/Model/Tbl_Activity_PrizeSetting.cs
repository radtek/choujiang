using System;
using System.Linq;
using System.Text;

namespace XGTech.Model.Model
{
    public class Tbl_Activity_PrizeSetting
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int64 ID {get;set;}

       

        /// <summary>
        /// Desc:活动ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 ActivityID {get;set;}

        /// <summary>
        /// Desc:奖品ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 PrizeID {get;set;}

        /// <summary>
        /// Desc:奖项 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Awards {get;set;}

        /// <summary>
        /// Desc:奖品数量 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 Inventory {get;set;}

        /// <summary>
        /// Desc:概率 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public decimal Probability {get;set;}

    }
}
