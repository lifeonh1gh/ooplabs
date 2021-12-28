using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reports.Entities;
using Reports.Helpers;
using Reports.Models.Task;

namespace Reports.Services
{
    public class TaskService : ITaskService
    {
        private readonly ReportsContext _context;
        private readonly IMapper _mapper;

        public TaskService(ReportsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public IQueryable<TaskModel> GetAll()
        {
            return _context.Tasks;
        }

        public async Task<TaskModel> GetById(Guid? id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) throw new KeyNotFoundException("Task not found");
            return task;
        }

        public async Task<TaskModel> Create(TaskCreateModel model)
        {
            if (_context.Tasks.Any(x => x.Name == model.Name))
                throw new ReportsException("Task with the name '" + model.Name + "' already exists");
            
            var task = _mapper.Map<TaskModel>(model);
            task.CreationDate = DateTime.Today;
            var employeeId = ClaimTypes.NameIdentifier;
            task.AssignedEmployee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskModel> Update(Guid id, TaskUpdateModel model)
        {
            var task = await _context.Tasks.FindAsync(id);
            
            if (model.Name != task.Name && _context.Tasks.Any(x => x.Name == model.Name))
                throw new ReportsException("Task with the name '" + model.Name + "' already exists");
            
            _mapper.Map(model, task);
            var employeeId = ClaimTypes.NameIdentifier;
            task.AssignedEmployee = await _context.Employees.SingleAsync(x => x.Id == Guid.Parse(employeeId));
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskModel> Details(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            return task;
        }

        public async Task<TaskModel> Delete(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}