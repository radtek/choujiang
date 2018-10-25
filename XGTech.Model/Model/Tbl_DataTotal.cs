using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.Model
{
    public class Tbl_DataTotal
    {
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string activity_time { get; set; }
        public string activity_title { get; set; }
        public string roi { get; set; }
        public int visitor_count { get; set; }
        public int login_count { get; set; }
        public int lottery_count { get; set; }
        public int win_count { get; set; }
        public int cash_count { get; set; }
        public int lottery_number { get; set; }
        public int win_number { get; set; }
        public decimal totalworth { get; set; }
        public decimal activity_income { get; set; }
        public int share_number { get; set; }
        public int reflux_count { get; set; }
        public DateTime updatetime { get; set; }

    }
}
