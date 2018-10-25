using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using XGTech.Model.Model;
using XGTech.Model.ResponseModel;
using XGTech.Web.Common;
using XGTech.Web.Database;

namespace XGTech.Web.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 判断活动状态
        /// </summary>
        /// <returns></returns>
        public IActionResult ConditionActiveStatus(Int64 id)
        {
            var db = DBHelper.GetInstance();

            Tbl_Activity activityModel = db.Queryable<Tbl_Activity>().Where(t1 => t1.ID == id).First();

            if (activityModel.Status == 1)
            {
                return Json(new { code = "1", msg = "活动未发布!" });
            }

            if (activityModel.Status == 2)
            {
                return Json(new { code = "2", msg = "活动未开始!" });
            }

            if (activityModel.Status == 4)
            {
                return Json(new { code = "4", msg = "活动已结束!" });
            }

            return Json(new { code = "0", msg = "ok" });
        }

        /// <summary>
        /// 活动主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(Int64 id)
        {
            //todo:
            //判断该活动状态是否启用
            //判断该活动是否在指定有效期内
            var db = DBHelper.GetInstance();

            Tbl_Activity activityModel = db.Queryable<Tbl_Activity>().Where(t1 => t1.ID == id).First();

            if (activityModel == null)
            {
                return Content("不存在该活动!");
            }

            _httpContextAccessor.HttpContext.Session.SetString("activeID", activityModel.ID.ToString());

            string userid = CookiesHelper.GetCookies("login" + id);

            if (userid != "")
            {
                //查询该用户此活动抽奖次数
                int count = db.Queryable<Tbl_AwardRecord>().Where(t1 => t1.ActivityID == id && t1.UserID == long.Parse(userid) && t1.PType != 6).Count();

                //查询该活动抽奖的次数
                Tbl_Activity activity = db.Queryable<Tbl_Activity>().Where(t1 => t1.ID == id).Single();
                //剩余次数
                ViewBag.CJCountText = "我的剩余抽奖次数:" + (activity.LotteryNumber - count);
            }


            //添加访问记录
            Tbl_Visit tbl_Visit = new Tbl_Visit()
            {
                activity_id = id,
                createtime = DateTime.Now,
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            db.Insertable<Tbl_Visit>(tbl_Visit).ExecuteCommand();

            return View(activityModel);
        }

        /// <summary>
        /// 获取奖品选项
        /// </summary>
        /// <returns></returns>
        public List<String> GetPrizeItem(int id)
        {
            SqlSugarClient db = DBHelper.GetInstance();

            var query = db.Queryable<Tbl_Activity_PrizeSetting, Tbl_PrizeSetting>((s1, s2) => new object[] { JoinType.Left, s1.PrizeID == s2.ID })
                .Where((s1) => s1.ActivityID == id).OrderBy((s1, s2) => s2.ID).Select((s1, s2) => new { s2.PName }).ToList();

            List<String> list = new List<string>();

            query.ForEach(s => list.Add(s.PName));

            return list;
        }

        /// <summary>
        /// 判断抽奖次数
        /// </summary>
        /// <returns></returns>
        public IActionResult ConditionCount()
        {
            string activeID = _httpContextAccessor.HttpContext.Session.GetString("activeID");

            //判断用户是否登录
            string userid = CookiesHelper.GetCookies("login" + activeID);


            if (userid == "")
            {
                return Json(new { code = -1, data = "/Login/Index?id=" + activeID });
            }

            SqlSugarClient db = DBHelper.GetInstance();

            //查询该用户活动抽奖次数
            int count = db.Queryable<Tbl_AwardRecord>().Where(t1 => t1.ActivityID == long.Parse(activeID) && t1.UserID == long.Parse(userid)).Count();
            //查询该活动抽奖的次数
            Tbl_Activity activity = db.Queryable<Tbl_Activity>().Where(t1 => t1.ID == long.Parse(activeID)).Single();

            if (count >= activity.LotteryNumber)
            {
                return Json(new { code = 0, data = "" });
            }

            return Json(new { code = 1, data = "" });
        }

        /// <summary>
        /// 根据概率计算出获奖奖品
        /// </summary>
        /// <returns></returns>
        public IActionResult GetProb()
        {
            string activeID = _httpContextAccessor.HttpContext.Session.GetString("activeID");

            //判断用户是否登录
            string userid = CookiesHelper.GetCookies("login" + activeID);

            SqlSugarClient db = DBHelper.GetInstance();
            var PrizeList = db.Queryable<Tbl_Activity_PrizeSetting, Tbl_PrizeSetting>((s1, s2) => new object[] { JoinType.Left, s1.PrizeID == s2.ID })
            .Where(s1 => s1.ActivityID == long.Parse(activeID))
            .OrderBy((s1, s2) => s2.ID).Select((s1, s2) =>
               new { s1.Probability, s2.ID, s2.PType, s2.PRule, s1.Inventory, ApsID = s1.ID, s2.Pworth }).ToList();

            List<float> probList = new List<float>();

            List<AwardRecordProbResponseModel> AwardRecordList = new List<AwardRecordProbResponseModel>();

            PrizeList.ForEach(p =>
            {
                probList.Add((float)p.Probability);
                AwardRecordList.Add(new AwardRecordProbResponseModel()
                {
                    ID = p.ID,
                    Probability = p.Probability,
                    PType = p.PType,
                    PRule = p.PRule,
                    Inventory = p.Inventory,
                    ApsID = p.ApsID,
                    Pworth = p.Pworth.HasValue ? p.Pworth.Value : 0
                });
            });


            //概率计算
            int index = ProbHelper.Get(probList.ToArray());

            //插入获奖记录
            Tbl_AwardRecord model = new Tbl_AwardRecord()
            {
                ActivityID = long.Parse(activeID),
                Address = "",
                AddTime = DateTime.Now,
                BillCode = "",
                ExchangeTime = null,
                IsPrize = AwardRecordList[index].PType == 7 ? 0 : 1, //如果是谢谢参与,则设置未中奖
                PrizeID = AwardRecordList[index].ID,
                ShippingStatus = 0,
                UserID = long.Parse(userid),
                UserName = CommonHelper.GetUserName(long.Parse(userid)),
                Status =0, //AwardRecordList[index].PType != 4 ? 1 : 0,
                Worth = AwardRecordList[index].Pworth,
                PType = AwardRecordList[index].PType
            };

            //计算库存
            if (AwardRecordList[index].Inventory <= 0)
            {
                //找到类型为谢谢惠顾的选项
                for (int i = 0; i < AwardRecordList.Count; i++)
                {
                    if (AwardRecordList[i].PType == 7)
                    {
                        index = i;
                    }
                }

                return Json(new { code = 0, msg = AwardRecordList[index].PRule, prizeIndex = index });
            }

            //更新库存
            Tbl_Activity_PrizeSetting tapModel = new Tbl_Activity_PrizeSetting()
            {
                ID = AwardRecordList[index].ApsID,
                Inventory = AwardRecordList[index].Inventory - 1
            };
            db.Updateable(tapModel).UpdateColumns(t1 => new { t1.Inventory }).ExecuteCommand();


            long recordID = db.Insertable<Tbl_AwardRecord>(model).ExecuteReturnBigIdentity();
            return Json(new { code = 1, recordID = recordID, prizeIndex = index, msg = AwardRecordList[index].PRule, type = AwardRecordList[index].PType });
        }
    }
}