using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XGTech.Web.Common;

namespace XGTech.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            String s=SiteConfig.GetSite("DefaultConnection");
            return Content(s);
        }
    }
}