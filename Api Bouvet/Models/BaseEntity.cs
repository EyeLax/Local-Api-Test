namespace Api_Bouvet.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; } = null;
    }
}
