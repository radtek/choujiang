using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.Model.ViewModel
{
    public class RoleMenu
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int? menu_id { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string menu_name { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        public int? menu_group_id { get; set; }

        public long? role_id { get; set; }
    }
}
