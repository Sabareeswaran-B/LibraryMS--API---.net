using System.Text.Json.Serialization;

namespace LibraryMS.Model
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string? BookName { get; set; }
        public int ReleasedYear { get; set; }
        public int Edition { get; set; }
        public int CopiesAvailable { get; set; }
        public string? Language { get; set; }
        public Guid AuthorId { get; set; }

        [JsonIgnore]
        public string? Active { get; set; }

        public virtual Author? Author { get; set; }
    }
}
