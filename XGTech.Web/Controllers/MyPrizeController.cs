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
    /// <summary>
    /// 我的奖品
    /// </summary>
    public class MyPrizeController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyPrizeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 根据用户获取奖品列表
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string activeID = _httpContextAccessor.HttpContext.Session.GetString("activeID");

            string userid = CookiesHelper.GetCookies("login"+ activeID);

            if (userid == "")
            {
                return Redirect("/login/index?id="+ activeID);
            }

            //用户id
            long uid = long.Parse(userid);

            var db = DBHelper.GetInstance();

            List<AwardRecordPrizeResponseModel> list = db.Queryable<Tbl_AwardRecord, Tbl_PrizeSetting>((t1, t2) => new object[] { JoinType.Left, t1.PrizeID == t2.ID })
            .Where(t1 => t1.UserID == uid).Where(t1 => t1.IsPrize == 1 && t1.PType!=6).OrderBy(t1 => t1.AddTime, OrderByType.Desc)
            .Select((t1, t2) => 
            new AwardRecordPrizeResponseModel() { ID = t1.ID, AddTime = t1.AddTime, PDetails = t2.PDetails, PImg = t2.PImg, PName = t2.PName,PTYpe=t2.PType, Status=t1.Status.Value })
            .ToList();


            return View(list);
        }


      
    }
}