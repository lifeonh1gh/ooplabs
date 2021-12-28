using AutoMapper;
using Reports.Entities;
using Reports.Models.Task;

namespace Reports.Helpers
{
    public class TaskAutoMapper : Profile
    {
        public TaskAutoMapper()
        {
            CreateMap<TaskCreateModel, TaskModel>();
        
            CreateMap<TaskUpdateModel, TaskModel>()
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