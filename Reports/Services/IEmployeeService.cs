using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Reports.Entities;
using Reports.Models.Employee;

namespace Reports.Services
{
    public interface IEmployeeService
    {
        IQueryable<Employee> GetAll();
        Task<Employee> GetById(Guid? id);
        Task<Employee> Create(EmployeeCreateModel model);
        Task<Employee> Update(Guid id, EmployeeUpdateModel model);
        Task<Employee> Details(Guid id);
        Task<Employee> Delete(Guid id);
        Task<Employee> SetMentor(Guid id, EmployeeSetMentorModel model);
        string ComputeHash(string input, HashAlgorithm algorithm);
    }
}