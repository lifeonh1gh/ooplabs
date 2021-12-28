using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Reports.Helpers;
using Reports.Entities;
using Reports.Models.Employee;
using BCryptNet = BCrypt.Net.BCrypt;


namespace Reports.Services
{
    public class EmployeeService : IEmployeeService
    {
        private ReportsContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(ReportsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<Employee> GetAll()
        {
            return _context.Employees;
        }

        public async Task<Employee> GetById(Guid? id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) throw new KeyNotFoundException("Employee not found");
            return employee;
        }

        public async Task<Employee> Create(EmployeeCreateModel model)
        {
            if (_context.Employees.Any(x => x.Email == model.Email))
                throw new ReportsException("Employee with the email '" + model.Email + "' already exists");
            
            var employee = _mapper.Map<Employee>(model);
            employee.PasswordHash = ComputeHash(model.Password, new MD5CryptoServiceProvider());
            // employee.PasswordHash = BCryptNet.HashPassword(model.Password);

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> Update(Guid id, EmployeeUpdateModel model)
        {
            var employee = await _context.Employees.FindAsync(id);
            
            if (model.Email != employee.Email && _context.Employees.Any(x => x.Email == model.Email))
                throw new ReportsException("Employee with the email '" + model.Email + "' already exists");

            if (!string.IsNullOrEmpty(model.Password))
                employee.PasswordHash = ComputeHash(model.Password, new MD5CryptoServiceProvider());

            _mapper.Map(model, employee);
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
        
        public async Task<Employee> Details(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return employee;
        }

        public async Task<Employee> Delete(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> SetMentor(Guid id, EmployeeSetMentorModel model)
        {
            var employee = await GetById(id);
            employee.MentorId = (await _context.Employees.FirstOrDefaultAsync(e => e.Name == model.MentorName))
                .Id;
            employee.MentorName = model.MentorName;
            await _context.SaveChangesAsync();
            return employee;
        }

        public string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }
    }
}