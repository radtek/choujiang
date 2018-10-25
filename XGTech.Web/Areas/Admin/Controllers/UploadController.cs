using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XGTech.Model.ResponseModel;

namespace XGTech.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _env;
        public UploadController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult UpLoadFile()
        {
            var files = Request.Form.Files;

            var now = DateTime.Now;
            var webRootPath = _env.WebRootPath;
            var filePath = @"/Uploads/Images/";

            if (!Directory.Exists(webRootPath + filePath))
            {
                try
                {
                    Directory.CreateDirectory(webRootPath + filePath);
                }
                catch
                {
                    return Json(new { code = 0, msg = "无法创建服务端{@root/Uploads/Images/}目录，请确认是否有操作权限！" });
                }
            }
            try
            {
                if (files.Count() > 0)
                {
                    List<UploadImageModel> list = new List<UploadImageModel>();
                    foreach (var uploadfile in files)
                    {
                        //文件后缀
                        var fileExtension = Path.GetExtension(uploadfile.FileName);

                        long fileSize = uploadfile.Length; //获得文件大小，以字节为单位
                        var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                        var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                        var saveName = strDateTime + strRan + fileExtension;
                        //检查文件扩展名是否合法
                        if (!CheckFileExt(fileExtension))
                        {
                            return Json(new { code = 0, msg = "不允许上传" + fileExtension + "类型的文件", result = "no", message = "不允许上传" + fileExtension + "类型的文件" });
                        }
                        //检查文件大小是否合法
                        if (!CheckFileSize(fileSize))
                        {
                            return Json(new { code = 0, msg = "文件超过限制的大小" , result = "no", message = "文件超过限制的大小" });
                        }

                        using (FileStream fs = System.IO.File.Create(webRootPath + filePath + saveName))
                        {
                            uploadfile.CopyTo(fs);
                            fs.Flush();
                        }

                        list.Add(new UploadImageModel
                        {
                            filename = uploadfile.FileName,
                            url = $"{Request.Scheme}://{Request.Host}" + filePath.Replace("\\", "/") + saveName,
                        });
                    }
                    return Json(new { code = 1, msg = "上传成功", datas = list, result = "ok", message = "上传成功" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, msg = ex.Message, result = "no", message = "上传失败" });
                throw;
            }
            return Json(new { code = 0, msg = "上传失败", result = "no" ,message= "上传失败" });
        }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string _fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "ashx", "asa", "asmx", "asax", "php", "jsp", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == _fileExt.ToLower())
                {
                    return false;
                }
            }
            //检查合法文件
            string[] allowExt = (".gif,.jpg,.png,.bmp,.rar,.zip,.doc,.xls,.txt,.jpeg,.png").Split(',');
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i].ToLower() == _fileExt.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckFileSize(long _fileSize)
        {
            if (_fileSize > 1024 * 1024 * 10)
            {
                return false;
            }
            return true;
        }
    }
}