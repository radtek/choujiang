using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.ResponseModel
{
    public class UserResponseModel
    {

    }

    public class UserInfo
    {
        public long user_id { get; set; }
        public string emp_no { get; set; }
        public string user_name { get; set; }
        public List<UserRole> role_list { get; set; }
    }

    public class UserRole
    {
        public string role_id { get; set; }
        public string role_name { get; set; }
    }

}
