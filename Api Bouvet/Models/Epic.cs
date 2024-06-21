using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Api_Bouvet.Models
{
    public class Epic : BaseEntity
    {
        [Required]
        public int ProjectId { get; set; }
        public  Project? Project { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
