namespace Api_Bouvet.Models
{
    public class Project : BaseEntity
    {
        public string? ProjectManager { get; set; }
        public ICollection<Epic> Epics { get; set; } = new List<Epic>();
    }
}
