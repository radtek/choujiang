using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Model.Model.ViewModel
{
    public class FunDTO
    {
        public long fun_id { get; set; }
        /// <summary>
        /// 是否是查看，如果是查看，不用管fun表
        /// </summary>
        public bool isLook { get; set; }

        public string fun_name { get; set; }

        public long menu_id { get; set; }

        public bool ischecked { get; set; }
    }
}
