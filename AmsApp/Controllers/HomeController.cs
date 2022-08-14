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

        [HttpGet("GetLookUpData")]
        public JsonResult GetLookUpData()
        {
            LookUpDto data = new()
            {
                Disposition = LookUpTable.GetCCDispositions(_db),
                Gender = LookUpTable.GetGender(_db),
                ClinicBranch = LookUpTable.GetClinicBranchs(_db),
                MainDisease = LookUpTable.GetMainDiseases(_db),
                SubDisease = LookUpTable.GetSubDiseases(_db),
                City = LookUpTable.GetCities(_db),
                State = LookUpTable.GetStates(_db),
                Country = LookUpTable.GetCountries(_db)
            };

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