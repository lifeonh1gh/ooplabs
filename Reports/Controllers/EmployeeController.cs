using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reports.Entities;
using Reports.Helpers;
using Reports.Models;
using Reports.Models.Employee;
using Reports.Services;

namespace Reports.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ReportsContext _context;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, ReportsContext context, IMapper mapper)
        {
            _employeeService = employeeService;
            _context = context;
            _mapper = mapper;
        }

        private async Task Authenticate(Employee employee)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role.ToString())
            };
            var identity = new ClaimsIdentity(claims, ClaimTypes.NameIdentifier,
                ClaimTypes.Name, ClaimTypes.Role);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticateModel model)
        {
            if (ModelState.IsValid)
            {
                var pass = _employeeService.ComputeHash(model.Password, new MD5CryptoServiceProvider());
                var employee =
                    await _context.Employees.FirstOrDefaultAsync(u => u.Email == model.Email && u.PasswordHash == pass);
                if (employee != null)
                {
                    model.Password = pass;
                    await Authenticate(employee);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "User not found or password is incorrect");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string sortOrder, string searchString, string employeeFilter,
            int? pageNumber)
        {
            ViewData["EmployeeSort"] = sortOrder;
            ViewData["NameSort"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name";
            ViewData["RoleSort"] = String.IsNullOrEmpty(sortOrder) ? "role_desc" : "role";
            ViewData["EmailSort"] = String.IsNullOrEmpty(sortOrder) ? "email_desc" : "email";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = employeeFilter;
            }

            ViewData["EmployeeFilter"] = searchString;
            var employees = from s in _employeeService.GetAll()
                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.Name.Contains(searchString)
                                                 && s.Email.Contains(searchString));
            }

            employees = sortOrder switch
            {
                "name" => employees.OrderBy(s => s.Name),
                "name_desc" => employees.OrderByDescending(s => s.Name),
                "role" => employees.OrderBy(s => s.Role),
                "role_desc" => employees.OrderByDescending(s => s.Role),
                "email" => employees.OrderBy(s => s.Email),
                "email_desc" => employees.OrderByDescending(s => s.Email),
                _ => employees.OrderBy(s => s.Id)
            };

            const int pageSize = 5;
            return View(await Pagination<Employee>.CreateAsync(employees.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateModel model)
        {
            await _employeeService.Create(model);
            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            if (id == null)
                return NotFound();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeUpdateModel>());
            var mapper = new Mapper(config);
            var employee = mapper.Map<Employee, EmployeeUpdateModel>(await _employeeService.GetById(id));
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Guid id, EmployeeUpdateModel model)
        {
            await _employeeService.Update(id, model);
            return RedirectToAction(nameof(GetAll));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var employee = await _employeeService.Details(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var employee = await _employeeService.GetById(id);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _employeeService.Delete(id);
            return RedirectToAction(nameof(GetAll));
        }
        
        [HttpGet]
        public async Task<IActionResult> SetMentor(Guid? id)
        {
            if (id == null)
                return NotFound();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeSetMentorModel>());
            var mapper = new Mapper(config);
            var employee = mapper.Map<Employee, EmployeeSetMentorModel>(await _employeeService.GetById(id));
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMentor(Guid id, EmployeeSetMentorModel model)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            await _employeeService.SetMentor(id, model);
            return RedirectToAction(nameof(GetAll));
        }
    }
}