using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Model.Model.ViewModel
{
    public class TreeGridDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public int pid { get; set; }
        public List<FunDTO> funs { get; set; }

        public bool isAllselect { get; set; }
    }
}
