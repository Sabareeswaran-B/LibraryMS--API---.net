#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Model;
using LibraryMS.Helpers.RBA;
using LibraryMS.Entities;

namespace LibraryMS.Controllers
{
    [Authorize(Role.Admin)]
    [Route("[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly LMSContext _context;

        public EmployeeController(LMSContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                var employees = await _context.Employee
                    .Where(w => w.Active == "true")
                    .ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all employees successfully",
                        data = employees
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get all employees exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByID(Guid id)
        {
            try
            {
                var employees = await _context.Employee
                    .Where(w => w.EmployeeId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (employees == null)
                {
                    return NotFound(new { status = "failed", message = "Employee not found!" });
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get single employees successfully",
                        data = employees
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get employees by id exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExistingEmployee(Guid id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest(new { status = "failed", message = "Given id is not matching" });
            }
            var hashPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password);
            employee.Password = hashPassword;
            employee.Active = "true";
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var updatedEmployee = await _context.Employee.FindAsync(id);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Employee updated successfully",
                        data = updatedEmployee
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsEmployeeExists(id))
                {
                    return NotFound(new { status = "failed", message = "Employee not found" });
                }
                else
                {
                    Console.WriteLine("Update existing Employee exception: {0}", ex);
                    Sentry.SentrySdk.CaptureException(ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(Employee employee)
        {
            try
            {
                var hashPassword = BCrypt.Net.BCrypt.HashPassword(employee.Password);
                employee.Password = hashPassword;
                employee.Active = "true";
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetEmployeebyId",
                    new { id = employee.EmployeeId },
                    new
                    {
                        status = "success",
                        message = "Created employee successfully",
                        data = employee
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Update existing Employee exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var employee = await _context.Employee
                    .Where(w => w.EmployeeId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (employee == null)
                {
                    return NotFound(new { status = "failed", message = "Employee not found" });
                }

                employee.Active = "false";
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Employee deleted successfully" });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Delete Employee exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool IsEmployeeExists(Guid id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id && e.Active == "true");
        }
    }
}
