using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace XGTech.Utility
{
    /// <summary>
    /// 基础的用户自定义异常
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class BaseCustomException : Exception
    {
        public BaseCustomException()
        {
        }

        public BaseCustomException(string message) : base(message)
        {
        }

        public BaseCustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseCustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
