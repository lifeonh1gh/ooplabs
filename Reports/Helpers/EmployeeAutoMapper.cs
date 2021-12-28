using AutoMapper;
using Reports.Entities;
using Reports.Models.Employee;

namespace Reports.Helpers
{
    public class EmployeeAutoMapper : Profile
    {
        public EmployeeAutoMapper()
        {
            CreateMap<EmployeeCreateModel, Employee>();
            
            CreateMap<EmployeeUpdateModel, Employee>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore both null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        // ignore null role
                        if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                        return true;
                    }
                ));
            
            CreateMap<EmployeeSetMentorModel, Employee>();
        }
    }
}