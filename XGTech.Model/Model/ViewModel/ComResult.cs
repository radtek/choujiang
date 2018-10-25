using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Model.Model.ViewModel
{
    public class ComResult
    {
        public object DataResult { get; set; }

        /// <summary>
        /// 0失败1成功
        /// </summary>
        public int State { get; set; }

        public string Msg { get; set; }

        public string url { get; set; }
    }
}
