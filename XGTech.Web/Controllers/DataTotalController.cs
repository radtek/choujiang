using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using XGTech.Model;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Web.Database;

namespace XGTech.Web.Controllers
{
    public class DataTotalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 数据统计 检索
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult GetList(DataTotalRequestModel model)
        {
            SqlSugarClient db = DBHelper.GetInstance();
            var list = db.Queryable<Tbl_DataTotal>();
            if (!string.IsNullOrWhiteSpace(model.Title))
                list = list.Where(p => p.activity_title.Contains(model.Title));

            if (model.startTime.HasValue && model.endTime.HasValue)
                list = list.Where(p => SqlFunc.Between(p.startDate, model.startTime.Value, model.endTime.Value));
            var pageIndex = model.page;
            var pageSize = model.limit;
            var totalCount = 0;
            var listR = list.OrderBy(p => p.startDate, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount);
            var Resoult = new LayUIBaseJsonResoult()
            {
                data = listR,
                count = totalCount,
                code = "0",
                msg = "获取成功"
            };
            return Json(Resoult);
        }
    }
}