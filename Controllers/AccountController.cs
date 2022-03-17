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
        try
        {
            var user = _context.Employee.FirstOrDefault(x => x.EmployeeEmail == model.Email);
            if (user == null)
                return BadRequest(new { status = "failed", message = "Email not found!" });
            var verify = BCrypt.Net.BCrypt.Verify(model.Password, user!.Password);
            if (!verify)
                return BadRequest(new { status = "failed", message = "Incorrect password!" });
            var response = _userServices.Authenticate(user);
            return Ok(new { status = "success", message = "Login Successfull", data = response });
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("login exception: {0}", ex);
            Sentry.SentrySdk.CaptureException(ex);
            return BadRequest(new { status = "failed", message = ex.Message });
        }
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = _userServices.GetAll();
            return Ok(
                new { status = "success", message = "Get all users Successfull", data = users }
            );
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("get all users exception: {0}", ex);
            Sentry.SentrySdk.CaptureException(ex);
            return BadRequest(new { status = "failed", message = ex.Message });
        }
    }
}
