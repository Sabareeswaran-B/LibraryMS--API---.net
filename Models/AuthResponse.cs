namespace LibraryMS.Model;

    public class AuthResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }

        public AuthResponse(Employee employee, string token)
        {
            Id = employee.EmployeeId;
            Username = employee.EmployeeName;
            Role = employee.EmployeeRole;
            Token = token;
        }
    };

