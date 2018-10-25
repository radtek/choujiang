using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.Model.ViewModel
{
    public class RoleUserQueryModel: QueryDTO
    {

        public long RoleId { get; set; }
        public string Keyword { get; set; }
    }
}
