using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;

namespace CafeJWTMVC.Controllers
{
    [Authorize]
    public class Cafes1Controller : Controller
    {
        private readonly ApplicationDBContext _context;

        public Cafes1Controller(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Cafes1
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Cafe.Include(c => c.Produtor).Include(c => c.Regiao);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Cafes1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Cafe cafe = _context.Cafe.Find(id);
            CafeCommentViewModel vm = new CafeCommentViewModel();

            if (cafe == null)
            {
                return NotFound();
            }
            vm.CafesId = id.Value;
            vm.NomeCafe = cafe.NomeCafe;
            var Comments = _context.CafeComments.Where(d => d.CafesId.Equals(id.Value)).ToList();
            vm.ListOfComments = Comments;

            var ratings = _context.CafeComments.Where(d => d.CafesId.Equals(id.Value)).ToList();
            if (ratings.Count() > 0)
            {
                var ratingSum = ratings.Sum(d => d.Rating);
                ViewBag.RatingSum = ratingSum;
                var ratingCount = ratings.Count();
                ViewBag.RatingCount = ratingCount;
            }
            else
            {
                ViewBag.RatingSum = 0;
                ViewBag.RatingCount = 0;
            }
            return View(vm);
        }

        // GET: Cafes1/Create
        public IActionResult Create()
        {
            ViewData["ProdutorId"] = new SelectList(_context.Produtor, "ProdutorId", "ProdutorId");
            ViewData["RegiaoId"] = new SelectList(_context.Regiao, "RegiaoId", "RegiaoId");
            return View();
        }

        // POST: Cafes1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeCafe,Nota,Impressoes,PublishedDate,ProdutorId,RegiaoId")] Cafe cafe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cafe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutorId"] = new SelectList(_context.Produtor, "ProdutorId", "ProdutorId", cafe.ProdutorId);
            ViewData["RegiaoId"] = new SelectList(_context.Regiao, "RegiaoId", "RegiaoId", cafe.RegiaoId);
            return View(cafe);
        }

        // GET: Cafes1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafe.FindAsync(id);
            if (cafe == null)
            {
                return NotFound();
            }
            ViewData["ProdutorId"] = new SelectList(_context.Produtor, "ProdutorId", "ProdutorId", cafe.ProdutorId);
            ViewData["RegiaoId"] = new SelectList(_context.Regiao, "RegiaoId", "RegiaoId", cafe.RegiaoId);
            return View(cafe);
        }

        // POST: Cafes1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeCafe,Nota,Impressoes,PublishedDate,ProdutorId,RegiaoId")] Cafe cafe)
        {
            if (id != cafe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cafe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CafeExists(cafe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutorId"] = new SelectList(_context.Produtor, "ProdutorId", "ProdutorId", cafe.ProdutorId);
            ViewData["RegiaoId"] = new SelectList(_context.Regiao, "RegiaoId", "RegiaoId", cafe.RegiaoId);
            return View(cafe);
        }

        // GET: Cafes1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cafe = await _context.Cafe
                .Include(c => c.Produtor)
                .Include(c => c.Regiao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cafe == null)
            {
                return NotFound();
            }

            return View(cafe);
        }

        // POST: Cafes1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cafe = await _context.Cafe.FindAsync(id);
            _context.Cafe.Remove(cafe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CafeExists(int id)
        {
            return _context.Cafe.Any(e => e.Id == id);
        }
    }
}
