using LibraryMS.Entities;
using LibraryMS.Helpers.RBA;
using LibraryMS.Model;
using LibraryMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryMS.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly LMSContext _context;
    private readonly IUserServices _userServices;

    public AccountController(LMSContext context, IUserServices userServices)
    {
        _context = context;
        _userServices = userServices;
    }

    [HttpPost]
    public IActionResult Login(AuthRequest model)
    {
        var user = _context.Employee.FirstOrDefault(x => x.EmployeeEmail == model.Email);
        if (user == null)
            return BadRequest(new { message = "Email not found!" });
        var verify = BCrypt.Net.BCrypt.Verify(model.Password, user!.Password);
        if (!verify)
            return BadRequest(new { message = "Incorrect password!" });
        var response = _userServices.Authenticate(user);
        return Ok(response);
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _userServices.GetAll();
        return Ok(users);
    }
}
