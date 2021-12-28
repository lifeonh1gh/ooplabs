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
using Reports.Models.Task;
using Reports.Services;

namespace Reports.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ReportsContext _context;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, ReportsContext context, IMapper mapper)
        {
            _taskService = taskService;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string sortOrder, string searchString, string taskFilter,
            int? pageNumber)
        {
            ViewData["TaskSort"] = sortOrder;
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name";
            ViewData["StateSort"] = String.IsNullOrEmpty(sortOrder) ? "state_desc" : "state";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = taskFilter;
            }

            ViewData["TaskFilter"] = searchString;
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            var mentor = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.MentorId);
            var subordinate = await _context.Employees.FirstOrDefaultAsync(x => x.MentorId == employee.Id);
            IQueryable<TaskModel> tasks;
            if (employee.Role == Role.TeamLeader)
                tasks = from s in _taskService.GetAll() select s;
            else if(employee.Role == Role.Mentor)
                tasks = from s in _taskService.GetAll() where s.AssignedEmployee == employee || s.AssignedEmployee == subordinate  select s;
            else
                tasks = from s in _taskService.GetAll() where s.AssignedEmployee == employee select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(s => s.Name.Contains(searchString)
                                         && s.State.ToString().Contains(searchString));
            }

            tasks = sortOrder switch
            {
                "name" => tasks.OrderBy(s => s.Name),
                "name_desc" => tasks.OrderByDescending(s => s.Name),
                "state" => tasks.OrderBy(s => s.State),
                "state_desc" => tasks.OrderByDescending(s => s.State),
                _ => tasks.OrderBy(s => s.Id)
            };

            const int pageSize = 5;
            return View(await Pagination<TaskModel>.CreateAsync(tasks.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateModel model)
        {
            if (_context.Tasks.Any(x => x.Name == model.Name))
                throw new ReportsException("Task with the name '" + model.Name + "' already exists");

            var task = _mapper.Map<TaskModel>(model);
            task.CreationDate = DateTime.Today;
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            task.AssignedEmployee = employee;
            await _context.Tasks.AddAsync(task);
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TaskModel, TaskUpdateModel>());
            var mapper = new Mapper(config);
            var task = mapper.Map<TaskModel, TaskUpdateModel>(await _taskService.GetById(id));
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, TaskUpdateModel model)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (model.Name != task.Name && _context.Tasks.Any(x => x.Name == model.Name))
                throw new ReportsException("Task with the name '" + model.Name + "' already exists");

            _mapper.Map(model, task);
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            task.AssignedEmployee = employee;
            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var task = await _context.Tasks.FindAsync(id);
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet]
        public async Task<IActionResult> AllComments(Guid? id)
        {
            var task = await _context.Tasks.FindAsync(id);
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            var mentor = await _context.Employees.FirstOrDefaultAsync(x => x.Id == employee.MentorId);
            var comments = from s in _context.Comments where s.Task == task || s.SenderEmployee == employee || s.SenderEmployee == mentor
                select s;
            return View(comments);
        }

        [HttpGet]
        public async Task<IActionResult> SendComment(Guid? id)
        {
            await _context.Tasks.FirstOrDefaultAsync(m => m.Id == id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendComment(CommentSendModel model, Guid id)
        {
            var employeeId = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var employee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            var task = await _taskService.GetById(id);
            var comment = _mapper.Map<Comment>(model);
            comment.SenderEmployee = employee;
            comment.Task = task;
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteComment(Guid? id)
        {
            var comment = await _context.Comments.FindAsync(id);
            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetAll));
        }
    }
}