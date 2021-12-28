using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Reports.Entities;
using Reports.Helpers;
using Reports.Models.Report;

namespace Reports.Services
{
    public class ReportService : IReportService
    {
        private readonly ReportsContext _context;
        private readonly IMapper _mapper;

        public ReportService(ReportsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public IQueryable<Report> GetAll()
        {
            return _context.Reports;
        }

        public async Task<Report> GetById(Guid? id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null) throw new KeyNotFoundException("Report not found");
            return report;
        }
    }
}