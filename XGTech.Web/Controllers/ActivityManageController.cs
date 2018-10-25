using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using SqlSugar;
using StarNet.Multimedia.IO.Helper.Excels;
using XGTech.Model;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Model.ResponseModel;
using XGTech.Web.Database;

namespace XGTech.Web.Controllers
{
    public class ActivityManageController : Controller
    {
        private readonly IExcelWriter _excelWriter;
        public ActivityManageController(IExcelWriter excelWriter)
        {
            _excelWriter = excelWriter;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateActivity()
        {
            return View();
        }

        public IActionResult EditActivity()
        {
            return View();
        }

        public IActionResult CodeImage()
        {
            return View();
        }

        public IActionResult CopyActivity()
        {
            return View();
        }

        public IActionResult ActivityInfo()
        {
              return View();
        }

    /// <summary>
    /// 活动列表 检索
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public IActionResult GetList(ActivityManageListRequestModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var pageIndex = model.page;
            var pageSize = model.limit;
            var list = db.Queryable<View_Activity>();
            if (!string.IsNullOrWhiteSpace(model.title))
                list = list.Where(p => p.Title.Contains(model.title));
            if (model.AType.HasValue)
            {
                if (model.AType.Value != -1)
                    list = list.Where(p => p.AType == model.AType);
            }

            if (model.Status.HasValue)
            {
                if (model.Status.Value != -1)
                    list = list.Where(p => p.Status == model.Status);
            }

            if (model.datetype == 1)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                    list = list.Where(p => SqlFunc.Between(p.AddTime, model.startTime.Value, model.endTime.Value + " 23:59:59"));
            }

            if (model.datetype == 2)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                    list = list.Where(p => SqlFunc.Between(p.StartDate, model.startTime.Value, model.endTime.Value + " 23:59:59") && SqlFunc.Between(p.EndDate, model.startTime.Value, model.endTime.Value + " 23:59:59"));
            }

            var totalCount = 0;
            List<View_Activity> listR = list.OrderBy(it => it.ID, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount);
         
            var Resoult = new LayUIBaseJsonResoult()
            {
                data = listR,
                count = totalCount,
                code = "0",
                msg = "获取成功"
            };
            return Json(Resoult);
        }

        /// <summary>
        /// 获取奖品名称
        /// </summary>
        /// <returns></returns>
        public IActionResult GetPrizeSettingList()
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var list = db.Queryable<Tbl_PrizeSetting, Tbl_Dict>((st, sc) => new object[] {
        JoinType.Left,st.PType==sc.DKey}).Where((st, sc) => sc.DType == "奖品类型")
      .Select((st, sc) => new { PDetails = st.PDetails, ID = st.ID, PName = st.PName, Pworth = st.Pworth, TypeName = sc.DValue }).ToList();

            var Resoult = new LayUIBaseJsonResoult()
            {
                data = list,
                count = list.Count(),
                code = "0",
                msg = "获取成功"
            };
            return Json(Resoult);
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult InserActivity(InsertActivityRequestModel model)
        {
            try
            {
                SqlSugarClient db = DBHelper.GetInstance();

                Tbl_Activity activity = new Tbl_Activity();
                activity.Number = "100000";
                activity.EndDate = model.EndDate.Value;
                activity.EntTime = model.EntTime;
                activity.StartDate = model.StartDate;
                activity.StartTime = model.StartTime;
                activity.Status = 1;
                activity.AType = model.AType;
                activity.Title = model.Title;
                activity.DistributionScore = model.DistributionScore;
                activity.ConsumeScore = model.ConsumeScore;
                activity.BannerImg = model.BannerImg;
                activity.BGImg = model.BGImg;
                activity.ARule = model.ARule;
                activity.LotteryNumber = model.LotteryNumber;
                activity.AddTime = DateTime.Now;
                activity.PriorityUse = model.PriorityUse;
                int ID = db.Ado.SqlQuerySingle<int>(" insert into  Tbl_Activity (Number,Title,AType,StartDate,EndDate,StartTime,EntTime,Status,BGImg,BannerImg,ConsumeScore,DistributionScore,ARule,PriorityUse,AddTime,LotteryNumber) " +
                    "values ( @Number ,@Title,@AType,@StartDate,@EndDate,@StartTime,@EntTime,@Status,@BGImg,@BannerImg,@ConsumeScore,@DistributionScore,@ARule,@PriorityUse,getdate(),@LotteryNumber); select @@identity as ID; ", new SugarParameter[]{
             new SugarParameter("@Number",activity.Number),
             new SugarParameter("@Title",activity.Title),
             new SugarParameter("@AType",activity.AType),
             new SugarParameter("@StartDate",activity.StartDate.Value),
             new SugarParameter("@EndDate",activity.EndDate.Value),
             new SugarParameter("@StartTime",activity.StartTime.ToString()),
             new SugarParameter("@EntTime",activity.EntTime.ToString()),
             new SugarParameter("@Status",activity.Status),
             new SugarParameter("@BGImg",activity.BGImg),
             new SugarParameter("@BannerImg",activity.BannerImg),
             new SugarParameter("@ConsumeScore",activity.ConsumeScore),
             new SugarParameter("@DistributionScore",activity.DistributionScore),
             new SugarParameter("@ARule",activity.ARule),
             new SugarParameter("@PriorityUse",activity.PriorityUse),
             new SugarParameter("@LotteryNumber",activity.LotteryNumber)
             });

                db.Ado.SqlQuerySingle<int>("update Tbl_Activity set Number=RIGHT(100000 + Cast(id as varchar), 6) where id=" + ID);

                //int ID = db.Insertable<Tbl_Activity>(activity).ExecuteReturnIdentity();
                string[] strs1 = model.ATypes.Split(',');
                string[] strs2 = model.PNames.Split(',');
                string[] strs3 = model.Inventorys.Split(',');
                string[] strs4 = model.Probabilitys.Split(',');
                if (strs1.Length != 9)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "奖品设置错误!"
                    };
                    return Json(Resoult);
                }

                for (int j = 0; j < strs1.Length - 1; j++)
                {
                    Tbl_Activity_PrizeSetting tb = new Tbl_Activity_PrizeSetting();
                    tb.Awards = strs1[j];
                    tb.Inventory = int.Parse(strs3[j]);
                    tb.Probability = decimal.Parse((decimal.Parse(strs4[j]) / decimal.Parse("100")).ToString());
                    tb.PrizeID = int.Parse(strs2[j]);
                    tb.ActivityID = ID;
                    db.Insertable<Tbl_Activity_PrizeSetting>(tb).ExecuteCommand();
                }
                if (ID > 0)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "1",
                        msg = "保存成功"
                    };
                    return Json(Resoult);
                }
                else
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "保存失败"
                    };
                    return Json(Resoult);
                }
            }
            catch (Exception x)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = x.Message.ToString()
                };
                return Json(Resoult);
            }
        }

       


        /// <summary>
        /// 停止活动
        /// </summary>
        /// <returns></returns>
        public IActionResult StopActivity(StopActivityModel model)
        {
            try
            {
                //and Status=3;
                SqlSugarClient db = DBHelper.GetInstance();
                for (int i = 0; i < model.ActivityID.Split(',').Length - 1; i++)
                {
                    db.Ado.ExecuteCommand("update Tbl_Activity set Status=4 where ID=@ID   ", new SugarParameter[]{
                                          new SugarParameter("@ID",model.ActivityID.Split(',')[i])
                                      });
                }
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "1",
                    msg = "停止活动成功"
                };
                return Json(Resoult);
            }
            catch (Exception x)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = x.Message.ToString()
                };
                return Json(Resoult);
            }
        }

        public IActionResult ReleaseActivity(StopActivityModel model)
        {
            try
            {
                SqlSugarClient db = DBHelper.GetInstance();
                db.Ado.ExecuteCommand("update Tbl_Activity set Status=2 where ID=@ID and Status=1;  ", new SugarParameter[]{
                                          new SugarParameter("@ID",model.ActivityID)
                                      });
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "1",
                    msg = "发布成功"
                };
                return Json(Resoult);
            }
            catch (Exception x)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = x.Message.ToString()
                };
                return Json(Resoult);
            }
        }

        /// <summary>
        /// 查询单条活动信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult SingleActivityInfo(UpdateActivityModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var list = db.Queryable<Tbl_Activity>();
            List<Tbl_Activity> listR = list.Where(p => p.ID == model.ActivityID).ToList();
            if (listR.Count == 0)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "当前编辑的数据不存在"
                };
                return Json(Resoult);
            }
            else
            {
                LayUIBaseJsonResoult Resoult = new LayUIBaseJsonResoult();
                foreach (Tbl_Activity item in listR)
                {
                    List<Tbl_Activity_PrizeSetting> Prizelist = db.Queryable<Tbl_Activity_PrizeSetting>().Where(p => p.ActivityID == item.ID).OrderBy(it => it.ID).ToList();
                    var re = new
                    {
                        ID = item.ID,
                        Number = item.Number,
                        Title = item.Title,
                        AType = item.AType,
                        StartDate = item.StartDate.Value.ToString("yyyy-MM-dd"),
                        EndDate = item.EndDate.Value.ToString("yyyy-MM-dd"),
                        StartTime = item.StartTime,
                        EntTime = item.EntTime,
                        Status = item.Status,
                        BGImg = item.BGImg,
                        BannerImg = item.BannerImg,
                        ConsumeScore = item.ConsumeScore,
                        DistributionScore = item.DistributionScore,
                        ARule = item.ARule,
                        PriorityUse = item.PriorityUse,
                        Activity_PrizeSetting = Prizelist,
                        LotteryNumber = item.LotteryNumber
                    };
                    Resoult = new LayUIBaseJsonResoult()
                    {
                        data = re,
                        code = "1",
                        msg = "获取成功"
                    };

                }
                return Json(Resoult);
            }
        }

        /// <summary>
        /// 编辑活动
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateActivity(UpdateActivityRequestModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var list = db.Queryable<Tbl_Activity>();
            List<Tbl_Activity> listR = list.Where(p => p.ID == model.ActivityID).ToList();
            if (listR.Count == 0)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = "当前编辑的数据不存在"
                };
                return Json(Resoult);
            }
            else
            {
                Tbl_Activity activity = new Tbl_Activity();
                activity.ID = model.ActivityID;
                activity.EndDate = model.EndDate.Value;
                activity.EntTime = model.EntTime;
                activity.StartDate = model.StartDate.Value;
                activity.StartTime = model.StartTime;

                activity.Title = model.Title;
                activity.DistributionScore = model.DistributionScore;
                activity.ConsumeScore = model.ConsumeScore;
                activity.BannerImg = model.BannerImg;
                activity.BGImg = model.BGImg;
                activity.ARule = model.ARule;
                activity.LotteryNumber = model.LotteryNumber;
                activity.PriorityUse = model.PriorityUse;
                db.Ado.SqlQuerySingle<int>("update Tbl_Activity set BGImg='" + activity.BGImg + "',BannerImg='" + activity.BannerImg + "', StartDate='" + activity.StartDate + "',EndDate='" + activity.EndDate + "',Title='" + activity.Title + "',StartTime='" + activity.StartTime + "',EntTime='" + activity.EntTime + "',DistributionScore='" + activity.DistributionScore + "',ConsumeScore='" + activity.ConsumeScore + "',ARule='" + activity.ARule + "',PriorityUse='" + activity.PriorityUse + "',LotteryNumber='" + activity.LotteryNumber + "' where ID='" + activity.ID + "'");

                string[] strs0 = model.PrizeID.Split(',');
                string[] strs1 = model.ATypes.Split(',');
                string[] strs2 = model.PNames.Split(',');
                string[] strs3 = model.Inventorys.Split(',');
                string[] strs4 = model.Probabilitys.Split(',');
                if (strs1.Length != 9)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "奖品设置错误!"
                    };
                    return Json(Resoult);
                }

                for (int j = 0; j < strs1.Length - 1; j++)
                {
                    Tbl_Activity_PrizeSetting tb = new Tbl_Activity_PrizeSetting();
                    tb.ID = int.Parse(strs0[j]);
                    tb.Awards = strs1[j];
                    tb.Inventory = int.Parse(strs3[j]);
                    tb.Probability = decimal.Parse((decimal.Parse(strs4[j]) / decimal.Parse("100")).ToString());
                    tb.PrizeID = int.Parse(strs2[j]);
                    tb.ActivityID = model.ActivityID;
                    db.Updateable<Tbl_Activity_PrizeSetting>(tb).ExecuteCommand();
                }

                var Resoult1 = new LayUIBaseJsonResoult()
                {
                    code = "1",
                    msg = "编辑成功"
                };
                return Json(Resoult1);
            }
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> Export(ActivityManageListRequestModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var list = db.Queryable<Tbl_Activity>();
            if (!string.IsNullOrWhiteSpace(model.title))
                list = list.Where(p => p.Title.Contains(model.title));
            if (model.AType.HasValue)
            {
                if (model.AType.Value != -1)
                    list = list.Where(p => p.AType == model.AType);
            }

            if (model.Status.HasValue)
            {
                if (model.Status.Value != -1)
                    list = list.Where(p => p.Status == model.Status);
            }

            if (model.datetype == 1)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                    list = list.Where(p => SqlFunc.Between(p.AddTime, model.startTime.Value, model.endTime.Value));
            }

            if (model.datetype == 2)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                    list = list.Where(p => SqlFunc.Between(p.StartDate, model.startTime.Value, model.endTime.Value));
            }
            string[] fmts = { "c", "g", "G", @"hh\:mm\:ss", "%m' min.'" };
            List<ActivityManageListResponseModel> listR = await list.OrderBy(p => p.ID, OrderByType.Desc).Select(p => new ActivityManageListResponseModel
            {
                AddTime = p.AddTime.Value,
                Number = p.Number,
                Title = p.Title,
                StartTime = p.StartTime.ToString(),
                EntTime = p.EntTime.ToString(),
                AType = p.AType.ToString(),
                Status = p.Status.Value.ToString()
            }).ToListAsync();
            for (int i = 0; i < listR.Count; i++)
            {
                listR[i].AType = GetAType(int.Parse(listR[i].AType));
                listR[i].Status = GetStatus(int.Parse(listR[i].Status));
            }

            String result = await _excelWriter.ListToExcel(listR);

            return Json(new { code = 0, msg = "导出成功!", url = Request.GetAbsoluteUri() + "/Admin/DownloadExcel/Excel?excelToken=" + result + "&fileName=活动列表" });
        }

        /// <summary>
        /// 复制重开
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public IActionResult CopyReopen(UpdateActivityRequestModel model)
        {
            try
            {
                SqlSugarClient db = DBHelper.GetInstance();

                Tbl_Activity activity = new Tbl_Activity();
                activity.Number = "100000";
                activity.EndDate = model.EndDate.Value;
                activity.EntTime = model.EntTime;
                activity.StartDate = model.StartDate;
                activity.StartTime = model.StartTime;
                activity.Status = 1;
                activity.AType = model.AType;
                activity.Title = model.Title;
                activity.DistributionScore = model.DistributionScore;
                activity.ConsumeScore = model.ConsumeScore;
                activity.BannerImg = model.BannerImg;
                activity.BGImg = model.BGImg;
                activity.ARule = model.ARule;
                activity.AddTime = DateTime.Now;
                activity.PriorityUse = model.PriorityUse;
                activity.LotteryNumber = model.LotteryNumber;
                int ID = db.Ado.SqlQuerySingle<int>(" insert into  Tbl_Activity (Number,Title,AType,StartDate,EndDate,StartTime,EntTime,Status,BGImg,BannerImg,ConsumeScore,DistributionScore,ARule,PriorityUse,AddTime,LotteryNumber) " +
                    "values ( @Number ,@Title,@AType,@StartDate,@EndDate,@StartTime,@EntTime,@Status,@BGImg,@BannerImg,@ConsumeScore,@DistributionScore,@ARule,@PriorityUse,getdate(),@LotteryNumber); select @@identity as ID; ", new SugarParameter[]{
             new SugarParameter("@Number",activity.Number),
             new SugarParameter("@Title",activity.Title),
             new SugarParameter("@AType",activity.AType),
             new SugarParameter("@StartDate",activity.StartDate.Value),
             new SugarParameter("@EndDate",activity.EndDate.Value),
             new SugarParameter("@StartTime",activity.StartTime.ToString()),
             new SugarParameter("@EntTime",activity.EntTime.ToString()),
             new SugarParameter("@Status",activity.Status),
             new SugarParameter("@BGImg",activity.BGImg),
             new SugarParameter("@BannerImg",activity.BannerImg),
             new SugarParameter("@ConsumeScore",activity.ConsumeScore),
             new SugarParameter("@DistributionScore",activity.DistributionScore),
             new SugarParameter("@ARule",activity.ARule),
             new SugarParameter("@PriorityUse",activity.PriorityUse),
             new SugarParameter("@LotteryNumber",activity.LotteryNumber),

});
                db.Ado.SqlQuerySingle<int>("update Tbl_Activity set Number=RIGHT(100000 + Cast(id as varchar), 6) where id=" + ID);

                //int ID = db.Insertable<Tbl_Activity>(activity).ExecuteReturnIdentity();
                string[] strs1 = model.ATypes.Split(',');
                string[] strs2 = model.PNames.Split(',');
                string[] strs3 = model.Inventorys.Split(',');
                string[] strs4 = model.Probabilitys.Split(',');
                if (strs1.Length != 9)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "奖品设置错误!"
                    };
                    return Json(Resoult);
                }

                for (int j = 0; j < strs1.Length - 1; j++)
                {
                    Tbl_Activity_PrizeSetting tb = new Tbl_Activity_PrizeSetting();
                    tb.Awards = strs1[j];
                    tb.Inventory = int.Parse(strs3[j]);
                    tb.Probability = decimal.Parse((decimal.Parse(strs4[j]) / decimal.Parse("100")).ToString());
                    tb.PrizeID = int.Parse(strs2[j]);
                    tb.ActivityID = ID;
                    db.Insertable<Tbl_Activity_PrizeSetting>(tb).ExecuteCommand();
                }
                if (ID > 0)
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "1",
                        msg = "复制重开成功"
                    };
                    return Json(Resoult);
                }
                else
                {
                    var Resoult = new LayUIBaseJsonResoult()
                    {
                        code = "0",
                        msg = "复制重开失败"
                    };
                    return Json(Resoult);
                }
            }
            catch (Exception x)
            {
                var Resoult = new LayUIBaseJsonResoult()
                {
                    code = "0",
                    msg = x.Message.ToString()
                };
                return Json(Resoult);
            }
        }


        public string GetStatus(int status)
        {
            string returnValue = string.Empty;
            if (status == 1)
                returnValue = "未发布";
            if (status == 2)
                returnValue = "未开始";
            if (status == 3)
                returnValue = "进行中";
            if (status == 4)
                returnValue = "已结束";
            return returnValue;
        }

        public string GetAType(long type)
        {
            string returnValue = string.Empty;
            if (type == 1)
                returnValue = "幸运转转乐";
            if (type == 2)
                returnValue = "幸运开火车";
            return returnValue;
        }

        /// <summary>
        /// 二维码生成
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult GetCodeImage(StopActivityModel model)
        {
            Bitmap bitmap = GetQRCode((Request.GetAbsoluteUri() + "/Default/index?id=" + model.ActivityID), 150);
            var Resoult = new LayUIBaseJsonResoult()
            {

                data = Bitmap2Byte(bitmap),
                code = "1",
                msg = Request.GetAbsoluteUri() + "/Default/index?id=" + model.ActivityID
            };
            return Json(Resoult);
        }

        public byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
        public Bitmap GetQRCode(string url, int pixel)
        {
            QRCodeGenerator generator = new QRCodeGenerator();
            QRCodeData codeData = generator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M, true);
            QRCode qrcode = new QRCode(codeData);
            Bitmap qrImage = qrcode.GetGraphic(pixel);
            return qrImage;
        }
    }
}