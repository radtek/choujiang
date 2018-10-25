

using System;
using System.Linq;
using System.Text;

namespace XGTech.Model.Model
{
    public class wz_tbl_base_users
    {

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int64 user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 emp_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string emp_no { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string user_pwd { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 net_no { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 org_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string salt { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string real_name { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string telphone { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Byte? user_status { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 job_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public int? duty { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public int? position { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string op_ip { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime open_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime disable_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string token_code { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime token_expiry_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string sto_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 create_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime edit_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 edit_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Boolean del_flag { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime del_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 del_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string remark { get; set; }

    }


    public class View_base_user
    {

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Int64 user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 emp_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string emp_no { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string user_pwd { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 net_no { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 org_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string salt { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string real_name { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string telphone { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Byte? user_status { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 job_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public int? duty { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public int? position { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string op_ip { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime open_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime disable_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string token_code { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime token_expiry_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string sto_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime create_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 create_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime edit_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 edit_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Boolean del_flag { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:('getdate()') 
        /// Nullable:False 
        /// </summary>
        public DateTime del_time { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public Int64 del_user_id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:(NULL) 
        /// Nullable:True 
        /// </summary>
        public string remark { get; set; }

        public string createName { get; set; }

    }
}
