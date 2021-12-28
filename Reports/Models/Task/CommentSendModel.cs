using System.ComponentModel.DataAnnotations;

namespace Reports.Models.Task
{
    public class CommentSendModel
    {
        [Required]
        public string Message { get; set; }
    }
}