using System.Text.Json.Serialization;

namespace LibraryMS.Model
{
    public class Visitor
    {
        public int VisitorId { get; set; }
        public string? VisitorName { get; set; }
        public string? VisitorAddress { get; set; }
        public int VisitorAge { get; set; }
        public string? VisitorEmail { get; set; }
        public string? VisitorPhoneNo { get; set; }
        public string? IsEmployee { get; set; }
        public string? IsAuthor { get; set; }

        [JsonIgnore]
        public string? Active { get; set; }
    }
}
