using System.IdentityModel.Tokens.Jwt;
using LibraryMS.Helpers;
using LibraryMS.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace LibraryMS.Services;

public interface IUserServices
{
    AuthResponse Authenticate(Employee model);
    IEnumerable<Employee> GetAll();
    Employee GetById(Guid Id);
}

public class UserServices : IUserServices
{
    private readonly LMSContext _context;

    private readonly AppSettings _appSettings;

    public UserServices(LMSContext context, IOptions<AppSettings> appSettings)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public AuthResponse Authenticate(Employee employee)
    {       
        var token = GenerateJwtToken(employee);
        return new AuthResponse(employee, token);
    }

    public IEnumerable<Employee> GetAll()
    {
        return _context.Employee.ToList();
    }

    public Employee GetById(Guid Id)
    {
        return _context.Employee.FirstOrDefault(x => x.EmployeeId == Id)!;
    }

    private string GenerateJwtToken(Employee employee)
    {
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim("Id", employee.EmployeeId.ToString())
                }
            ),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
