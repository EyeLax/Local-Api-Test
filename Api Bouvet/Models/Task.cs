namespace Api_Bouvet.Models{
    public class Task : BaseEntity
    {
        public string Responsible { get; set; }
        public int EpicId { get; set; }
        public Epic Epic { get; set; }
    }
}
