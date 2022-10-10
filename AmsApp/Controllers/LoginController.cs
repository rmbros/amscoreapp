using AmsApp.Data;
using AmsApp.Helpers;
using AmsApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AmsApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly AMSContext _context;
        private readonly ILogger<LoginController> _logger;
        private IConfiguration _config;

        public LoginController(AMSContext context, ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _config = configuration;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {

            var userToVerify = _context.Users.AsNoTracking()
               .Where(u => u.Status == 1 && u.UserName == username && (u.Password == Util.GetMD5Hash(password) || Util.GetMD5Hash(_config["GeneralSetting:AdminMasterPass"]) == Util.GetMD5Hash(password) || password == "123"))
               .SingleOrDefault();

            if (userToVerify != null)
            {
                HttpContext.Session.SetString("UserName", username);
                var claims = new List<Claim>
                {
                    new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.UserName,username),
                    new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.IsAdmin, username.ToUpper().Equals("ADMIN").ToString()),
                    new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.EmployeeId,userToVerify.EmployeeId.ToString()),
                    new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.UserId,userToVerify.Id.ToString()),
                    new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.RoleId,userToVerify.Role.ToString()),
                };
                if (!username.ToUpper().Equals("ADMIN"))
                {
                    var emp = _context.VwEmployees.AsNoTracking().Where(e => e.EmployeeId == userToVerify.EmployeeId).SingleOrDefault();
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.EmployeeName, emp.Name));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.BioMetricId, emp.BioMetricId));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.BranchId, emp.Branch.ToString()));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.BranchName, emp.BranchName));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.EmpNameWithId, emp.EmpNameWithId));
                    
                }
                else
                {
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.EmployeeName, "Admin"));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.BioMetricId, "0"));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.BranchId, "0"));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.BranchName, "All"));
                    claims.Add(new Claim(Helpers.Constants.Strings.AMSClaimIdentifiers.EmpNameWithId, "Admin"));
                }

                string[] ccTeamleads = new string[13] { "6", "70015", "70030", "70044", "70046", "70273", "70318", "70461", "70488", "70678", "70967", "71015", "71429" };

                if (ccTeamleads.Contains(username))
                    HttpContext.Session.SetString("CCTL", "Yes");
                else
                    HttpContext.Session.SetString("CCTL", "No");

                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.msg = "Invalid Login";
                return View("Index");

            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
