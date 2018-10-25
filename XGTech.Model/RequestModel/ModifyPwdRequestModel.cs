using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.RequestModel
{
    public class ModifyPwdRequestModel
    {
        public String oldPwd { get; set; }

        public String newPwd { get; set; }

        public String affirmPwd { get; set; }
    }
}
