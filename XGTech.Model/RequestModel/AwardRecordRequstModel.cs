using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class AwardRecordRequstModel
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
        public Int64? PrizeID { get; set; }

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
    }

    /// <summary>
    /// 兑换
    /// </summary>
    public class AwardRecordExchangeRequstModel
    {
        public long id { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public String recipients { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public String phone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public String province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public String city { get; set; }

        /// <summary>
        /// 县
        /// </summary>
        public String county { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public String address { get; set; }
    }


   
    public class AwardRecordPagesRequstModel
    {
        public int page { get; set; } = 1;

        public int limit { get; set; } = 10;

        public String PName { get; set; }

        public Int64? PType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public String Title { get; set; }

        public String UserName { get; set; }

        public String BillCode { get; set; }

        public int? ShippingStatus { get; set; }

        public int? Status { get; set; }

    }

    public class AwardStatExportResponseModel
    {
        [DisplayName("开始时间")]
        public DateTime StartDate { get; set; }

        [DisplayName("结束时间")]
        public DateTime EndDate { get; set; }

        [DisplayName("活动编号")]
        public String Number { get; set; }

        [DisplayName("标题")]
        public String Title { get; set; }

        [DisplayName("抽奖人数")]
        public int cjCount { get; set; }

        [DisplayName("抽奖次数")]
        public int cjTime { get; set; }

        [DisplayName("中奖人数")]
        public int zjCount { get; set; }

        [DisplayName("奖品价值")]
        public int djja { get; set; }

        [DisplayName("兑奖人数")]
        public int djcount { get; set; }
    }
}
