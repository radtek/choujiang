using System;
using System.Linq;
using System.Text;

namespace XGTech.Model.Model
{
    public class Tbl_AwardRecord
    {

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int64 ID { get; set; }

        /// <summary>
        /// Desc:活动ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 ActivityID { get; set; }

        /// <summary>
        /// Desc:抽奖日期 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// Desc:用户ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 UserID { get; set; }

        /// <summary>
        /// Desc:用户名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Desc:奖品ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 PrizeID { get; set; }

        /// <summary>
        /// Desc:兑换状态,0.未兑奖、1.已兑奖 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Desc:兑换时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? ExchangeTime { get; set; }

        /// <summary>
        /// Desc:发货状态,0.未发货、1.已发货 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? ShippingStatus { get; set; }

        /// <summary>
        /// Desc:收货地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Desc:物流单号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string BillCode { get; set; }

        /// <summary>
        /// Desc:是否中奖,0.未中奖、1.已中奖 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? IsPrize { get; set; }

        /// <summary>
        /// Desc:收件人 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Recipients { get; set; }

        /// <summary>
        /// Desc:手机 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Phone { get; set; }

        public decimal Worth { get; set; }

        public Int64 Express { get; set; }

        public Int64 PType { get; set; }

    }
}
