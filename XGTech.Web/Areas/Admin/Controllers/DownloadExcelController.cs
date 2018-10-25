using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StarNet.Multimedia.IO.Helper.Excels;
using System;
using System.Threading.Tasks;

namespace SupplyHub_API.Controllers
{
    [Area("Admin")]
    public class DownloadExcelController : Controller
    {
        private readonly IExcelReader _excelReader;
        public DownloadExcelController(IExcelReader excelReader)
        {
            _excelReader = excelReader;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Excel(string excelToken, string fileName)
        {
            return File(await _excelReader.GetExcel(excelToken), "application/vnd.ms-excel", $"{fileName}{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }
    }
}