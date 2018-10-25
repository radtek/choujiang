using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using StarNet.Multimedia.IO.Helper.Excels;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Web.Database;

namespace XGTech.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AwardStatController : BaseController
    {
        SqlSugarClient db = DBHelper.GetInstance();

        private readonly IExcelWriter _excelWriter;

        public AwardStatController(IExcelWriter excelWriter)
        {
            _excelWriter = excelWriter;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List(AwardRecordPagesRequstModel model)
        {
            List<AwardStatExportResponseModel> list = null;
            int recordCount;
            GetDataList(model, out list, out recordCount);

            return Json(new { code = 0, data = list, msg = "", count = recordCount });
        }

        public async Task<IActionResult> Export(AwardRecordPagesRequstModel model)
        {
            model.page = 1;
            model.limit = int.MaxValue;

            List<AwardStatExportResponseModel> list = null;
            int recordCount;
            GetDataList(model, out list, out recordCount);

            String result = await _excelWriter.ListToExcel(list);

            return Json(new { code = 0, msg = "导出成功!", url = Request.GetAbsoluteUri() + "/Admin/DownloadExcel/Excel?excelToken=" + result + "&fileName=奖品列表" });
        }

        private void GetDataList(AwardRecordPagesRequstModel model, out List<AwardStatExportResponseModel> list, out int recordCount)
        {
            List<SugarParameter> parList = new List<SugarParameter>()
            {
                  new SugarParameter("@page",((model.page - 1) * model.limit + 1)),
                  new SugarParameter("@limit", model.page * model.limit)
            };

            StringBuilder sql = new StringBuilder(@"
            select * from(select ROW_NUMBER() over(order by id desc) as num, ID,StartDate, EndDate, Number, Title,
            isnull((select count(1) from[dbo].[Tbl_AwardRecord] where ActivityID = t1.id), 0) as cjTime, --抽奖次数
            isnull((select count(distinct userid) from[dbo].[Tbl_AwardRecord] where ActivityID = t1.id), 0) as cjCount,--抽奖人数
            isnull((select count(distinct userid) from[dbo].[Tbl_AwardRecord] where ActivityID = t1.id and IsPrize = 1),0) as zjCount, --中奖人数
            isnull((select count(distinct userid) from[dbo].[Tbl_AwardRecord] where ActivityID = t1.id and[Status] = 1),0) as djcount, --兑奖人数
            isnull((select sum(worth) from[dbo].[Tbl_AwardRecord] where ActivityID = t1.id),0) as djja --兑奖价值
             from [dbo].[Tbl_Activity] t1 where (1=1) ");

            if (model.StartDate.HasValue)
            {
                sql.Append(" and StartDate>=@StartDate");
                parList.Add(new SugarParameter("@StartDate", model.StartDate));
            }
            if (model.EndDate.HasValue)
            {
                sql.Append(" and EndDate<=@EndDate");
                parList.Add(new SugarParameter("@EndDate", model.EndDate));
            }
            if (!String.IsNullOrEmpty(model.Title))
            {
                sql.Append(" and Title like @Title");
                parList.Add(new SugarParameter("@Title", "%" + model.Title + "%"));
            }

            sql.Append(" ) as t where num between @page and @limit ");
         
            list = db.Ado.SqlQuery<AwardStatExportResponseModel>(sql.ToString(), parList);
            recordCount = db.Queryable<Tbl_Activity>().Count();
        }
    }
}