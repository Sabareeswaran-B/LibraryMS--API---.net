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
    public class BookController : ControllerBase
    {
        private readonly LMSContext _context;

        public BookController(LMSContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            return await _context.Book.ToListAsync();
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Book/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(Guid id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //Put: AddStock/:id/:CopiesAvailable
        [Authorize(Role.Admin,Role.Clerk)]
        [HttpPut("{id}/{CopiesAvailable}")]
        public async Task<ActionResult<Book>> AddStock(Guid id, [FromRoute]int CopiesAvailable)
        {
            Book book = await _context.Book.FindAsync(id);
            book.CopiesAvailable += CopiesAvailable;
            await _context.SaveChangesAsync();
            return Ok(book);
        }

        //Put: RemoveStock/:id/:CopiesAvailable
        [Authorize(Role.Admin,Role.Clerk)]
        [HttpPut("{id}/{CopiesAvailable}")]
        public async Task<ActionResult<Book>> RemoveStock(Guid id, [FromRoute]int CopiesAvailable)
        {
            Book book = _context.Book.Where(data => data.BookId == id).FirstOrDefault();
            book.CopiesAvailable -= CopiesAvailable;
            await _context.SaveChangesAsync();
            return Ok(book);
        }

        // POST: api/Book
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }
        [Authorize(Role.Admin)]
        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(Guid id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
