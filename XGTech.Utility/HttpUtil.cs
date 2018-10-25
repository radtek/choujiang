using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XGTech.V2.WebJob.Utility
{
    /// <summary>
    /// 上传数据参数
    /// </summary>
    public class UploadEventArgs : EventArgs
    {
        int bytesSent;
        int totalBytes;

        /// <summary>
        /// 已发送的字节数
        /// </summary>
        public int BytesSent
        {
            get { return bytesSent; }
            set { bytesSent = value; }
        }

        /// <summary>
        /// 总字节数
        /// </summary>
        public int TotalBytes
        {
            get { return totalBytes; }
            set { totalBytes = value; }
        }
    }

    /// <summary>
    /// 下载数据参数
    /// </summary>
    public class DownloadEventArgs : EventArgs
    {
        int bytesReceived;
        int totalBytes;
        byte[] receivedData;

        /// <summary>
        /// 已接收的字节数
        /// </summary>
        public int BytesReceived
        {
            get { return bytesReceived; }
            set { bytesReceived = value; }
        }

        /// <summary>
        /// 总字节数
        /// </summary>
        public int TotalBytes
        {
            get { return totalBytes; }
            set { totalBytes = value; }
        }

        /// <summary>
        /// 当前缓冲区接收的数据
        /// </summary>
        public byte[] ReceivedData
        {
            get { return receivedData; }
            set { receivedData = value; }
        }
    }

    public delegate void UploadProgressChangedEventHandler(UploadEventArgs e);
    public delegate void DownloadProgressChangedEventHandler(DownloadEventArgs e);

    /// <summary>
    /// HttpUtil
    /// </summary>
    public class HttpUtil
    {
        private int bufferSize = 15240;

        public event UploadProgressChangedEventHandler OnUploadProgressChanged;
        public event DownloadProgressChangedEventHandler OnDownloadProgressChanged;

        static HttpUtil()
        {
            LoadCookiesFromDisk();
        }

        /// <summary>
        /// 创建HttpUtil的实例
        /// </summary>
        public HttpUtil()
        {
            requestHeaders = new WebHeaderCollection();
            responseHeaders = new WebHeaderCollection();
        }

        /// <summary>
        /// 设置发送和接收的数据缓冲大小
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        }

        private WebHeaderCollection responseHeaders;
        /// <summary>
        /// 获取响应头集合
        /// </summary>
        public WebHeaderCollection ResponseHeaders
        {
            get { return responseHeaders; }
        }

        private WebHeaderCollection requestHeaders;
        /// <summary>
        /// 获取请求头集合
        /// </summary>
        public WebHeaderCollection RequestHeaders
        {
            get { return requestHeaders; }
        }

        private WebProxy proxy;
        /// <summary>
        /// 获取或设置代理
        /// </summary>
        public WebProxy Proxy
        {
            get { return proxy; }
            set { proxy = value; }
        }

        private Encoding encoding = Encoding.UTF8;
        /// <summary>
        /// 获取或设置请求与响应的文本编码方式
        /// </summary>
        public Encoding Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        private string responseHtml = string.Empty;
        /// <summary>
        /// 获取或设置响应的html代码
        /// </summary>
        public string ResponseHtml
        {
            get { return responseHtml; }
            set { responseHtml = value; }
        }

        private static CookieContainer cc;
        /// <summary>
        /// 获取或设置与请求关联的Cookie容器
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return cc; }
            set { cc = value; }
        }

        /// <summary>
        /// 获取网页源代码
        /// </summary>
        /// <param name="url">网址</param>
        /// <returns></returns>
        public string GetHtml(string url)
        {
            HttpWebRequest request = CreateRequest(url, "GET");
            responseHtml = encoding.GetString(GetData(request));

            return responseHtml;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">文件URL地址</param>
        /// <param name="filename">文件保存完整路径</param>
        public void DownloadFile(string url, string filename)
        {
            FileStream fs = null;
            try
            {
                HttpWebRequest request = CreateRequest(url, "GET");
                byte[] data = GetData(request);

                fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                fs.Write(data, 0, data.Length);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 从指定URL下载数据
        /// </summary>
        /// <param name="url">网址</param>
        public byte[] GetData(string url)
        {
            HttpWebRequest request = CreateRequest(url, "GET");
            return GetData(request);
        }

        /// <summary>
        /// 向指定URL发送文本数据
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="postData">urlencode编码的文本数据 </param>
        /// <returns></returns>
        public string Post(string url, string postData)
        {
            byte[] data = encoding.GetBytes(postData);
            return Post(url, data);
        }

        /// <summary>
        /// 向指定URL发送文本数据
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="postData">urlencode编码的文本数据 </param>
        /// <param name="gzip">是否使用Gzip压缩</param>
        /// <returns></returns>
        public string Post(string url, string postData, bool gzip)
        {
            byte[] data = encoding.GetBytes(postData);
            return Post(url, data, gzip);
        }

        /// <summary>
        /// 向指定URL发送字节数据
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="postData">发送的字节数组</param>
        public string Post(string url, byte[] postData)
        {
            HttpWebRequest request = CreateRequest(url, "POST");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postData.Length;
            request.KeepAlive = true;
            request.Timeout = 30000;
            PostData(request, postData);

            responseHtml = encoding.GetString(GetData(request));
            return responseHtml;
        }

        /// <summary>
        /// 向指定URL发送字节数据
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="postData">发送的字节数组</param>
        /// <param name="gzip">是否使用Gzip压缩</param>
        public string Post(string url, byte[] postData, bool gzip)
        {
            HttpWebRequest request = CreateRequest(url, "POST");
            if (gzip)
            {
                request.ContentType = "application/x-www-form-urlencoded; Accept-Encoding: gzip";
                postData = GzipCompress(postData);
            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }

            request.ContentLength = postData.Length;
            request.KeepAlive = true;

            PostData(request, postData);

            responseHtml = encoding.GetString(GetData(request));
            return responseHtml;
        }

        /// <summary>
        /// Gzip压缩
        /// </summary>
        public byte[] GzipCompress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                GZipStream compress = new GZipStream(ms, CompressionMode.Compress);
                compress.Write(bytes, 0, bytes.Length);
                compress.Close();

                return ms.ToArray();
            }
        }
        /// <summary>
        /// Gzip解压
        /// </summary>
        public string GzipUnCompress(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                GZipStream gzs1 = new GZipStream(ms, CompressionMode.Decompress);
                StreamReader streamR = new StreamReader(gzs1, Encoding.Default);
                string str = streamR.ReadToEnd();
                gzs1.Close();
                return str;
            }
        }

        /// <summary>
        /// 向指定网址发送mulitpart编码的数据
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="mulitpartForm">mulitpart form data</param>
        public string Post(string url, MultipartForm mulitpartForm)
        {
            HttpWebRequest request = CreateRequest(url, "POST");

            request.ContentType = mulitpartForm.ContentType;
            request.ContentLength = mulitpartForm.FormData.Length;
            request.KeepAlive = true;

            PostData(request, mulitpartForm.FormData);

            responseHtml = encoding.GetString(GetData(request));
            return responseHtml;
        }

        /// <summary>
        /// 读取请求返回的数据
        /// </summary>
        /// <param name="request">请求</param>
        private byte[] GetData(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();

            responseHeaders = response.Headers;

            SaveCookiesToDisk();

            DownloadEventArgs args = new DownloadEventArgs();

            if (responseHeaders[HttpResponseHeader.ContentLength] != null)
                args.TotalBytes = Convert.ToInt32(responseHeaders[HttpResponseHeader.ContentLength]);

            MemoryStream ms = new MemoryStream();
            int count = 0;
            byte[] buf = new byte[bufferSize];

            while ((count = stream.Read(buf, 0, buf.Length)) > 0)
            {
                ms.Write(buf, 0, count);

                if (this.OnDownloadProgressChanged != null)
                {
                    args.BytesReceived += count;
                    args.ReceivedData = new byte[count];

                    Array.Copy(buf, args.ReceivedData, count);

                    this.OnDownloadProgressChanged(args);
                }
            }

            stream.Close();

            // 解压
            if (ResponseHeaders[HttpResponseHeader.ContentEncoding] != null)
            {
                MemoryStream msTemp = new MemoryStream();

                count = 0;
                buf = new byte[100];

                switch (ResponseHeaders[HttpResponseHeader.ContentEncoding].ToLower())
                {
                    case "gzip":
                        GZipStream gzip = new GZipStream(ms, CompressionMode.Decompress);

                        while ((count = gzip.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }

                        return msTemp.ToArray();
                    case "deflate":
                        DeflateStream deflate = new DeflateStream(ms, CompressionMode.Decompress);
                        while ((count = deflate.Read(buf, 0, buf.Length)) > 0)
                        {
                            msTemp.Write(buf, 0, count);
                        }

                        return msTemp.ToArray();
                    default:
                        break;
                }
            }
            return ms.ToArray();
        }

        /// <summary>
        /// 发送请求数据
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <param name="postData">请求发送的字节数组</param>
        private void PostData(HttpWebRequest request, byte[] postData)
        {
            int offset = 0;
            int sendBufferSize = bufferSize;
            int remainBytes = 0;

            Stream stream = request.GetRequestStream();
            UploadEventArgs args = new UploadEventArgs();

            args.TotalBytes = postData.Length;

            while ((remainBytes = postData.Length - offset) > 0)
            {
                if (sendBufferSize > remainBytes)
                    sendBufferSize = remainBytes;

                stream.Write(postData, offset, sendBufferSize);

                offset += sendBufferSize;

                if (this.OnUploadProgressChanged != null)
                {
                    args.BytesSent = offset;
                    this.OnUploadProgressChanged(args);
                }
            }
            stream.Close();
        }

        /// <summary>
        /// 创建HTTP请求
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="method">方法</param>
        /// <returns></returns>
        private HttpWebRequest CreateRequest(string url, string method)
        {
            Uri uri = new Uri(url);
            if (uri.Scheme == "https")
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);

            // Set a default policy level for the "http:" and "https" schemes.

            HttpRequestCachePolicy policy = new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);

            HttpWebRequest.DefaultCachePolicy = policy;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AllowAutoRedirect = false;
            request.AllowWriteStreamBuffering = false;
            request.Method = method;

            if (proxy != null)
                request.Proxy = proxy;

            request.CookieContainer = cc;

            foreach (string key in requestHeaders.Keys)
            {
                request.Headers.Add(key, requestHeaders[key]);
            }
            requestHeaders.Clear();

            return request;
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// 将Cookie保存到磁盘
        /// </summary>
        private static void SaveCookiesToDisk()
        {
            string cookieFile = System.Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";

            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Create);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formater.Serialize(fs, cc);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 从磁盘加载Cookie
        /// </summary>
        private static void LoadCookiesFromDisk()
        {
            cc = new CookieContainer();

            string cookieFile = System.Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";

            if (!File.Exists(cookieFile))
                return;

            FileStream fs = null;
            try
            {
                fs = new FileStream(cookieFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formater = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                cc = (CookieContainer)formater.Deserialize(fs);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        private static bool RemoteCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //用户https请求
            return true; //总是接受
        }
        public string SendPost(string url, string data)
        {
            return Send(url, "POST", data, null);
            //return Post(url,data,true);
        }
        public string Send(string url, string method, string data, HttpConfig config)
        {
            if (config == null) config = new HttpConfig();
            string result;
            using (HttpWebResponse response = GetResponse(url, method, data, config))
            {
                Stream stream = response.GetResponseStream();

                //if (!String.IsNullOrEmpty(response.ContentEncoding))
                //{
                //    if (response.ContentEncoding.Contains("gzip"))
                //{
                if (config.GZipCompress)
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                //    }
                //    else if (response.ContentEncoding.Contains("deflate"))
                //    {
                //        stream = new DeflateStream(stream, CompressionMode.Decompress);
                //    }
                //}

                byte[] bytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    int count;
                    byte[] buffer = new byte[4096];
                    while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, count);
                    }
                    bytes = ms.ToArray();
                }

                #region 检测流编码
                Encoding encoding;

                //检测响应头是否返回了编码类型,若返回了编码类型则使用返回的编码
                //注：有时响应头没有编码类型，CharacterSet经常设置为ISO-8859-1
                if (!string.IsNullOrEmpty(response.CharacterSet) && response.CharacterSet.ToUpper() != "ISO-8859-1")
                {
                    encoding = Encoding.GetEncoding(response.CharacterSet == "utf8" ? "utf-8" : response.CharacterSet);
                }
                else
                {
                    //若没有在响应头找到编码，则去html找meta头的charset
                    result = Encoding.Default.GetString(bytes);
                    //在返回的html里使用正则匹配页面编码
                    Match match = Regex.Match(result, @"<meta.*charset=""?([\w-]+)""?.*>", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        encoding = Encoding.GetEncoding(match.Groups[1].Value);
                    }
                    else
                    {
                        //若html里面也找不到编码，默认使用utf-8
                        encoding = Encoding.GetEncoding(config.CharacterSet);
                    }
                }
                #endregion

                result = encoding.GetString(bytes);
            }
            return result;
        }
        private static HttpWebResponse GetResponse(string url, string method, string data, HttpConfig config)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Referer = config.Referer;
            //有些页面不设置用户代理信息则会抓取不到内容
            request.UserAgent = config.UserAgent;
            request.Timeout = config.Timeout;
            request.Accept = config.Accept;
            request.Headers.Set("Accept-Encoding", config.AcceptEncoding);
            request.ContentType = config.ContentType;
            request.KeepAlive = config.KeepAlive;

            if (url.ToLower().StartsWith("https"))
            {
                //这里加入解决生产环境访问https的问题--Could not establish trust relationship for the SSL/TLS secure channel
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidate);
            }
            if (method.ToUpper() == "POST")
            {
                if (!string.IsNullOrEmpty(data))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);

                    if (config.GZipCompress)
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress))
                            {
                                gZipStream.Write(bytes, 0, bytes.Length);
                            }
                            bytes = stream.ToArray();
                        }
                    }

                    request.ContentLength = bytes.Length;
                    request.GetRequestStream().Write(bytes, 0, bytes.Length);
                }
                else
                {
                    request.ContentLength = 0;
                }
            }

            return (HttpWebResponse)request.GetResponse();
        }
    }

    public class HttpConfig
    {
        public string Referer { get; set; }

        /// <summary>
        /// 默认(text/html)
        /// </summary>
        public string ContentType { get; set; }

        public string Accept { get; set; }

        public string AcceptEncoding { get; set; }

        /// <summary>
        /// 超时时间(毫秒)默认100000
        /// </summary>
        public int Timeout { get; set; }

        public string UserAgent { get; set; }

        private bool _GZipCompress;
        /// <summary>
        /// POST请求时，数据是否进行gzip压缩
        /// </summary>
        public bool GZipCompress 
        { 
            get
            {
                return _GZipCompress;
            }
            set
            {
                _GZipCompress = value;
                this.AcceptEncoding = _GZipCompress ? "gzip,deflate" : "deflate";
            }
        }

        public bool KeepAlive { get; set; }

        public string CharacterSet { get; set; }

        public HttpConfig()
        {
            this.Timeout = 100000;
            this.ContentType = "text/html; charset=" + Encoding.UTF8.WebName;
            this.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.117 Safari/537.36";
            this.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //this.AcceptEncoding = "gzip,deflate";
            this.GZipCompress = true;
            this.KeepAlive = true;
            this.CharacterSet = "UTF-8";
        }
    }
    /// <summary>
    /// 对文件和文本数据进行Multipart形式的编码
    /// </summary>
    public class MultipartForm
    {
        private MemoryStream ms;
        private string boundary;

        private byte[] formData;
        /// <summary>
        /// 获取编码后的字节数组
        /// </summary>
        public byte[] FormData
        {
            get
            {
                if (formData == null)
                {
                    byte[] buffer = encoding.GetBytes("--" + this.boundary + "--\r\n");
                    ms.Write(buffer, 0, buffer.Length);
                    formData = ms.ToArray();
                }
                return formData;
            }
        }

        private Encoding encoding;
        /// <summary>
        /// 获取此编码内容的类型
        /// </summary>
        public string ContentType
        {
            get { return string.Format("multipart/form-data; boundary={0}", this.boundary); }
        }

        /// <summary>
        /// 获取或设置对字符串采用的编码类型
        /// </summary>
        public Encoding StringEncoding
        {
            set { encoding = value; }
            get { return encoding; }
        }

        /// <summary>
        /// 实例化
        /// </summary>
        public MultipartForm()
        {
            boundary = string.Format("--{0}--", Guid.NewGuid());
            ms = new MemoryStream();
            encoding = Encoding.Default;
        }

        /// <summary>
        /// 添加一个文件
        /// </summary>
        /// <param name="name">文件域名称</param>
        /// <param name="filename">文件的完整路径</param>
        public void AddFlie(string name, string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("尝试添加不存在的文件。", filename);

            FileStream fs = null;
            byte[] fileData = { };
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                fileData = new byte[fs.Length];

                fs.Read(fileData, 0, fileData.Length);

                this.AddFlie(name, Path.GetFileName(filename), fileData, fileData.Length);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 添加一个文件
        /// </summary>
        /// <param name="name">文件域名称</param>
        /// <param name="filename">文件名</param>
        /// <param name="fileData">文件二进制数据</param>
        /// <param name="dataLength">二进制数据大小</param>
        public void AddFlie(string name, string filename, byte[] fileData, int dataLength)
        {
            if (dataLength <= 0 || dataLength > fileData.Length)
            {
                dataLength = fileData.Length;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", this.boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n", name, filename);
            sb.AppendFormat("Content-Type: {0}\r\n", this.GetContentType(filename));
            sb.Append("\r\n");

            byte[] buf = encoding.GetBytes(sb.ToString());

            ms.Write(buf, 0, buf.Length);
            ms.Write(fileData, 0, dataLength);

            byte[] crlf = encoding.GetBytes("\r\n");

            ms.Write(crlf, 0, crlf.Length);
        }

        /// <summary>
        /// 添加字符串
        /// </summary>
        /// <param name="name">文本域名称</param>
        /// <param name="value">文本值</param>
        public void AddString(string name, string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("--{0}\r\n", this.boundary);
            sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n", name);
            sb.Append("\r\n");
            sb.AppendFormat("{0}\r\n", value);

            byte[] buf = encoding.GetBytes(sb.ToString());

            ms.Write(buf, 0, buf.Length);
        }

        /// <summary>
        /// 从注册表获取文件类型
        /// </summary>
        /// <param name="filename">包含扩展名的文件名 </param>
        /// <returns></returns>
        private string GetContentType(string filename)
        {
            Microsoft.Win32.RegistryKey fileExtKey = null; ;

            string contentType = "application/stream";
            try
            {
                fileExtKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename));
                contentType = fileExtKey.GetValue("Content Type", contentType).ToString();
            }
            finally
            {
                if (fileExtKey != null)
                    fileExtKey.Close();
            }
            return contentType;
        }


    }
}
