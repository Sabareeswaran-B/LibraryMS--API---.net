using System.Text.Json.Serialization;
using LibraryMS.Entities;

namespace LibraryMS.Model
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public Role EmployeeRole { get; set; }
        public int EmployeeAge { get; set; }
        public int EmployeeSalary { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? EmployeePhoneNo { get; set; }

        // [JsonIgnore]
        public string? Password { get; set; }

        [JsonIgnore]
        public string? Active { get; set; }
    }
}
