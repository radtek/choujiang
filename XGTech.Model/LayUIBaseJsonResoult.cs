using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model
{
    public class LayUIBaseJsonResoult
    {
        public string msg { get; set; }

        public string code { get; set; } = "0";

        public int count { get; set; }

        public object data { get; set; }
    }
}
