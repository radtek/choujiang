using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Model.Model.ViewModel
{
    public class QueryDTO
    {

        public long net_no { get; set; }
        public string Keyword { get; set; }

        public int page { get; set; }

        public int limit { get; set; }
    }
}
