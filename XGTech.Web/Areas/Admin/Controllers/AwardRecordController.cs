using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Web.Database;

namespace XGTech.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AwardRecordController : BaseController
    {

        SqlSugarClient db = DBHelper.GetInstance();

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetList(AwardRecordPagesRequstModel model)
        {
            var totalCount = 0;

            var query = db.Queryable<Tbl_AwardRecord, Tbl_Activity, Tbl_PrizeSetting, Tbl_Dict>((t1, t2, t3, t4) => new object[]
             {
                JoinType.Left,t1.ActivityID==t2.ID,
                JoinType.Left,t1.PrizeID==t3.ID,
                JoinType.Left,t3.PType==t4.DKey && t4.DType=="奖品类型"
             });

            if (model.StartDate.HasValue)
            {
                query = query.Where((t1, t2) => t2.StartDate>=model.StartDate);
            }

            if (model.EndDate.HasValue)
            {
                query = query.Where((t1, t2) => t2.EndDate <= model.EndDate.Value.AddDays(1));
            }

            if (!String.IsNullOrWhiteSpace(model.BillCode))
            {
                query = query.Where((t1, t2) => t1.BillCode==model.BillCode);
            }


            if (!String.IsNullOrWhiteSpace(model.PName))
            {
                query = query.Where((t1, t2,t3,t4) => t3.PName.Contains(model.PName));
            }

            if (model.PType.HasValue)
            {
                query = query.Where((t1, t2, t3, t4) => t3.PType==model.PType);
            }

            if (model.ShippingStatus.HasValue)
            {
                query = query.Where((t1, t2, t3, t4) => t1.ShippingStatus==model.ShippingStatus);
            }

            if (model.Status.HasValue)
            {
                query = query.Where((t1, t2, t3, t4) => t1.Status == model.Status);
            }

            if (!String.IsNullOrWhiteSpace(model.Title))
            {
                query = query.Where((t1, t2, t3, t4) => t2.Title.Contains(model.Title));
            }

            if (!String.IsNullOrWhiteSpace(model.UserName))
            {
                query = query.Where((t1, t2, t3, t4) => t1.UserName.Contains(model.UserName));
            }

            var list=query.Select((t1, t2, t3, t4) => new { t1.ID,t2.StartDate, t2.EndDate, t1.AddTime, t2.Number, t2.Title, t1.UserName, PTypeName = t4.DValue, t3.PName, t3.PDetails, t3.Pworth, t1.Status, t1.ExchangeTime, t1.ShippingStatus,t1.Address, t1.BillCode }).OrderBy((t1)=>t1.AddTime,OrderByType.Desc).ToPageList(model.page, model.limit,ref totalCount);

            return Json(new { code=0,msg="",data=list,count= totalCount });
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <returns></returns>
        public IActionResult Deliver(int id)
        {
            var list=db.Queryable<Tbl_Dict>().Where(t1=>t1.DType== "快递类型").ToList();

            ViewData["type"] = list;
            ViewData["id"] = id;

            var model=db.Queryable<Tbl_AwardRecord>().Single(it=>it.ID==id);

            if (model!=null)
            {
                ViewData["BillCode"] = model.BillCode;
                ViewData["Express"] =model.Express;
            }

            

            return View();
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Deliver(DeliverViewModel model)
        {
            Tbl_AwardRecord arecord = new Tbl_AwardRecord()
            {
                BillCode=model.BillCode,
                ID=model.ID,
                Express=model.Express,
                ShippingStatus=1
            };

            db.Updateable(arecord).UpdateColumns(it=> new{ it.BillCode,it.Express,it.ShippingStatus }).ExecuteCommand();

            return Json(new { code=0 });
        }


        public IActionResult Tab()
        {
            return View();
        }

    }
}