#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Model;

namespace LibraryMS.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Visitor>>> GetVisitor()
        {
            return await _context.Visitor.ToListAsync();
        }

        // GET: api/Visitor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Visitor>> GetVisitor(int id)
        {
            var visitor = await _context.Visitor.FindAsync(id);

            if (visitor == null)
            {
                return NotFound();
            }

            return visitor;
        }

        // PUT: api/Visitor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisitor(Guid id, Visitor visitor)
        {
            if (id != visitor.VisitorId)
            {
                return BadRequest();
            }

            _context.Entry(visitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitorExists(id))
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

        // POST: api/Visitor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Visitor>> PostVisitor(Visitor visitor)
        {
            _context.Visitor.Add(visitor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVisitor", new { id = visitor.VisitorId }, visitor);
        }

        // DELETE: api/Visitor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(int id)
        {
            var visitor = await _context.Visitor.FindAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }

            _context.Visitor.Remove(visitor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VisitorExists(Guid id)
        {
            return _context.Visitor.Any(e => e.VisitorId == id);
        }
    }
}
