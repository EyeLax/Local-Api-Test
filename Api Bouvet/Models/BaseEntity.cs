using System.ComponentModel.DataAnnotations;

namespace Api_Bouvet.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; } = null;
    }
}
