using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CafeJWTMVC.Controllers
{
    [Authorize]
    public class CafeCommentsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CafeCommentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: CafeComments
        public static string baseUrl = "http://localhost:62049/api/cafecomments/";
        public async Task<IActionResult> Index()
        {
            var products = await GetProducts();
            return View(products);
        }

        [HttpGet]
        public async Task<List<CafeComment>> GetProducts()
        {
            // Use the access token to call a protected web API.
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);


            var res = JsonConvert.DeserializeObject<List<CafeComment>>(jsonStr).ToList();

            return res;
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
            var res = JsonConvert.DeserializeObject<CafeComment>(jsonStr);

            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        // POST: CafeComments/Delete/5
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

        private bool CafeCommentExists(int id)
        {
            return _context.CafeComments.Any(e => e.Id == id);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CafeCommentViewModel vm)
        {
            var comment = vm.Comment;
            var cafeId = vm.CafesId;
            var rating = vm.Rating;

            CafeComment cafcomment = new CafeComment()
            {
                CafesId = cafeId,
                Comments = comment,
                Rating = rating,
                PublishedDate = DateTime.Now
            };

            _context.CafeComments.Add(cafcomment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Cafes1", new { id = cafeId });

        }

    }
}
