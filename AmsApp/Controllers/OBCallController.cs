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
            if(lead.StartTime < DateTime.Today) lead.StartTime = DateTime.Now;
            lead.EndTime = DateTime.Now;
            try
            {
                if (source == null)
                {
                    throw new ApplicationException("Invalid request");
                }
                else
                {
                    if (!context.OBCallHistories.Any(o => o.LeadId == lead.Id && o.AgentId == Extensions.GetEmployeeId(this) &&  o.CallDate == DateTime.Today))
                    {
                        source.ModifiedBy = userId;
                        source.ModifiedOn = DateTime.Now;
                        source.Name = lead.Name;
                        source.Age = lead.Age;
                        source.Gender = lead.Gender;
                        //source.Email = lead.Email;
                        source.Address = lead.Address;
                        //source.City = lead.City;
                        //source.Country = lead.Country;
                        //source.State = lead.State;
                        //source.Pin=lead.Pin;
                        source.MainDisease = lead.MainDisease;
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
                        history.CreatedOn = DateTime.Now;
                        history.LeadId = lead.Id;
                        history.AgentId = Extensions.GetEmployeeId(this);
                        history.CallDate = DateTime.Today;
                        history.StartTime = lead.StartTime;
                        history.EndTime = lead.EndTime;
                        history.Duration = Convert.ToInt32((lead.EndTime - lead.StartTime).TotalSeconds);

                        context.OBCallHistories.Add(history);
                        await context.SaveChangesAsync();
                    }

                    if (lead.SaveAndClose)
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
            ViewBag.Date = calldate;
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
            var strSql = string.Empty;
            if(HttpContext.Session.GetString("CCTL")?.ToString() == "Yes")
                strSql = $"select Id, Mobile, Name, Disposition, NextCallDate from OBLeads where PatientId is null and AppointmentDate between '{fdate.Date.ToString("yyyy-MM-dd")}' and '{tdate.Date.ToString("yyyy-MM-dd")}' and (AllocatedAgentId={empId} or AllocatedAgentId in (select CCE from CCTeams where CCTL={empId}))  ";
            else
                strSql = $"select Id, Mobile, Name, Disposition, NextCallDate from OBLeads where AllocatedAgentId={empId} and PatientId is null and AppointmentDate between '{fdate.Date.ToString("yyyy-MM-dd")}' and '{tdate.Date.ToString("yyyy-MM-dd")}' ";

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

        [HttpGet("Visits")]
        public IActionResult Visits()
        {
            return View();
        }

        [HttpPost("GetVisitsList")]
        public IActionResult GetVisitsList(SearchDto searchDto)
        {
            try
            {
                var empId = Extensions.GetEmployeeId(this);
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int isTeamLead = (HttpContext.Session.GetString("CCTL")?.ToString() == "Yes") ? 1 : 0;

                var patientData = (from x in context.VwVisits
                                   join e in context.VwEmployees on x.AgentId equals e.EmployeeId into em
                                   from e in em.DefaultIfEmpty()
                                   where x.AppointmentDate != null
                                    && ((isTeamLead == 0 && x.AgentId == empId) || (isTeamLead == 1 && x.TLId == empId))
                                    && (string.IsNullOrEmpty(searchDto.Mobile) || x.Mobile == searchDto.Mobile)
                                    && (string.IsNullOrEmpty(searchDto.FromDate) || x.AppointmentDate >= Convert.ToDateTime(searchDto.FromDate))
                                    && (string.IsNullOrEmpty(searchDto.ToDate) || x.AppointmentDate < Convert.ToDateTime(searchDto.ToDate).AddDays(1))
                                       select new OBVisitListDto
                                       {
                                           Id = x.Id,
                                           Name = x.Name,
                                           Mobile = x.Mobile,
                                           Agent = e.EmpNameWithId,
                                           VisitDate = x.AppointmentDate != null ? x.AppointmentDate.Value.ToString("dd/MM/yyyy hh:mm tt") : string.Empty,
                                           PatientId = x.PatientId
                                       });

                if (!(string.IsNullOrEmpty(sortColumn)))
                {
                    if (sortColumnDirection == "desc")
                    {
                        patientData = patientData.OrderByDescending(s => sortColumn);
                    }
                    else
                    {
                        patientData = patientData.OrderBy(s => sortColumn);
                    }

                }
                else
                {
                    patientData = patientData.OrderByDescending(s => s.Name);
                }


                recordsTotal = patientData.Count();
                var data = patientData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("Visit/{id}")]
        public async Task<IActionResult> Visit(int id)
        {
            var data = await context.OBLeads.AsNoTracking().Where(p => p.Id == id).FirstOrDefaultAsync();
            return View(data);
        }

        [HttpPost("Visit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Visit(OBLead source)
        {
            var responceMessage = string.Empty;
            try
            {
                if (source == null || source.PatientId == null)
                {
                    throw new ApplicationException("Invalid request");
                }
                else
                {
                    source.ModifiedBy = Extensions.GetUserId(this);
                    source.ModifiedOn = DateTime.Now;
                    source.Status = 2;
                    context.OBLeads.Attach(source);
                    context.Entry(source).Property(x => x.PatientId).IsModified = true;
                    context.Entry(source).Property(x => x.AppointmentDate).IsModified = true;
                    context.Entry(source).Property(x => x.ModifiedOn).IsModified = true;
                    context.Entry(source).Property(x => x.ModifiedBy).IsModified = true;
                    context.Entry(source).Property(x => x.Status).IsModified = true;
                    await context.SaveChangesAsync();
                    responceMessage = "Done";
                }
            }
            catch (Exception ex)
            {
                responceMessage = $"Error:{ex.Message}";
            }
            return this.Ok(responceMessage);
        }

        [HttpGet("AppAppointments")]
        public IActionResult AppAppointments()
        {
            return View();
        }

        [HttpPost("GetAppAppointmentList")]
        public IActionResult GetAppAppointmentList(SearchDto searchDto)
        {
            try
            {
                var empId = Extensions.GetEmployeeId(this);
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int isTeamLead = (HttpContext.Session.GetString("CCTL")?.ToString() == "Yes") ? 1 : 0;
                List<int> agentslist = new();
                if (isTeamLead == 1)
                    agentslist = LookUpTable.GetAgentIdsByLead(context, empId);
                else
                    agentslist.Add(empId);

                var patientData = (from x in context.AppAppointments
                                   join e in context.VwEmployees on x.AgentId equals e.EmployeeId into em
                                   from e in em.DefaultIfEmpty()
                                   where agentslist.Contains(x.AgentId.Value)
                                 && (string.IsNullOrEmpty(searchDto.Mobile) || x.Mobile == searchDto.Mobile)
                                 && (string.IsNullOrEmpty(searchDto.FromDate) || x.AppointmentDate >= Convert.ToDateTime(searchDto.FromDate))
                                 && (string.IsNullOrEmpty(searchDto.ToDate) || x.AppointmentDate < Convert.ToDateTime(searchDto.ToDate).AddDays(1))
                                   select new AppAppointmentListDto
                                   {
                                       Id = x.Id,
                                       Name = x.Name,
                                       Mobile = x.Mobile,
                                       Agent = e.EmpNameWithId,
                                       AppointmentDate = x.AppointmentDate != null ? x.AppointmentDate.Value.ToString("dd/MM/yyyy hh:mm tt") : string.Empty,
                                       PatientId = x.PatientId
                                   });

                if (!(string.IsNullOrEmpty(sortColumn)))
                {
                    if (sortColumnDirection == "desc")
                    {
                        patientData = patientData.OrderByDescending(s => sortColumn);
                    }
                    else
                    {
                        patientData = patientData.OrderBy(s => sortColumn);
                    }

                }
                else
                {
                    patientData = patientData.OrderByDescending(s => s.Name);
                }


                recordsTotal = patientData.Count();
                var data = patientData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("AppAppointment")]
        public async Task<IActionResult> AppAppointment()
        {
            var data = new AppAppointment();
            return View(data);
        }

        [HttpPost("SaveAppointment")]
        public async Task<IActionResult> SaveAppointment(AppAppointment appointment)
        {
            var responceMessage = string.Empty;
            var lead = context.OBLeads.Where(p => p.Mobile == appointment.Mobile).FirstOrDefault();
            var userId = Extensions.GetUserId(this);
            var empId = Extensions.GetEmployeeId(this);
            try
            {
                if (appointment == null)
                {
                    throw new ApplicationException("Invalid request");
                }
                else
                {
                    if (context.AppAppointments.Any(o => o.Mobile == appointment.Mobile))
                    {
                        throw new ApplicationException("Patient is all ready exists. Please Check the mobile number");
                    }
                    else
                    {
                        appointment.CreatedOn = DateTime.Now;
                        appointment.CreatedBy = userId;
                        appointment.AgentId = empId; 
                        context.AppAppointments.Add(appointment);
                        await context.SaveChangesAsync();
                    }
                    if (lead != null)
                    {
                        lead.ModifiedBy = userId;
                        lead.ModifiedOn = DateTime.Now;
                        lead.Name = appointment.Name;
                        lead.Age = appointment.Age;
                        lead.Gender = appointment.Gender;
                        lead.Address = appointment.Location;
                        lead.MainDisease = appointment.MainDisease;
                        lead.SubDisease = appointment.SubDisease;
                        lead.ClinicBranch = appointment.ClinicBranch;
                        lead.AppointmentDate = appointment.AppointmentDate;
                        lead.Disposition = 1;
                        lead.AllocatedAgentId = empId;
                        lead.LastCalledBy = empId;
                        lead.LastCallOn = DateTime.Now;
                        await context.SaveChangesAsync();
                    }

                    responceMessage = "Done";
                }
            }
            catch (Exception ex)
            {
                responceMessage = $"Error:{ex.Message}";
            }
            return this.Ok(responceMessage);
        }
    }
}
