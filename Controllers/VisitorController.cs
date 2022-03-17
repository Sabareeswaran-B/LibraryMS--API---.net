#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Model;
using LibraryMS.Helpers.RBA;
using LibraryMS.Entities;

namespace LibraryMS.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        private readonly LMSContext _context;

        public VisitorController(LMSContext context)
        {
            _context = context;
        }

        // GET: api/Visitor
        [HttpGet]
        public async Task<IActionResult> GetAllVisitors()
        {
            try
            {
                var visitors = await _context.Visitor.Where(w => w.Active == "true").ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all visitors successfully",
                        data = visitors
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get all visitors exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Visitor/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitorById(Guid id)
        {
            try
            {
                var visitor = await _context.Visitor
                    .Where(w => w.VisitorId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (visitor == null)
                {
                    return NotFound(new { status = "failed", message = "Visitor not found!" });
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get single visitor successfully",
                        data = visitor
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get visitor by id exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Visitor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Role.Admin, Role.Clerk)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExistingVisitor(Guid id, Visitor visitor)
        {
            if (id != visitor.VisitorId)
            {
                return BadRequest(new { status = "failed", message = "Given id is not matching" });
            }
            visitor.Active = "true";
            _context.Entry(visitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var updatedVisitor = await _context.Visitor.FindAsync(id);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Visitor updated successfully",
                        data = updatedVisitor
                    }
                );
            }
            catch (System.Exception ex)
            {
                if (!IsVisitorExists(id))
                {
                    return NotFound(new { status = "failed", message = "Visitor not found" });
                }
                else
                {
                    Console.WriteLine("Update existing visitor exception: {0}", ex);
                    Sentry.SentrySdk.CaptureException(ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        // POST: api/Visitor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Role.Admin, Role.Clerk)]
        [HttpPost]
        public async Task<IActionResult> AddNewVisitor(Visitor visitor)
        {
            try
            {
                visitor.Active = "true";
                _context.Visitor.Add(visitor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetVisitorById",
                    new { id = visitor.VisitorId },
                    new
                    {
                        status = "success",
                        message = "Created visitor successfully",
                        data = visitor
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Add new visitor exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Visitor/5
        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(Guid id)
        {
            try
            {
                var visitor = await _context.Visitor
                    .Where(w => w.VisitorId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (visitor == null)
                {
                    return NotFound(new { status = "failed", message = "Visitor not found" });
                }

                visitor.Active = "false";
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Visitor deleted successfully" });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Delete Visitor exception: {0}", ex);
                Sentry.SentrySdk.CaptureException(ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool IsVisitorExists(Guid id)
        {
            return _context.Visitor.Any(e => e.VisitorId == id && e.Active == "true");
        }
    }
}
