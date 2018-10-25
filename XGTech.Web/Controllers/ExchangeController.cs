using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XGTech.Model.Model;
using XGTech.Model.RequestModel;
using XGTech.Web.Database;

namespace XGTech.Web.Controllers
{
    public class ExchangeController : Controller
    {
        public IActionResult Index(int id)
        {
            ViewBag.ID = id;
            return View();
        }


        /// <summary>
        /// 获取省、市、区
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public IActionResult GetArea(int id)
        {
            var db = DBHelper.GetInstance();

            var list = db.Queryable<Tbl_China>().Where(t1 => t1.Pid == id).Where(t1 => t1.Id != 0).ToList();

            return Json(list);
        }


        /// <summary>
        /// 兑换
        /// </summary>
        /// <returns></returns>
        public IActionResult PostExchange(AwardRecordExchangeRequstModel model)
        {
            var db = DBHelper.GetInstance();

            string p = "";


            var province = db.Queryable<Tbl_China>().Where((t1) => t1.Id == int.Parse(model.province)).First();

            if (province != null)
            {
                p += province.Name;
            }

            var city = db.Queryable<Tbl_China>().Where((t1) => t1.Id == int.Parse(model.city)).First();

            if (city != null)
            {
                p += city.Name;
            }

            if (model.county != "0")
            {
                var county = db.Queryable<Tbl_China>().Where((t1) => t1.Id == int.Parse(model.county)).First();

                if (county != null)
                {
                    p += county.Name;
                }
            }

            Tbl_AwardRecord entity = new Tbl_AwardRecord()
            {
                ID = model.id,
                ExchangeTime = DateTime.Now,
                Status = 1,
                Address = p + model.address,
                Phone = model.phone,
                Recipients = model.recipients,
            };

            db.Updateable(entity).UpdateColumns(t1 => new { t1.ExchangeTime, t1.Status, t1.Address, t1.Phone, t1.Recipients }).ExecuteCommand();


            return Content("ok");
        }

    }
}