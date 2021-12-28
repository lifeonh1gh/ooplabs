using AutoMapper;
using Reports.Entities;
using Reports.Models.Task;

namespace Reports.Helpers
{
    public class CommentAutoMapper : Profile
    {
        public CommentAutoMapper()
        {
            CreateMap<CommentSendModel, Comment>();
        }
    }
}