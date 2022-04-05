using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;

namespace CafeJWTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CafesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Cafes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cafe>>> GetCafe()
        {
            return await _context.Cafe.ToListAsync();
        }

        // GET: api/Cafes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cafe>> GetCafe(int id)
        {

            var cafe = await _context.Cafe.FindAsync(id);

            if (cafe == null)
            {
                return NotFound();
            }

            return cafe;
        }

        // PUT: api/Cafes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCafe(int id, Cafe cafe)
        {
            if (id != cafe.Id)
            {
                return BadRequest();
            }

            _context.Entry(cafe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CafeExists(id))
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

        // POST: api/Cafes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cafe>> PostCafe(Cafe cafe)
        {
            _context.Cafe.Add(cafe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCafe", new { id = cafe.Id }, cafe);
        }

        // DELETE: api/Cafes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cafe>> DeleteCafe(int id)
        {
            var cafe = await _context.Cafe.FindAsync(id);
            if (cafe == null)
            {
                return NotFound();
            }

            _context.Cafe.Remove(cafe);
            await _context.SaveChangesAsync();

            return cafe;
        }

        private bool CafeExists(int id)
        {
            return _context.Cafe.Any(e => e.Id == id);
        }


    }
}
