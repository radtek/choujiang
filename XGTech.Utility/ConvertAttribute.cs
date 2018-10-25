using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGTech.Utility
{
    public class ConvertAttribute:Attribute
    {
        public bool HasTime { get; set; }

        public bool HasException { get; set; }

        public Type OpposedType { get; set; }

        public EntityViewModelConvertType ConvertType { get; set; }

        public Type EnumType { get; set; }
    }
}
