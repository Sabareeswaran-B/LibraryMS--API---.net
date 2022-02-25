using System.Text.Json.Serialization;

namespace LibraryMS.Model
{
    public class Lending
    {
        public int LendingId { get; set; }
        public int VisitorId { get; set; }
        public int BookId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LendedOn { get; set; }
        public int Lendinglimit { get; set; }

        [JsonIgnore]
        public int Active { get; set; }

        public virtual Visitor? Visitor { get; set; }
        public virtual Book? Book { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
