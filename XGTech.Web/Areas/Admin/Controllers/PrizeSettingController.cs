using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using StarNet.Multimedia.IO.Helper.Excels;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Model.ResponseModel;
using XGTech.Web.Database;

namespace XGTech.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PrizeSettingController : BaseController
    {
        private readonly IExcelWriter _excelWriter;
        private readonly IMapper _mapper;

        public PrizeSettingController(IMapper mapper, IExcelWriter excelWriter)
        {
            _mapper = mapper;
            _excelWriter = excelWriter;
        }


        public IActionResult Index()
        {
            var db = DBHelper.GetInstance();

            List<Tbl_Dict> ptypeList = db.Queryable<Tbl_Dict>().Where(t1 => t1.DType == "奖品类型").ToList();

            ViewData["ptypeList"] = ptypeList;

            return View();
        }


        public IActionResult List(PrizeSettingRequestModel model)
        {
            var db = DBHelper.GetInstance();

            var body = db.Queryable<Tbl_PrizeSetting, Tbl_Dict>((t1, t2) => new object[] { JoinType.Left, t1.PType == t2.DKey && t2.DType == "奖品类型" });

            if (!String.IsNullOrWhiteSpace(model.PName))
            {
                body.Where(t1 => t1.PName.Contains(model.PName));
            }

            if (model.PType.HasValue)
            {
                body.Where(t1 => t1.PType == model.PType);
            }

            var totalCount = 0;

            var list = body.OrderBy(t1 => t1.ID, OrderByType.Desc).Select((t1, t2) => new { t1.ID, t1.PDetails, t1.PImg, t1.PName, t1.PRule, t1.PType, t1.Pworth, PTypeName = t2.DValue,AddTime=t1.AddTime, OperName=t1.OperName }).ToPageList(model.Page, model.Limit, ref totalCount);

            var obj = new { code = 0, msg = "", count = totalCount, data = list };

            return Json(obj);
        }


        public IActionResult Delete(int[] ids)
        {
            var db = DBHelper.GetInstance();

            db.Deleteable<Tbl_PrizeSetting>().In(ids).ExecuteCommand();

            return Json(new { code = 0, msg = "ok" });
        }


        public IActionResult Add()
        {
            var db = DBHelper.GetInstance();

            var list = db.Queryable<Tbl_Dict>().Where(t1 => t1.DType == "奖品类型").ToList();

            ViewBag.PrizeTypeList = list;

            return View();
        }

        [HttpPost]
        public IActionResult Add(PrizeSettingRequestModel model)
        {
            var db = DBHelper.GetInstance();

            if(db.Queryable<Tbl_PrizeSetting>().Any(t1=>t1.PName==model.PName))
            {
                return Json(new { code = 1, msg = "奖品选项名称已存在!" });
            }

            if (model.Pworth.HasValue)
            {
                model.Pworth = Math.Round(model.Pworth.Value, 2, MidpointRounding.AwayFromZero);
            }

            model.OperName = userInfo.emp_no;
            model.OperID = userInfo.user_id;


            db.Insertable<Tbl_PrizeSetting>(model).ExecuteCommand();

            var obj = new { code = 0, msg = "添加成功!" };

            return Json(obj);
        }

        public IActionResult Edit(long id)
        {
            var db = DBHelper.GetInstance();

            var list = db.Queryable<Tbl_Dict>().Where(t1 => t1.DType == "奖品类型").ToList();

            ViewBag.PrizeTypeList = list;

            var model = db.Queryable<Tbl_PrizeSetting>().Where(it => it.ID == id).Single();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PrizeSettingRequestModel model)
        {
            var db = DBHelper.GetInstance();

            var prizeModel = db.Queryable<Tbl_PrizeSetting>().Single(t1 => t1.PName == model.PName);


            if (prizeModel!=null && prizeModel.ID != model.ID)
            {
                return Json(new { code = 1, msg = "奖品选项名称已存在!" });
            }

            if (model.Pworth.HasValue)
            {
                model.Pworth = Math.Round(model.Pworth.Value, 2, MidpointRounding.AwayFromZero);
            }

            var entity = _mapper.Map<PrizeSettingRequestModel, Tbl_PrizeSetting>(model);

            db.Updateable(entity).IgnoreColumns(t1=>new { t1.AddTime }).ExecuteCommand();

            var obj = new { code = 0, msg = "添加成功!" };

            return Json(obj);
        }

        public IActionResult ShowImage(String url)
        {
            return View("ShowImage", url);
        }


        public async Task<IActionResult> Export(PrizeSettingRequestModel model)
        {

            var db = DBHelper.GetInstance();

            var body = db.Queryable<Tbl_PrizeSetting, Tbl_Dict>((t1, t2) => new object[] { JoinType.Left, t1.PType == t2.DKey && t2.DType == "奖品类型" });

            if (!String.IsNullOrWhiteSpace(model.PName))
            {
                body.Where(t1 => t1.PName.Contains(model.PName));
            }

            if (model.PType.HasValue)
            {
                body.Where(t1 => t1.PType == model.PType);
            }

            var list=await body.Select((t1,t2)=>  new PrizeSettingExportResponseModel()
            {
                PDetails=t1.PDetails,
                PName=t1.PName,
                PRule=t1.PRule,
                PType=t2.DValue,
                Pworth=t1.Pworth
            }).ToListAsync();
            
            String result = await _excelWriter.ListToExcel(list);

            return Json(new { code = 0, msg = "导出成功!", url = Request.GetAbsoluteUri() + "/Admin/DownloadExcel/Excel?excelToken=" + result + "&fileName=奖品列表" });
        }

    }
}