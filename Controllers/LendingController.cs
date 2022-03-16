#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Model;
using LibraryMS.Helpers.RBA;
using LibraryMS.Entities;

namespace LibraryMS.Controllers
{
    [Authorize(Role.Admin, Role.Clerk)]
    [Route("[controller]/[action]")]
    [ApiController]
    public class LendingController : ControllerBase
    {
        private readonly LMSContext _context;

        public LendingController(LMSContext context)
        {
            _context = context;
        }

        // GET: api/Lending
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lending>>> GetLending()
        {
            try
            {
                var employees = await _context.Lending.Where(w => w.Active == "true").ToListAsync();
                return Ok(
                    new
                    {
                        status = "Success",
                        message = "Get all employees successfully",
                        data = employees
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get all employees exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Lending/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lending>> GetLendingById(Guid id)
        {
            try
            {
                var lendings = await _context.Lending
                    .Where(w => w.LendingId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (lendings == null)
                {
                    return NotFound(new { status = "failed", message = "Lendings not found!" });
                }

                return Ok(
                    new
                    {
                        status = "Success",
                        message = "Get single lendings successfully",
                        data = lendings
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get lendings by id exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Lending/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExistingLending(Guid id, Lending lending)
        {
            if (id != lending.LendingId)
            {
                return BadRequest(new { status = "failed", message = "Given id is not matching" });
            }
            lending.Active = "true";
            _context.Entry(lending).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var updatedEmployee = await _context.Lending.FindAsync(id);
                return Ok(
                    new
                    {
                        status = "Success",
                        message = "Lending updated successfully",
                        data = updatedEmployee
                    }
                );
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!LendingExists(id))
                {
                    return NotFound(new { status = "failed", message = "Lending not found" });
                }
                else
                {
                    Console.WriteLine("Update existing lending exception: {0}", ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        // POST: api/Lending
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lending>> AddNewLending(Lending lending)
        {
            try
            {
                _context.Lending.Add(lending);

                Book book = _context.Book
                    .Where(data => data.BookId == lending.BookId)
                    .FirstOrDefault();
                book.CopiesAvailable -= 1;

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetLendingById", new { id = lending.LendingId }, lending);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Update existing Employee exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Lending/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLending(int id)
        {
            try
            {
                var lending = await _context.Lending.FindAsync(id);
                if (lending == null)
                {
                    return NotFound(new { status = "failed", message = "Lending not found" });
                }
                Book book = _context.Book
                    .Where(data => data.BookId == lending.BookId)
                    .FirstOrDefault();
                book.CopiesAvailable += 1;
                lending.Active = "false";
                await _context.SaveChangesAsync();

                return Ok(new { status = "Success", message = "Lending deleted successfully" });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Delete Lending exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool LendingExists(Guid id)
        {
            return _context.Lending.Any(e => e.LendingId == id);
        }
    }
}
