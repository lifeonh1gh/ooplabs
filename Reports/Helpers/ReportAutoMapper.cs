using AutoMapper;
using Reports.Entities;
using Reports.Models.Report;

namespace Reports.Helpers
{
    public class ReportAutoMapper : Profile
    {
        public ReportAutoMapper()
        {
            CreateMap<ReportCreateModel, Report>();
        
            CreateMap<ReportUpdateModel, Report>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;
                        
                        if (x.DestinationMember.Name == "State" && src.State == null) return false;

                        return true;
                    }
                ));
        }
    }
}