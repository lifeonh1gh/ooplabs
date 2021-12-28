using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reports.Entities;
using Reports.Helpers;
using Reports.Models;
using Reports.Models.Report;
using Reports.Services;

namespace Reports.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ReportsContext _context;
        private readonly IMapper _mapper;

        public ReportController(IReportService reportService, ReportsContext context, IMapper mapper)
        {
            _reportService = reportService;
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(string sortOrder, string searchString, string reportFilter,
            int? pageNumber)
        {
            ViewData["ReportSort"] = sortOrder;
            ViewData["DescriptionSort"] = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "description";
            ViewData["StateSort"] = String.IsNullOrEmpty(sortOrder) ? "state_desc" : "state";
            ViewData["DateSort"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "date";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = reportFilter;
            }

            ViewData["ReportFilter"] = searchString;
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            var mentor = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.MentorId);
            var subordinate = await _context.Employees.FirstOrDefaultAsync(x => x.MentorId == employee.Id);
            IQueryable<Report> reports;
            if (employee.Role == Role.TeamLeader)
                reports = from s in _reportService.GetAll() select s;
            else if(employee.Role == Role.Mentor)
                reports = from s in _reportService.GetAll() where s.AssignedEmployee == employee || s.AssignedEmployee == subordinate  select s;
            else
                reports = from s in _reportService.GetAll() where s.AssignedEmployee == employee select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                reports = reports.Where(s => s.Description.Contains(searchString)
                                             || s.State.ToString().Contains(searchString));
            }

            reports = sortOrder switch
            {
                "description" => reports.OrderBy(s => s.Description),
                "description_desc" => reports.OrderByDescending(s => s.Description),
                "state" => reports.OrderBy(s => s.State),
                "state_desc" => reports.OrderByDescending(s => s.State),
                "date" => reports.OrderBy(s => s.CreationDate),
                "date_desc" => reports.OrderByDescending(s => s.CreationDate),
                _ => reports.OrderBy(s => s.Id)
            };

            const int pageSize = 5;
            return View(await Pagination<Report>.CreateAsync(reports.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReportCreateModel model)
        {
            if (_context.Reports.Any(x => x.Description == model.Description))
                throw new ReportsException("Report with the description '" + model.Description + "' already exists");
            
            var report = _mapper.Map<Report>(model);
            report.CreationDate = DateTime.Today;
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            report.AssignedEmployee = employee;
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }
        
        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Report, ReportUpdateModel>());
            var mapper = new Mapper(config);
            var report = mapper.Map<Report, ReportUpdateModel>(await _reportService.GetById(id));
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, ReportUpdateModel model)
        {
            var report = await _context.Reports.FindAsync(id);
            
            if (model.Description != report.Description && _context.Reports.Any(x => x.Description == model.Description))
                throw new ReportsException("Report with the description '" + model.Description + "' already exists");
            
            _mapper.Map(model, report);
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var report = await _context.Reports.FindAsync(id);
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }
        
        [HttpGet]
        public async Task<IActionResult> AllTasks(Guid? id)
        {
            var tasks = from s in _context.WeeklyReports
                select s;
            tasks = tasks.Where(s => s.Report.Id == id);
            return View(tasks);
        }
        
        [HttpGet]
        public async Task<IActionResult> InsertTask(Guid? id)
        {
            await _context.WeeklyReports.FirstOrDefaultAsync(m => m.Id == id);
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertTask(TaskInsertModel model, Guid id)
        {
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            var report = await _reportService.GetById(id);
            var task = _mapper.Map<WeeklyReport>(model);
            task.Report = report;
            var currentTask = await _context.Tasks.FirstOrDefaultAsync(m => m.Name == model.TaskName);
            task.Task = currentTask;
            if (task.Task.AssignedEmployee != employee)
            {
                throw new ReportsException("Employee can only add their own tasks to the report");
            }
            if (task.Task.State != TaskState.Resolved)
            {
                throw new ReportsException("Open and Active tasks cannot be added to the report");
            }
            await _context.WeeklyReports.AddAsync(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteWeeklyReport(Guid? id)
        {
            var weeklyReport = await _context.WeeklyReports.FindAsync(id);
            return View(weeklyReport);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWeeklyReport(Guid id)
        {
            var weeklyReport = await _context.WeeklyReports.FindAsync(id);
            _context.WeeklyReports.Remove(weeklyReport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }
    }
}