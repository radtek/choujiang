using System;
using System.Collections.Generic;
using System.Text;

namespace XGTech.Utility
{
    /// <summary>
    /// 用户传递信息的异常类
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class MessageException : BaseCustomException
    {
        public MessageException() { }
        public MessageException(string message) : base(message) { }
        public MessageException(string message, Exception inner) : base(message, inner) { }
    }
}


