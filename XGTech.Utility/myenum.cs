using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Utility
{
    public enum moveorder
    {
        
        领用 = 1,
        退回 = 2,
    };
    public enum storage
    {
       
       正常入库=1,
       返修入库=2
    };
    public enum outorder
    {
        返厂维修=1,
        报废=2,
        变卖=3
    };
}
