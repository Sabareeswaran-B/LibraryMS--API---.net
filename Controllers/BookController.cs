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
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _context.Book.Where(w => w.Active == "true").ToListAsync();
                return Ok(
                    new { status = "Success", message = "Get all books successfully", data = books }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get all books exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            try
            {
                var book = await _context.Book
                    .Where(w => w.BookId == id)
                    .Where(w => w.Active == "true")
                    .FirstAsync();
                if (book == null)
                {
                    return NotFound(new { status = "failed", message = "Book not found!" });
                }

                return Ok(
                    new
                    {
                        status = "Success",
                        message = "Get single books successfully",
                        data = book
                    }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("get book by id exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // PUT: api/Book/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExistingBook(Guid id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var updatedBook = await _context.Book.FindAsync(id);
                return Ok(
                    new
                    {
                        status = "Success",
                        message = "Book updated successfully",
                        data = updatedBook
                    }
                );
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!BookExists(id))
                {
                    return NotFound(new { status = "failed", message = "Book not found" });
                }
                else
                {
                    Console.WriteLine("update existing book exception: {0}", ex);
                    return BadRequest(new { status = "failed", message = ex.Message });
                }
            }
        }

        //Put: AddStock/:id/:CopiesToAdd
        [Authorize(Role.Admin, Role.Clerk)]
        [HttpPut("{id}/{CopiesToAdd}")]
        public async Task<IActionResult> AddStock(Guid id, [FromRoute] int CopiesToAdd)
        {
            try
            {
                Book book = await _context.Book.FindAsync(id);
                if (book == null)
                    return NotFound(new { status = "failed", message = "Book not found" });

                book.CopiesAvailable += CopiesToAdd;
                await _context.SaveChangesAsync();
                return Ok(
                    new { status = "Success", message = "Added stock successfully", data = book }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Add stock exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        //Put: RemoveStock/:id/:CopiesToRemove
        [Authorize(Role.Admin, Role.Clerk)]
        [HttpPut("{id}/{CopiesToRemove}")]
        public async Task<IActionResult> RemoveStock(Guid id, [FromRoute] int CopiesToRemove)
        {
            try
            {
                Book book = _context.Book.Where(data => data.BookId == id).FirstOrDefault();

                if (book == null)
                    return NotFound(new { status = "failed", message = "Book not found" });

                book.CopiesAvailable -= CopiesToRemove;
                await _context.SaveChangesAsync();
                return Ok(
                    new { status = "Success", message = "Removed stock successfully", data = book }
                );
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Remove stock exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        // POST: api/Book
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> AddNewBook(Book book)
        {
            try
            {
                book.Active = "true";
                _context.Book.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = book.BookId }, book);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Add new book exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        [Authorize(Role.Admin)]
        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Book.FindAsync(id);
                if (book == null)
                {
                    return NotFound(new { status = "failed", message = "Book not found" });
                }

                book.Active = "false";
                await _context.SaveChangesAsync();

                return Ok(new { status = "Success", message = "Book deleted successfully" });
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Delete Book exception: {0}", ex);
                return BadRequest(new { status = "failed", message = ex.Message });
            }
        }

        private bool BookExists(Guid id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
