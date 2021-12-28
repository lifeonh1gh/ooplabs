using System;
using System.Linq;
using System.Threading.Tasks;
using Reports.Entities;
using Reports.Models.Task;

namespace Reports.Services
{
    public interface ITaskService
    {
        IQueryable<TaskModel> GetAll();
        Task<TaskModel> GetById(Guid? id);
        Task<TaskModel> Create(TaskCreateModel model);
        Task<TaskModel> Update(Guid id, TaskUpdateModel model);
        Task<TaskModel> Details(Guid id);
        Task<TaskModel> Delete(Guid id);
    }
}