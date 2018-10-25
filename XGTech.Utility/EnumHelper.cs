using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XGTech.Utility
{
    public enum ClaimProgressType
    {
        /// <summary>
        /// 客户重新提交
        /// </summary>
        [EnumDescription("客户重新提交")]
        CustomerRestSubmit = 0,
        /// <summary>
        /// 客服已提交
        /// </summary>
        [EnumDescription("客服已提交")]
        CustomerSubmit = 1,
        /// <summary>
        /// 客服处理中
        /// </summary>
        [EnumDescription("客服处理中")]
        ServiceDispose = 2,
        /// <summary>
        /// 客服处理完成
        /// </summary>
        [EnumDescription("客服处理完成")]
        ServiceDispostFinish = 3,
        /// <summary>
        /// 客户确认完成
        /// </summary>
        [EnumDescription("客户确认完成")]
        CustomerConfirmFinish = 4
    }

    /// <summary>
    /// 生成缩略图时所使用的模式
    /// </summary>
    public enum ThumbnailPicType
    {
        /// <summary>
        /// 智能匹配最大模式
        /// </summary>
        [EnumDescription("智能匹配最大模式")]
        Auto,
        /// <summary>
        /// 精确
        /// </summary>
        [EnumDescription("精确")]
        Cut,
        /// <summary>
        /// 按宽度
        /// </summary>
        [EnumDescription("按宽度")]
        Width,
        /// <summary>
        /// 按高度
        /// </summary>
        [EnumDescription("按高度")]
        Height
    }

    /// <summary>
    /// 上传文件所引发的错误
    /// </summary>
    public enum FileHelperException
    {
        /// <summary>
        /// 正常，没有错误
        /// </summary>
        [EnumDescription("没有错误")]
        None,
        /// <summary>
        /// 无效的文件
        /// </summary>
        [EnumDescription("无效的文件")]
        FileError,
        /// <summary>
        /// 文件已存在
        /// </summary>
        [EnumDescription("文件已存在")]
        FileExists,
        /// <summary>
        /// 文件类型不正确
        /// </summary>
        [EnumDescription("文件类型不正确")]
        FileTypeError,
        /// <summary>
        /// 上传过程中出现错误
        /// </summary>
        [EnumDescription("上传过程中出现错误")]
        UploadCauseError
    }

    /// <summary>
    /// 用于确定文件是什么类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// JPG文件
        /// </summary>
        [EnumDescription("255216")]
        JPG,
        /// <summary>
        /// GIF文件
        /// </summary>
        [EnumDescription("7173")]
        GIF,
        /// <summary>
        /// BMP文件
        /// </summary>
        [EnumDescription("6677")]
        BMP,
        /// <summary>
        /// PNG文件
        /// </summary>
        [EnumDescription("13780")]
        PNG,
        /// <summary>
        /// RAR文件
        /// </summary>
        [EnumDescription("8279")]
        RAR,
        /// <summary>
        /// EXE文件
        /// </summary>
        [EnumDescription("7790")]
        EXE,
        /// <summary>
        /// Office文件
        /// </summary>
        [EnumDescription("208207")]
        Office,
        /// <summary>
        /// Office2007文件
        /// </summary>
        [EnumDescription("8075")]
        Office2007,

        [EnumDescription("8076")]
        DOC,

        [EnumDescription("8076")]
        PDF,

        [EnumDescription("8076")]
        XLSX,

        [EnumDescription("8076")]
        XLS,

        [EnumDescription("8076")]
        SWF,
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescription : Attribute
    {
        private string desc;
        private int rank;
        private FieldInfo fieldIno;

        /// <summary>         
        /// 描述枚举值         
        /// </summary>         
        /// <param name="desc">描述内容</param>         
        /// <param name="rank">排列顺序</param>         
        public EnumDescription(string desc, int rank)
        {
            this.desc = desc;
            this.rank = rank;
        }

        /// <summary>         
        /// 描述枚举值，默认排序为5         
        /// </summary>         
        /// <param name="desc">描述内容</param>         
        public EnumDescription(string desc)
            : this(desc, 5)
        {
        }

        /// <summary>
        /// 描述内容
        /// </summary>
        public string Desc
        {
            get { return this.desc; }
        }

        /// <summary>
        /// 排序优先级
        /// </summary>
        public int Rank
        {
            get { return rank; }
        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public int Value
        {
            get
            {
                if (fieldIno == null) return 0;

                return (int)fieldIno.GetValue(null);
            }
        }

        /// <summary>
        /// 枚举名
        /// </summary>
        public string FieldName
        {
            get { return fieldIno.Name; }
        }

        /// <summary>         
        /// 排序类型         
        /// </summary>         
        public enum SortType
        {
            /// <summary>             
            ///按枚举顺序默认排序             
            /// </summary>             
            Default,

            /// <summary>             
            /// 按描述值排序             
            /// </summary>
            DisplayText,

            /// <summary>             
            /// 按排序熵
            /// </summary>             
            Rank
        }

        private static Hashtable cachedEnum = new Hashtable();

        /// <summary>         
        /// 获取枚举的描述文本
        /// </summary>         
        /// <param name="enumType">枚举类型</param>         
        /// <returns></returns>         
        public static string GetEnumDesc(Type enumType)
        {
            EnumDescription[] eds = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            if (eds.Length != 1)
                return string.Empty;

            return eds[0].Desc;
        }

        /// <summary>         
        /// 获取枚举值的描述字符串
        /// </summary>         
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>         
        /// <returns>描述字符串</returns>         
        public static string GetFieldDesc(object enumValue)
        {
            List<EnumDescription> descriptions = GetFieldDescList(enumValue.GetType(), SortType.Default);
            foreach (EnumDescription ed in descriptions)
            {
                if (ed.fieldIno != null && ed.fieldIno.Name == enumValue.ToString())
                    return ed.Desc;
            }

            return string.Empty;
        }

        /// <summary>         
        /// 得到枚举类型定义的所有文本，按定义的顺序返回         
        /// </summary>         
        /// <exception cref="NotSupportedException"></exception>         
        /// <param name="enumType">枚举类型</param>         
        /// <returns>所有定义的文本</returns>         
        public static List<EnumDescription> GetFieldDescList(Type enumType)
        {
            return GetFieldDescList(enumType, SortType.Default);
        }

        public static string GetEnumDescription(object enumSubitem)
        {
            enumSubitem = (Enum)enumSubitem;
            string strValue = enumSubitem.ToString();

            FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);

            if (fieldinfo != null)
            {

                Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objs == null || objs.Length == 0)
                {
                    return strValue;
                }
                else
                {
                    DescriptionAttribute da = (DescriptionAttribute)objs[0];
                    return da.Description;
                }
            }
            else
            {
                return "不限";
            }

        }


        /// <summary>
        /// 得到枚举类型定义的所有文本         
        /// </summary>         
        /// <exception cref="NotSupportedException"></exception>         
        /// <param name="enumType">枚举类型</param>         
        /// <param name="sortType">指定排序类型</param>         
        /// <returns>所有定义的文本</returns>         
        public static List<EnumDescription> GetFieldDescList(Type enumType, SortType sortType)
        {
            List<EnumDescription> descriptions = null;
            //缓存中没有找到，通过反射获得字段的描述信息             
            if (cachedEnum.Contains(enumType.FullName) == false)
            {
                FieldInfo[] fields = enumType.GetFields();
                List<EnumDescription> edAL = new List<EnumDescription>();
                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumDescription), false);
                    if (eds.Length != 1)
                        continue;
                    ((EnumDescription)eds[0]).fieldIno = fi;
                    edAL.Add((EnumDescription)eds[0]);
                }

                cachedEnum.Add(enumType.FullName, edAL);
            }

            descriptions = (List<EnumDescription>)cachedEnum[enumType.FullName];
            if (descriptions.Count <= 0)
                throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription");

            //按指定的属性冒泡排序             
            for (int m = 0; m < descriptions.Count; m++)
            {
                //默认就不排序了                 
                if (sortType == SortType.Default)
                    break;

                for (int n = m; n < descriptions.Count; n++)
                {
                    EnumDescription temp;
                    bool swap = false;

                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (string.Compare(descriptions[m].Desc, descriptions[n].Desc) > 0)
                                swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].Rank > descriptions[n].Rank)
                                swap = true;
                            break;
                    }

                    if (swap)
                    {
                        temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }
            return descriptions;
        }

        /* 用法 */
        /*           
         * string txt = EnumDescription.GetEnumText(typeof(OrderStateEnum));  
         * 
         * string txt = EnumDescription.GetFieldDesc(OrderStateEnum.Processing); 
         * 
         * EnumDescription[] des = EnumDescription.GetFieldDescList(typeof(OrderStateEnum))  
         * 
         * public static EnumDescription[] GetFieldDescList( Type enumType, SortType sortType ) 
         * 
         */
    }
}
