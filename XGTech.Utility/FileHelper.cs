using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace XGTech.Utility
{
   //public class FileHelper
   // {
   //     #region 文件操作
   //     /// <summary>
   //     /// 判断虚拟路径中的文件是否存在
   //     /// </summary>
   //     /// <param name="filePath">文件路径</param>
   //     /// <param name="filePath">是否为虚拟路径</param>
   //     /// <returns>是否存在</returns>
   //     public static bool FileExists(string filePath, bool virtualPath = true)
   //     {
   //         if (virtualPath)
   //         {
   //             return File.Exists(HttpContext.Current.Server.MapPath(filePath));
   //         }
   //         else
   //         {
   //             return File.Exists(filePath);
   //         }
   //     }


   //     /// <summary>
   //     /// 从源路径复制到目标路径
   //     /// </summary>
   //     /// <param name="sourceFilePath">源文件路径</param>
   //     /// <param name="destFilePath">目标路径</param>
   //     /// <param name="sourceFileVirtualPath">源文件是否为虚拟路径</param>
   //     /// <param name="destFileVirtualPath">目标是否为虚拟路径</param>
   //     public static void CopyFile(string sourceFilePath, string destFilePath, bool cover = true, bool sourceFileVirtualPath = true, bool destFileVirtualPath = true)
   //     {
   //         if (destFileVirtualPath)
   //         {
   //             destFilePath = HttpContext.Current.Server.MapPath(sourceFilePath);
   //         }
   //         CreateDirectory(destFilePath, false);

   //         string source = sourceFileVirtualPath ? HttpContext.Current.Server.MapPath(sourceFilePath) : sourceFilePath;
   //         if (File.Exists(source))
   //         {
   //             if (cover)
   //             {
   //                 if (File.Exists(destFilePath))
   //                 {
   //                     DeleteFile(destFilePath, false);
   //                 }
   //                 File.Copy(source, destFilePath);
   //             }
   //         }
   //     }

   //     /// <summary>
   //     /// 从源路径移动到目标路径
   //     /// </summary>
   //     /// <param name="sourceFilePath">源文件路径</param>
   //     /// <param name="destFilePath">目标路径</param>
   //     /// <param name="sourceFileVirtualPath">源文件是否为虚拟路径</param>
   //     /// <param name="destFileVirtualPath">目标是否为虚拟路径</param>
   //     public static void MoveFile(string sourceFilePath, string destFilePath, bool sourceFileVirtualPath = true, bool destFileVirtualPath = true)
   //     {
   //         if (destFileVirtualPath)
   //         {
   //             destFilePath = HttpContext.Current.Server.MapPath(sourceFilePath);
   //         }
   //         CreateDirectory(destFilePath, false);

   //         string source = sourceFileVirtualPath ? HttpContext.Current.Server.MapPath(sourceFilePath) : sourceFilePath;
   //         if (File.Exists(source))
   //         {
   //             File.Move(source, destFilePath);
   //         }
   //     }

   //     /// <summary>
   //     /// 删除文件（如果存在的话）
   //     /// </summary>
   //     /// <param name="filePath">路径</param>
   //     /// <param name="virtualPath">是否为虚拟路径，默认为虚拟路径</param>
   //     public static void DeleteFile(string filePath, bool virtualPath = true)
   //     {
   //         string physical = virtualPath ? HttpContext.Current.Server.MapPath(filePath) : filePath;
   //         if (File.Exists(physical))
   //         {
   //             File.Delete(physical);
   //         }
   //     }
   //     #endregion

   //     #region 上传文件主方法
   //     /// <summary>
   //     /// 上传一个文件，如果已存在，则覆盖。<br/>主存放路径由参数path定义，子存放路径及文件名由系统随机生成，生成规范参考<see cref="DateAsPath"/>和<see cref="FileNameWithDateAsPath"/>
   //     /// </summary>
   //     /// <param name="httpPostedFileBase">上传服务器控件</param>
   //     /// <param name="path">主存放路径</param>
   //     /// <param name="fileType">允许上传的文件类型枚举组成的数组</param>
   //     /// <param name="ex">如果出错，可由该输出参数捕获</param>
   //     /// <param name="virtualPath">如果出错，可由该输出参数捕获</param>
   //     /// <returns>如果上传成功，返回生成子存放路径及文件名；<br/>否则返回空字符串</returns>
   //     public static string Upload(HttpPostedFileBase httpPostedFileBase, string path, FileType[] fileType, ref FileHelperException ex, bool virtualPath = true)
   //     {
   //         if (httpPostedFileBase.ContentLength == 0)
   //         {
   //             ex = FileHelperException.FileError;
   //             return "";
   //         }

   //         string name = httpPostedFileBase.FileName;
   //         name = FileNameWithDateAsPath + name.Substring(name.LastIndexOf("."));

   //         if (Upload(httpPostedFileBase, path, name, fileType, ref ex, virtualPath: virtualPath))
   //         {
   //             return name;
   //         }
   //         else
   //             return "";
   //     }


   //     /// <summary>
   //     /// 上传一个文件，路径和文件名分别由path和name确定，是否覆盖由参数cover确定
   //     /// </summary>
   //     /// <param name="fileUpload">上传服务器控件</param>
   //     /// <param name="path">存放路径</param>
   //     /// <param name="name">生成的文件名</param>
   //     /// <param name="cover">如果对应的文件名已存在，是否覆盖</param>
   //     /// <param name="fileType">允许上传的文件类型枚举组成的数组</param>
   //     /// <param name="ex">如果出错，可由该输出参数捕获</param>
   //     /// <returns>返回一个布尔值，以标识本次上传是否成功</returns>
   //     public static bool Upload(HttpPostedFileBase fileUpload, string path, string name, FileType[] fileType, ref FileHelperException ex, bool cover = true, bool virtualPath = true)
   //     {
   //         string originalPath = path + name;
   //         if (virtualPath)
   //         {
   //             originalPath = HttpContext.Current.Server.MapPath(originalPath);
   //         }
   //         CreateDirectory(originalPath, false);

   //         if (!cover)
   //         {
   //             if (File.Exists(originalPath))
   //             {
   //                 ex = FileHelperException.FileExists;
   //                 return false;
   //             }
   //         }

   //         if (!CheckType(fileUpload, fileType))
   //         {
   //             ex = FileHelperException.FileTypeError;
   //             return false;
   //         }

   //         //if (!fileUpload.HasFile) {
   //         //    ex = FileHelperException.FileError;
   //         //    return false;
   //         //}

   //         try
   //         {
   //             if (File.Exists(originalPath))
   //             {
   //                 FileInfo fileInfo = new FileInfo(originalPath);
   //                 fileInfo.Attributes &= ~FileAttributes.ReadOnly;
   //             }

   //             fileUpload.SaveAs(originalPath);

   //             return true;
   //         }
   //         catch (Exception exception)
   //         {
   //             RecordException("~/ErrorLog/FileUpload.txt", exception);
   //             ex = FileHelperException.UploadCauseError;
   //             return false;
   //         }
   //     }
   //     #endregion

   //     #region 检查上传的文件是否在允许上传的文件类型中
   //     /// <summary>
   //     /// 检查上传的文件是否在允许上传的文件类型中
   //     /// </summary>
   //     /// <param name="fileUpload">上传服务器控件</param>
   //     /// <param name="fileType">允许上传的文件类型的枚举组成的数组</param>
   //     /// <returns></returns>
   //     public static bool CheckType(HttpPostedFileBase fileUpload, FileType[] fileType)
   //     {
   //         string Extension = Path.GetExtension(fileUpload.FileName);

   //         string fileclass = "";
   //         Stream myStream = fileUpload.InputStream;
   //         byte[] input = new byte[2];
   //         myStream.Read(input, 0, 2);

   //         for (int i = 0; i < 2; i++)
   //         {
   //             fileclass += input[i].ToString();
   //         }

   //         bool allow = false;
   //         foreach (FileType ft in fileType)
   //         {
   //             if (Extension.Trim('.').ToUpper() == ft.ToString())
   //             {
   //                 allow = true;
   //                 break;
   //             }
   //             if (fileclass == EnumDescription.GetFieldDesc(ft))
   //             {
   //                 allow = true;
   //                 break;
   //             }
   //         }

   //         return allow;
   //     }
   //     #endregion

   //     #region  时间字符串作为文件名
   //     /// <summary>
   //     /// 返回由年月日生成的字符串，以/分隔，如“1900/01-01/”
   //     /// </summary>
   //     public static string DateAsPath
   //     {
   //         get { return String.Format("{0:yyyy}/{0:MM-dd}/", DateTime.Now); }
   //     }

   //     /// <summary>
   //     /// 返回由年月日生成的字符串 + 由时分秒及一位毫秒数生成的字符串，如“1900/01-01/0305609”
   //     /// </summary>
   //     public static string FileNameWithDateAsPath
   //     {
   //         get
   //         {
   //             Thread.Sleep(5);
   //             return FileHelper.DateAsPath + String.Format("{0:HHmmssf}", DateTime.Now);
   //         }
   //     }
   //     #endregion

   //     #region 从虚拟路径或物理路径中获取文件扩展名
   //     /// <summary>
   //     /// 从虚拟路径或物理路径中获取文件扩展名
   //     /// </summary>
   //     /// <param name="path">虚拟路径或物理路径</param>
   //     /// <returns>文件扩展名</returns>
   //     public static string GetExtension(string path)
   //     {
   //         if (path.IndexOf(":") > -1)
   //         {
   //             return Path.GetExtension(path);
   //         }
   //         else
   //         {
   //             return VirtualPathUtility.GetExtension(path);
   //         }
   //     }
   //     #endregion

   //     #region 创建路径中所有不存在的目录
   //     /// <summary>
   //     /// 创建路径中所有不存在的目录,最后一级文件夹不能有句点(.)
   //     /// </summary>
   //     /// <param name="filePath">路径</param>
   //     /// <param name="virtualPath">是否是虚拟路径</param>
   //     public static void CreateDirectory(string filePath, bool virtualPath = true)
   //     {
   //         if (virtualPath)
   //         {
   //             filePath = HttpContext.Current.Server.MapPath(filePath);
   //         }

   //         string folderPath = "";
   //         string[] filePaths = filePath.Split(new string[] { "/", "\\" }, StringSplitOptions.None);

   //         foreach (string folderName in filePaths)
   //         {

   //             if (folderName.Contains(":"))
   //             {
   //                 //跳过磁盘节点
   //                 folderPath += folderName + "/";
   //                 continue;
   //             }

   //             if (!folderName.Contains(".") || folderName != filePaths.Last())
   //             {
   //                 folderPath += folderName + "/";
   //                 try
   //                 {
   //                     if (!Directory.Exists(folderPath))
   //                     {
   //                         Directory.CreateDirectory(folderPath);
   //                     }
   //                 }
   //                 catch (Exception ex)
   //                 {
   //                     RecordException(HttpContext.Current.Server.MapPath("/") + "/FileHelper-error.txt", ex, false);
   //                 }
   //             }
   //         }

   //     }
   //     #endregion

   //     #region 生成缩略图

   //     /// <summary>
   //     /// 生成缩略图
   //     /// </summary>
   //     /// <param name="source">源图物理路径</param>
   //     /// <param name="newFilePath">新虚拟路径(含文件名)</param>
   //     /// <param name="tpType">缩放类型</param>
   //     /// <param name="width">缩放至宽度</param>
   //     /// <param name="height">缩放至高度</param>
   //     /// <returns></returns>
   //     public static bool ThumbnailPic(string source, string newFilePath, ThumbnailPicType tpType, int width, int height, bool virtualPath = true)
   //     {
   //         System.Drawing.Image originalImage = System.Drawing.Image.FromFile(source);

   //         int x = 0;
   //         int y = 0;
   //         int ow = originalImage.Width;
   //         int oh = originalImage.Height;

   //         switch (tpType)
   //         {
   //             case ThumbnailPicType.Width:
   //                 height = originalImage.Height * width / originalImage.Width;
   //                 break;
   //             case ThumbnailPicType.Height:
   //                 width = originalImage.Width * height / originalImage.Height;
   //                 break;
   //             case ThumbnailPicType.Cut:
   //                 if ((double)ow / ((double)oh) > ((double)width / (double)height))
   //                 {
   //                     oh = originalImage.Height;
   //                     ow = originalImage.Height * width / height;
   //                     y = 0;
   //                     x = (originalImage.Width - ow) / 2;
   //                 }
   //                 else
   //                 {
   //                     ow = originalImage.Width;
   //                     oh = originalImage.Width * height / width;
   //                     x = 0;
   //                     y = (originalImage.Height - oh) / 2;
   //                 }
   //                 break;
   //             case ThumbnailPicType.Auto:
   //             default:
   //                 if ((double)ow / (double)width > (double)oh / height)
   //                 {
   //                     height = oh * width / ow;
   //                 }
   //                 else
   //                 {
   //                     width = ow * height / oh;
   //                 }
   //                 break;
   //         }

   //         //新建一个bmp图片
   //         System.Drawing.Image bitmap = new Bitmap(width, height);

   //         //新建一个画板
   //         Graphics g = Graphics.FromImage(bitmap);

   //         //设置高质量插值法
   //         g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

   //         //设置高质量,低速度呈现平滑程度
   //         g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

   //         //清空画布并以透明背景色填充
   //         g.Clear(Color.Transparent);
   //         //在指定位置并且按指定大小绘制原图片的指定部分
   //         g.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

   //         try
   //         {
   //             CreateDirectory(newFilePath, virtualPath);
   //             if (virtualPath)
   //             {
   //                 newFilePath = HttpContext.Current.Server.MapPath(newFilePath);
   //             }

   //             string extName = GetExtension(source);

   //             switch (extName.ToLower())
   //             {
   //                 case ".jpg":
   //                     bitmap.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
   //                     //bitmap.Save(newPath + newName, System.Drawing.Imaging.ImageFormat.Jpeg);
   //                     break;
   //                 case ".gif":
   //                     bitmap.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Gif);
   //                     break;
   //                 case ".png":
   //                     bitmap.Save(newFilePath, System.Drawing.Imaging.ImageFormat.Png);
   //                     break;
   //             }
   //             //outthumbnailPath = newPath;
   //         }
   //         catch (System.Exception e)
   //         {
   //             throw e;
   //         }
   //         finally
   //         {
   //             originalImage.Dispose();
   //             bitmap.Dispose();
   //             g.Dispose();
   //         }
   //         return true;
   //     }
   //     #endregion

   //     #region 将错误信息写入指定文件
   //     /// <summary>
   //     /// 将错误信息写入指定文件，物理路径不执行创建目录的操作
   //     /// </summary>
   //     /// <param name="filePath"></param>
   //     /// <param name="ex"></param>
   //     /// <param name="virtualPath">是否虚拟路径</param>
   //     public static void RecordException(string filePath, Exception ex, bool virtualPath = true)
   //     {
   //         if (virtualPath) CreateDirectory(filePath);

   //         RecordInnerException(filePath, ex, virtualPath);
   //     }

   //     private static void RecordInnerException(string filePath, Exception ex, bool virtualPath = true)
   //     {
   //         if (ex.InnerException == null)
   //         {
   //             if (virtualPath) filePath = HttpContext.Current.Server.MapPath(filePath);
   //             using (FileStream fs = File.Open(filePath, FileMode.OpenOrCreate | FileMode.Append))
   //             {
   //                 StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
   //                 sw.WriteLine("ERROR:");
   //                 sw.WriteLine(ex.GetType().ToString());
   //                 sw.WriteLine(ex.Message);
   //                 sw.WriteLine("发生时间：" + DateTime.Now.ToString());
   //                 sw.WriteLine("");
   //                 sw.Close();
   //             }
   //         }
   //         else
   //         {
   //             RecordInnerException(filePath, ex.InnerException, virtualPath);
   //         }
   //     }
   //     #endregion
   // }
}
