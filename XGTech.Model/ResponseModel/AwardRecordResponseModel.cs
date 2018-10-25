using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.ResponseModel
{
    public class AwardRecordResponseModel
    {
        
    }


    public class AwardRecordProbResponseModel
    {
        public long ID { get; set; }

        public long PType { get; set; }

        public decimal Probability { get; set; }

        public String PRule { get; set; }

        public Int64 Inventory { get; set; }

        public Int64 ApsID { get; set; }

        public decimal Pworth { get; set; }
    }

    public class AwardRecordPrizeResponseModel
    {
        public long ID { get; set; }

        public String PName { get; set; }

        public String PImg { get; set; }

        public String PDetails { get; set; }

        /// <summary>
        /// 中奖时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 奖品类型
        /// </summary>
        public long PTYpe { get; set; }


        public int Status { get; set; }

    }
}
