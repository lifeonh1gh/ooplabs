using AutoMapper;
using Reports.Entities;
using Reports.Models.Report;

namespace Reports.Helpers
{
    public class WeeklyReportAutoMapper : Profile
    {
        public WeeklyReportAutoMapper()
        {
            CreateMap<TaskInsertModel, WeeklyReport>();
        }
    }
}