using AmsApp.Dto;
using AmsApp.Models;
using AmsApp.Data;
using AmsApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.CompilerServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AmsApp.Controllers
{
    [Authorize]
    [Route("OBCall/")]
    public class OBCallController : Controller
    {
        private readonly AMSContext context;
        private readonly ILogger<OBCallController> logger;

        public OBCallController(AMSContext context, ILogger<OBCallController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var empId = Extensions.GetEmployeeId(this);
            var spSql = $"EXECUTE dbo.GetOBLeadByEmpId {empId}";
            var lead = context.OBLeads.FromSqlRaw(spSql).ToList().FirstOrDefault();
            if (lead == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            else
            {
                return View(lead);
            }
        }

        [HttpGet("OnCall/{leadId}")]
        public IActionResult OnCall(int leadId)
        {
            var spSql = $"EXECUTE dbo.GetOBLeadById {leadId}";
            var lead = context.OBLeads.FromSqlRaw(spSql).ToList().FirstOrDefault();

            return View(lead);
        }

        [HttpGet("OnCallNC/{leadId}")]
        public IActionResult OnCallNC(int leadId)
        {
            var spSql = $"EXECUTE dbo.GetOBLeadById {leadId}";
            var lead = context.OBLeads.FromSqlRaw(spSql).ToList().FirstOrDefault();
            ViewBag.Date = lead.NextCallDate?.ToString("yyyy-MM-dd");
            return View(lead);
        }

        [HttpGet("OnCallApp/{leadId}")]
        public IActionResult OnCallApp(int leadId)
        {
            var spSql = $"EXECUTE dbo.GetOBLeadById {leadId}";
            var lead = context.OBLeads.FromSqlRaw(spSql).ToList().FirstOrDefault();
            ViewBag.Date = lead.AppointmentDate?.ToString("yyyy-MM-dd");
            return View(lead);
        }

        [HttpPost("SaveCall")]
        public async Task<IActionResult> SaveCall(OBLead lead)
        {
            var responceMessage = string.Empty;
            var source = context.OBLeads.Where(p => p.Id == lead.Id).FirstOrDefault();
            var userId = Extensions.GetUserId(this);
            lead.EndTime=DateTime.Now;
            try
            {
                if (source == null)
                {
                    throw new ApplicationException("Invalid request");
                }
                else
                {
                    source.ModifiedBy = userId;
                    source.ModifiedOn = DateTime.Now;
                    source.Name = lead.Name;
                    source.Age = lead.Age;
                    source.Gender=lead.Gender;
                    //source.Email = lead.Email;
                    source.Address = lead.Address;
                    //source.City = lead.City;
                    //source.Country = lead.Country;
                    //source.State = lead.State;
                    //source.Pin=lead.Pin;
                    source.MainDisease=lead.MainDisease;
                    source.SubDisease = lead.SubDisease;
                    source.ClinicBranch = lead.ClinicBranch;
                    //source.Notes = lead.Notes;
                    //source.OnHold = lead.OnHold;
                    source.AppointmentDate = lead.AppointmentDate;
                    source.NextCallDate = lead.NextCallDate; 
                    source.Disposition = lead.Disposition;
                    source.LastCalledBy = Extensions.GetEmployeeId(this);
                    source.LastCallOn = DateTime.Now;
                    await context.SaveChangesAsync();

                    OBCallHistory history = new OBCallHistory();
                    history.Disposition = lead.Disposition;
                    history.NextCallDate = lead.NextCallDate;
                    history.CreatedOn= DateTime.Now;
                    history.LeadId = lead.Id;
                    history.AgentId = Extensions.GetEmployeeId(this);
                    history.CallDate = DateTime.Today;
                    history.StartTime = lead.StartTime;
                    history.EndTime = lead.EndTime;
                    history.Duration = Convert.ToInt32((lead.EndTime - lead.StartTime).TotalSeconds);

                    context.OBCallHistories.Add(history);
                    await context.SaveChangesAsync();

                    if(lead.SaveAndClose)
                    {
                        responceMessage = "Done";
                    }
                    else
                    {
                        // next number
                        var empId = Extensions.GetEmployeeId(this);
                        var spSql = $"EXECUTE dbo.GetOBLeadByEmpId {empId}";
                        var newlead = context.OBLeads.FromSqlRaw(spSql).ToList().FirstOrDefault();
                        if (newlead == null)
                        {
                            responceMessage = "Done";
                        }
                        else
                        {
                            responceMessage = newlead.Id.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                responceMessage = $"Error:{ex.Message}";
            }
            return this.Ok(responceMessage);
        }

        //[HttpGet("OnHold")]
        //public IActionResult OnHold()
        //{
        //    var empId = Extensions.GetEmployeeId(this);
        //    var strSql = $"select Id, Mobile, Name, Disposition, NextCallDate from OBLeads where OnHold =1 and AllocatedAgentId={empId}";
        //    var data= context.SqlQuery<CallDto>(strSql);
        //    return View(data);
        //}

        [HttpGet("NextCallDate/{calldate}")]
        public IActionResult NextCallDate(string calldate)
        {
            if (string.IsNullOrEmpty(calldate)) calldate = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.Date= calldate;
            var empId = Extensions.GetEmployeeId(this);
            var strSql = $"select Id, Mobile, Name, Disposition, NextCallDate from OBLeads where AllocatedAgentId={empId} and PatientId is null and NextCallDate = '{calldate}' ";
            var data = context.SqlQuery<CallDto>(strSql);
            return View(data);
        }

        [HttpGet("Appointments/{calldate}")]
        public IActionResult Appointments(string calldate)
        {
            if (string.IsNullOrEmpty(calldate)) calldate = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.Date = calldate;

            DateTime fdate = Convert.ToDateTime(calldate);
            DateTime tdate = fdate.AddDays(1);
            var empId = Extensions.GetEmployeeId(this);
            var strSql = $"select Id, Mobile, Name, Disposition, NextCallDate from OBLeads where AllocatedAgentId={empId} and PatientId is null and AppointmentDate between '{fdate.Date.ToString("yyyy-MM-dd")}' and '{tdate.Date.ToString("yyyy-MM-dd")}' ";
            var data = context.SqlQuery<CallDto>(strSql);
            return View(data);
        }

        [HttpGet("Visited/{calldate}")]
        public IActionResult Visited(string calldate)
        {
            if (string.IsNullOrEmpty(calldate)) calldate = DateTime.Today.ToString("yyyy-MM-dd");
            ViewBag.Date = calldate;
            var empId = Extensions.GetEmployeeId(this);
            DateTime fdate = Convert.ToDateTime(calldate);
            DateTime tdate = fdate.AddDays(1);
            var strSql = $"select Id, Mobile, Name, Disposition, NextCallDate from OBLeads where AllocatedAgentId={empId} and PatientId is not null and AppointmentDate between '{fdate.Date.ToString("yyyy-MM-dd")}' and '{tdate.Date.ToString("yyyy-MM-dd")}'";
            var data = context.SqlQuery<CallDto>(strSql);
            return View(data);
        }
    }
}
