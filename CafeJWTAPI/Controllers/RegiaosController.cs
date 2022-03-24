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
    public class RegiaosController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public RegiaosController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Regiaos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regiao>>> GetRegiao()
        {
            return await _context.Regiao.ToListAsync();
        }

        // GET: api/Regiaos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Regiao>> GetRegiao(int id)
        {
            var regiao = await _context.Regiao.FindAsync(id);

            if (regiao == null)
            {
                return NotFound();
            }

            return regiao;
        }

        // PUT: api/Regiaos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegiao(int id, Regiao regiao)
        {
            if (id != regiao.RegiaoId)
            {
                return BadRequest();
            }

            _context.Entry(regiao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegiaoExists(id))
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

        // POST: api/Regiaos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Regiao>> PostRegiao(Regiao regiao)
        {
            _context.Regiao.Add(regiao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegiao", new { id = regiao.RegiaoId }, regiao);
        }

        // DELETE: api/Regiaos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Regiao>> DeleteRegiao(int id)
        {
            var regiao = await _context.Regiao.FindAsync(id);
            if (regiao == null)
            {
                return NotFound();
            }

            _context.Regiao.Remove(regiao);
            await _context.SaveChangesAsync();

            return regiao;
        }

        private bool RegiaoExists(int id)
        {
            return _context.Regiao.Any(e => e.RegiaoId == id);
        }
    }
}
