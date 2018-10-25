namespace XGTech.IBLL
{
    public class BaseResponse
    {
        public object list { get; set; }

        public int pageIndex { get; set; }

        public int pageCount { get; set; }

        public int pageSize { get; set; }

        public int count { get; set; }

        public string msg { get; set; }

        public string code { get; set; } = "0";
    }
}