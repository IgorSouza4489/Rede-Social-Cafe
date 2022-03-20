using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;

namespace CafeJWTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeCommentsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public CafeCommentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/CafeComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CafeComment>>> GetCafeComments()
        {
            return await _context.CafeComments.ToListAsync();
        }

        // GET: api/CafeComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CafeComment>> GetCafeComment(int id)
        {
            var cafeComment = await _context.CafeComments.FindAsync(id);

            if (cafeComment == null)
            {
                return NotFound();
            }

            return cafeComment;
        }

        // PUT: api/CafeComments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCafeComment(int id, CafeComment cafeComment)
        {
            if (id != cafeComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(cafeComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CafeCommentExists(id))
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

        // POST: api/CafeComments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CafeComment>> PostCafeComment(CafeComment cafeComment)
        {
            _context.CafeComments.Add(cafeComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCafeComment", new { id = cafeComment.Id }, cafeComment);
        }

        // DELETE: api/CafeComments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CafeComment>> DeleteCafeComment(int id)
        {
            var cafeComment = await _context.CafeComments.FindAsync(id);
            if (cafeComment == null)
            {
                return NotFound();
            }

            _context.CafeComments.Remove(cafeComment);
            await _context.SaveChangesAsync();

            return cafeComment;
        }

        private bool CafeCommentExists(int id)
        {
            return _context.CafeComments.Any(e => e.Id == id);
        }
    }
}
