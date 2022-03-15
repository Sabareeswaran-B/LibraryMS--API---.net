using LibraryMS.Entities;

namespace LibraryMS.Model;

    public class AuthResponse
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public Role Role { get; set; }
        public string? Token { get; set; }

        public AuthResponse(Employee employee, string token)
        {
            Id = employee.EmployeeId;
            Username = employee.EmployeeName;
            Role = employee.EmployeeRole;
            Token = token;
        }
    };

