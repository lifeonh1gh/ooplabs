using System;
using System.Linq;
using System.Threading.Tasks;
using Reports.Entities;
using Reports.Models.Report;

namespace Reports.Services
{
    public interface IReportService
    {
        IQueryable<Report> GetAll();
        Task<Report> GetById(Guid? id);
    }
}