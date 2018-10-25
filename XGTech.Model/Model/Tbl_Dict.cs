using System;
using System.Linq;
using System.Text;

namespace XGTech.Model.Model
{
    public class Tbl_Dict
    {
        
        /// <summary>
        /// Desc:类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string DType {get;set;}

        /// <summary>
        /// Desc:ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Int64 DKey {get;set;}

        /// <summary>
        /// Desc:内容 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string DValue {get;set;}

    }
}
