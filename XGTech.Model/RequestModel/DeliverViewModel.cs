using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class DeliverViewModel
    {
        public Int64 ID { get; set; }

        public String BillCode { get; set; }

        public int ShippingStatus { get; set; }

        public Int64 Express { get; set; }
    }
}
