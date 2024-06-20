namespace Api_Bouvet.Models
{
    public class Epic : BaseEntity
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
