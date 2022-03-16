using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Model;
using LibraryMS.Helpers.RBA;
using LibraryMS.Entities;

namespace LibraryMS.Controllers
{
    [Route("[controller]/[action]")]
    [Authorize(Role.Admin)]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly LMSContext _context;

        public AuthorController(LMSContext context)
        {
            _context = context;
        }

        // GET: api/Author
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var authors = await _context.Author.Where(w => w.Active == "true").ToListAsync();
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get all authors successfully",
                        data = authors
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get all authors exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Author/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            try
            {
                var author = await _context.Author
                    .Where(w => w.AuthorId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (author == null)
                {
                    return NotFound(new { status = "failed", message = "Author not found!" });
                }

                return Ok(
                    new
                    {
                        status = "success",
                        message = "Get single authors successfully",
                        data = author
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Get Author By Id exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Author/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExistingAuthor(Guid id, Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest(new { status = "failed", message = "Given id is not matching" });
            }
            author.Active = "true";
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var updatedAuthor = await _context.Author.FindAsync(id);
                return Ok(
                    new
                    {
                        status = "success",
                        message = "Author updated successfully",
                        data = updatedAuthor
                    }
                );
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!IsAuthorExists(id))
                {
                    return NotFound(new { status = "failed", message = "Author not found" });
                }
                else
                {
                    Console.WriteLine("Update existing Author exception: {0}", ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        // POST: api/Author
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> AddNewAuthor(Author author)
        {
            try
            {
                author.Active = "true";
                _context.Author.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    "GetAuthorById",
                    new { id = author.AuthorId },
                    new
                    {
                        status = "success",
                        message = "Created author successfully",
                        data = author
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Add new Author exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // DELETE: api/Author/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                var author = await _context.Author
                    .Where(w => w.AuthorId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (author == null)
                {
                    return NotFound(new { status = "failed", message = "Author not found" });
                }

                author.Active = "false";
                await _context.SaveChangesAsync();

                return Ok(new { status = "success", message = "Author deleted successfully" });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Delete Author exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool IsAuthorExists(Guid id)
        {
            return _context.Author.Any(e => e.AuthorId == id && e.Active == "true");
        }
    }
}
