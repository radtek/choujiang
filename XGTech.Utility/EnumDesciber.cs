using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Utility
{
    public class EnumDesciber
    {
        public static string GetEnumDescription<T>(T enumObject)
        {
            if (!typeof(T).IsEnum)
            {
                throw new FormatException("只支持枚举类型");
            }
            return GetEnumDescription(enumObject.ToString(), typeof(T));
        }

        public static string GetEnumDescription<T>(string value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new FormatException("只支持枚举类型");
            }
            T enumObject = (T)Enum.Parse(typeof(T), value);
            return GetEnumDescription<T>(enumObject);
        }

        public static string GetEnumDescription(string name, Type type)
        {
            if (!type.IsEnum)
            {
                throw new FormatException("只支持枚举类型");
            }
            FieldInfo fieldInfo = type.GetField(name);
            DescriptionAttribute description = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));

            return description.Description;

        }
    }
}
