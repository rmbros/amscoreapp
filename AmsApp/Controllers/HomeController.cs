using AmsApp.Dto;
using AmsApp.Data;
using AmsApp.Helpers;
using AmsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AmsApp.Controllers
{
    [Authorize]
    [Route("Home/")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AMSContext _db;

        public HomeController(AMSContext dbContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var empId = Extensions.GetEmployeeId(this);
            var spSql = $"EXECUTE dbo.GetOBLeadsCountByEmpId {empId}";
            var lead = _db.LeadCountDtos.FromSqlRaw(spSql).ToList().FirstOrDefault();
            
            return View(lead);
        }

        [HttpGet("MIndex")]
        public IActionResult MIndex()
        {
            var empId = Extensions.GetEmployeeId(this);
            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); 
            var toDate = DateTime.Now.Date;

            var spSql = $"EXECUTE dbo.GetAgentMonthCallSummary {empId}, '{fromDate.ToString("yyyy-MM-dd")}', '{toDate.ToString("yyyy-MM-dd")}'";

            var data = _db.OBAgentDaySummary.FromSqlRaw(spSql)
                                           .ToList().FirstOrDefault();

            var totalTime = 0;
            DateTime startTime = DateTime.Now.Date.AddHours(9).AddMinutes(30);
            DateTime dayend = DateTime.Now.Date.AddHours(18).AddMinutes(30);
            DateTime endTime = dayend.Subtract(DateTime.Now).TotalSeconds < 0 ? dayend : DateTime.Now;
            TimeSpan span = endTime.Subtract(startTime);
            if (fromDate == DateTime.Now.Date)
            {
                totalTime = Convert.ToInt32(span.TotalMinutes);
                if (endTime.Hour >= 14) totalTime = totalTime - 30; //30min lunch break
            }
            else if (fromDate < DateTime.Now.Date)
            {
                //Today's time
                totalTime = Convert.ToInt32(span.TotalMinutes);
                if (endTime.Hour >= 14) totalTime = totalTime - 30; //30min lunch break
                //TimeBefore Today
                span = toDate.Subtract(fromDate);
                totalTime += Convert.ToInt32(span.TotalDays) * 510;
            }

            data.TimeWasted = totalTime - data.Duration;

            return View(data);
        }


        [HttpGet("GetLookUpData")]
        public JsonResult GetLookUpData()
        {
            LookUpDto data = new()
            {
                Disposition = LookUpTable.GetOBDispositions(_db),
                Gender = LookUpTable.GetGender(_db),
                ClinicBranch = LookUpTable.GetClinicBranchs(_db),
                MainDisease = LookUpTable.GetMainDiseases(_db),
                SubDiseases = LookUpTable.GetSubDiseases(_db)
            };
            //City = LookUpTable.GetCities(_db),
            //State = LookUpTable.GetStates(_db),
            //Country = LookUpTable.GetCountries(_db)
            return Json(data);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}