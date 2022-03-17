using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryMS.Model
{
    public class Lending
    {
        public Guid LendingId { get; set; }
        public Guid VisitorId { get; set; }
        public Guid BookId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime? LendedOn { get; set; }
        public int Lendinglimit { get; set; }

        [JsonIgnore]
        public string? Active { get; set; }

        [ForeignKey("VisitorId")]
        public virtual Visitor? Visitor { get; set; }
        public virtual Book? Book { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
