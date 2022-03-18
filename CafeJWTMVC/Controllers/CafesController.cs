using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace CafeJWTMVC.Controllers
{
    public class CafesController : Controller
    {

            public static string baseUrl = "http://localhost:62049/api/cafes/";
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

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("")] Cafe products)
            {
                var accessToken = HttpContext.Session.GetString("JWToken");
                var url = baseUrl;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var stringContent = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json");
                await client.PostAsync(url, stringContent);

                return RedirectToAction(nameof(Index));
            }

            public async Task<IActionResult> Edit(int? id)
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

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("")] Cafe products)
            {
                if (id != products.Id)
                {
                    return NotFound();
                }
                var accessToken = HttpContext.Session.GetString("JWToken");
                var url = baseUrl + id;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var stringContent = new StringContent(JsonConvert.SerializeObject(products), Encoding.UTF8, "application/json");
                await client.PutAsync(url, stringContent);

                return RedirectToAction(nameof(Index));

            }

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


        private readonly ApplicationDBContext _context;

        public CafesController(ApplicationDBContext context)
        {
            _context = context;
        }
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

        
    }
}
