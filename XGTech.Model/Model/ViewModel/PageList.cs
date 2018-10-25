using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Model.Model.ViewModel
{
    public class PageList
    {
        public object data { get; set; }

        public int pageIndex { get; set; }

        public int pageCount { get; set; }

        public int pageSize { get; set; }

        public int count { get; set; }

        public string msg { get; set; }

        public string code { get; set; } = "0";
    }
}
