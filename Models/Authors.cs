using System.Text.Json.Serialization;

namespace LibraryMS.Model
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorAge { get; set; }
        public string? PrimaryLanguage { get; set; }

        [JsonIgnore]
        public string? Active { get; set; }
    }
}
