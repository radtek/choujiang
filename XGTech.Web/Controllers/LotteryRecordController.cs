using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using StarNet.Multimedia.IO.Helper.Excels;
using XGTech.Model;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Model.ResponseModel;
using XGTech.Web.Database;

namespace XGTech.Web.Controllers
{
    public class LotteryRecordController : Controller
    {
        private readonly IExcelWriter _excelWriter;
        public LotteryRecordController(IExcelWriter excelWriter)
        {
            _excelWriter = excelWriter;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Total()
        {
            return View();
        }

        /// <summary>
        /// 获取抽奖记录统计
        /// </summary>
        /// <returns></returns>
        public IActionResult GetTotalList(LotteryTotalListRequestModel model)
        {
            model.page = model.page - 1;
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(model.title))
            {
                where += " and b.Title like '%" + model.title + "%'";
            }
            if (model.startTime.HasValue && model.endTime.HasValue)
            {
                DateTime endtime = DateTime.Parse(model.endTime.Value.ToString().Split(' ')[0] + " 23:59:59");
                where += " and a.AddTime between '" + model.startTime.Value + "' and '" + endtime + "'";
            }
            SqlSugarClient db = DBHelper.GetInstance();
            string sql = @" SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY ActivityID DESC) 
 AS rowid,* FROM 
 (select (select Number from Tbl_Activity where ID=a.ActivityID) as number,
(select Title from Tbl_Activity where ID=a.ActivityID) as title,
(select isnull(cell,' ') from Tbl_user where id=a.UserID) as username,
ActivityID,UserID,count(UserID) as lottery_count,
(select count(id) as count from [Tbl_AwardRecord] where IsPrize=1 and UserID=a.UserID and ActivityID=a.ActivityID) as win_count
,(select sum(c.Pworth) as Pworth from  [dbo].[Tbl_AwardRecord] b inner join Tbl_PrizeSetting c on b.PrizeID=c.ID where IsPrize=1 and UserID=a.UserID and ActivityID=a.ActivityID  ) as totalworth
 from [dbo].[Tbl_AwardRecord] a  inner join Tbl_Activity b  on a.ActivityID = b.ID where  1=1  " + where + @"  group by a.UserID,a.ActivityID) orders
 ) AS t
 WHERE t.rowid > " + (model.limit * model.page) + " AND t.rowid <= " + (model.limit * (model.page + 1)) + "; ";
            sql += @"select count(c.UserID) as  totalCount from 
 (select UserID  from[dbo].[Tbl_AwardRecord] a inner join Tbl_Activity b on a.ActivityID = b.ID
  where 1 = 1 " + where + @" group by a.UserID, a.ActivityID) c";
            DataSet ds = db.Ado.GetDataSetAll(sql);
            List<LotteryTotalListResponseModel> list = new List<LotteryTotalListResponseModel>();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                LotteryTotalListResponseModel mo = new LotteryTotalListResponseModel();
                mo.title = item["title"].ToString();
                mo.number = item["number"].ToString();
                mo.win_count = int.Parse(item["win_count"].ToString());
                mo.lottery_count = int.Parse(item["lottery_count"].ToString());
                mo.username = item["username"].ToString();
                mo.totalworth = decimal.Parse(item["totalworth"].ToString());
                list.Add(mo);
            }
            var totalCount = 0;
            foreach (DataRow item in ds.Tables[1].Rows)
            {
                totalCount = int.Parse(item["totalCount"].ToString());
            }
            var Resoult = new LayUIBaseJsonResoult()
            {
                data = list,
                count = totalCount,
                code = "0",
                msg = "获取成功"
            };
            return Json(Resoult);
        }

        public async Task<IActionResult> ExportTotalList(LotteryTotalListRequestModel model)
        {
            string where = string.Empty;
            if (!string.IsNullOrWhiteSpace(model.title))
                where += " and b.title like '%" + model.title + "%'";
            if (model.startTime.HasValue && model.endTime.HasValue)
            {
                DateTime endtime = DateTime.Parse(model.endTime.Value.ToString().Split(' ')[0] + " 23:59:59");
                where += " and a.AddTime between '" + model.startTime.Value + "' and '" + endtime + "'";
            }
            SqlSugarClient db = DBHelper.GetInstance();
            string sql = @"  select (select Number from Tbl_Activity where ID=a.ActivityID) as number,
(select Title from Tbl_Activity where ID=a.ActivityID) as title,
(select isnull(cell,' ') from Tbl_user where id=a.UserID) as username,
ActivityID,UserID,count(UserID) as lottery_count,
(select count(id) as count from [Tbl_AwardRecord] where IsPrize=1 and UserID=a.UserID and ActivityID=a.ActivityID) as win_count
,(select sum(c.Pworth) as Pworth from  [dbo].[Tbl_AwardRecord] b inner join Tbl_PrizeSetting c on b.PrizeID=c.ID where IsPrize=1 and UserID=a.UserID and ActivityID=a.ActivityID  ) as totalworth
 from [dbo].[Tbl_AwardRecord] a inner join Tbl_Activity b  on a.ActivityID = b.ID  where  1=1  " + where + @"   group by a.UserID,a.ActivityID  order by a.ActivityID desc ";
            // sql += " select count(UserID) as totalCount  from [dbo].[Tbl_AwardRecord] a where  1=1  " + where + " group by a.UserID,a.ActivityID";
            DataSet ds = db.Ado.GetDataSetAll(sql);
            List<LotteryTotalListResponseModel> list = new List<LotteryTotalListResponseModel>();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                LotteryTotalListResponseModel mo = new LotteryTotalListResponseModel();
                mo.title = item["title"].ToString();
                mo.number = item["number"].ToString();
                mo.win_count = int.Parse(item["win_count"].ToString());
                mo.lottery_count = int.Parse(item["lottery_count"].ToString());
                mo.username = item["username"].ToString();
                mo.totalworth = decimal.Parse(item["totalworth"].ToString());
                list.Add(mo);
            }
            if (list.Count > 0)
            {
                String result = await _excelWriter.ListToExcel(list);
                return Json(new { code = 0, msg = "导出成功!", url = Request.GetAbsoluteUri() + "/Admin/DownloadExcel/Excel?excelToken=" + result + "&fileName=抽奖记录统计" });
            }
            else
            {
                return Json(new
                {
                    code = 1,
                    msg = "暂无数据",
                });
            }
        }


        /// <summary>
        /// 活动列表 检索
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult GetList(LotteryRecordListRequestModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var list = db.Queryable<Tbl_AwardRecord, Tbl_Activity, Tbl_PrizeSetting>((a, b, c) => new object[] {
                JoinType.Inner,a.ActivityID==b.ID,
                   JoinType.Inner,a.PrizeID==c.ID
            });
            if (!string.IsNullOrWhiteSpace(model.Title))
                list = list.Where((a, b, c) => b.Title.Contains(model.Title));

            if (!string.IsNullOrWhiteSpace(model.UserName))
                list = list.Where((a, b, c) => a.UserName.Contains(model.UserName));

            if (model.datetype == 1)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                {
                    DateTime endtime = DateTime.Parse(model.endTime.Value.ToString().Split(' ')[0] + " 23:59:59");
                    list = list.Where((a, b, c) => SqlFunc.Between(a.AddTime, model.startTime.Value, endtime));
                }
            }
            if (model.datetype == 2)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                {
                    DateTime endtime = DateTime.Parse(model.endTime.Value.ToString().Split(' ')[0] + " 23:59:59");
                    list = list.Where((a, b, c) => SqlFunc.Between(b.AddTime, model.startTime.Value, endtime));
                }
            }
            var pageIndex = model.page;
            var pageSize = model.limit;
            var totalCount = 0;
            var listR = list.OrderBy((a, b, c) => a.AddTime, OrderByType.Desc).Select((a, b, c) => new { Number = b.Number, Title = b.Title, AddTime = a.AddTime, UserName = a.UserName, PName = c.PName, Pworth = c.Pworth })
            .ToPageList(pageIndex, pageSize, ref totalCount);

            var Resoult = new LayUIBaseJsonResoult()
            {
                data = listR,
                count = totalCount,
                code = "0",
                msg = "获取成功"
            };
            return Json(Resoult);
        }

        public async Task<IActionResult> Export(LotteryRecordListRequestModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var list = db.Queryable<Tbl_AwardRecord, Tbl_Activity, Tbl_PrizeSetting>((a, b, c) => new object[] {
                JoinType.Inner,a.ActivityID==b.ID,
                   JoinType.Inner,a.PrizeID==c.ID
            });
            if (!string.IsNullOrWhiteSpace(model.Title))
                list = list.Where((a, b, c) => b.Title.Contains(model.Title));

            if (!string.IsNullOrWhiteSpace(model.UserName))
                list = list.Where((a, b, c) => a.UserName.Contains(model.UserName));

            if (model.datetype == 1)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                    list = list.Where((a, b, c) => SqlFunc.Between(a.AddTime, model.startTime.Value, model.endTime.Value));
            }

            if (model.datetype == 2)
            {
                if (model.startTime.HasValue && model.endTime.HasValue)
                    list = list.Where((a, b, c) => SqlFunc.Between(b.AddTime, model.startTime.Value, model.endTime.Value));
            }

            var listR = await list.OrderBy((a, b, c) => a.AddTime, OrderByType.Desc).Select((a, b, c) => new LotteryRecordListResponseModel
            {
                Number = b.Number,
                Title = b.Title,
                AddTime = a.AddTime,
                UserName = a.UserName,
                PName = c.PName,
                Pworth = c.Pworth.Value
            }).ToListAsync();

            String result = await _excelWriter.ListToExcel(listR);

            return Json(new { code = 0, msg = "导出成功!", url = Request.GetAbsoluteUri() + "/Admin/DownloadExcel/Excel?excelToken=" + result + "&fileName=抽奖记录" });
        }


        public IActionResult tab()
        {

            return View();
        }
    }
}