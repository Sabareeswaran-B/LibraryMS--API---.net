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
            return await _context.Lending.ToListAsync();
        }

        // GET: api/Lending/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lending>> GetLending(int id)
        {
            var lending = await _context.Lending.FindAsync(id);

            if (lending == null)
            {
                return NotFound();
            }

            return lending;
        }

        // PUT: api/Lending/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLending(int id, Lending lending)
        {
            if (id != lending.LendingId)
            {
                return BadRequest();
            }

            _context.Entry(lending).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LendingExists(id))
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

        // POST: api/Lending
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lending>> PostLending(Lending lending)
        {
            _context.Lending.Add(lending);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLending", new { id = lending.LendingId }, lending);
        }

        // DELETE: api/Lending/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLending(int id)
        {
            var lending = await _context.Lending.FindAsync(id);
            if (lending == null)
            {
                return NotFound();
            }

            _context.Lending.Remove(lending);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LendingExists(int id)
        {
            return _context.Lending.Any(e => e.LendingId == id);
        }
    }
}
