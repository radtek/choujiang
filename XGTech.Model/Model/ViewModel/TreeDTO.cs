using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Model.Model.ViewModel
{
    public class TreeDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public IEnumerable<TreeDTO> children { get; set; }

        public bool spread { get; set; }
    }
}
