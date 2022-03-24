using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CafeJWTMVC.Controllers
{
    [Authorize]
    public class CafesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CafesController(ApplicationDBContext context)
        {
            _context = context;
        }

        public static string baseUrl = "http://localhost:62049/api/cafes/";

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string jsonStr = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<Cafe>(jsonStr);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            await client.DeleteAsync(url);
            return RedirectToAction(nameof(Index));
        }


        // GET: ProdutorsController
        public async Task<IActionResult> Index()
        {

            var products = await GetProducts();
            return View(products);
        }

        [HttpGet]
        public async Task<List<Cafe>> GetProducts()
        {
            // Use the access token to call a protected web API.
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<List<Cafe>>(jsonStr).ToList();

            return res;

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

    }
}
