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
    public class ProdutorsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ProdutorsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Produtors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produtor>>> GetProdutor()
        {
            return await _context.Produtor.ToListAsync();
        }

        // GET: api/Produtors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Produtor>> GetProdutor(int id)
        {
            var produtor = await _context.Produtor.FindAsync(id);

            if (produtor == null)
            {
                return NotFound();
            }

            return produtor;
        }

        // PUT: api/Produtors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdutor(int id, Produtor produtor)
        {
            if (id != produtor.ProdutorId)
            {
                return BadRequest();
            }

            _context.Entry(produtor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutorExists(id))
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

        // POST: api/Produtors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Produtor>> PostProdutor(Produtor produtor)
        {
            _context.Produtor.Add(produtor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProdutor", new { id = produtor.ProdutorId }, produtor);
        }

        // DELETE: api/Produtors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Produtor>> DeleteProdutor(int id)
        {
            var produtor = await _context.Produtor.FindAsync(id);
            if (produtor == null)
            {
                return NotFound();
            }

            _context.Produtor.Remove(produtor);
            await _context.SaveChangesAsync();

            return produtor;
        }

        private bool ProdutorExists(int id)
        {
            return _context.Produtor.Any(e => e.ProdutorId == id);
        }
    }
}
