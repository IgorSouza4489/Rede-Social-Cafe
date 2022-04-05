using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeJWTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MidiasController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public MidiasController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/<AmigosController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Midia>>> GetAll()
        {

            var amigos = new List<Midia>();


            amigos = _context.Midia.FromSqlRaw("EXECUTE dbo.ConsultarMidias ").ToList();

            if (amigos == null)
            {
                return NoContent();
            }

            return amigos;
        }
    }
}
