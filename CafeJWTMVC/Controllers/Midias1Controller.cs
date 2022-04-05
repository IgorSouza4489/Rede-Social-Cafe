using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace CafeJWTMVC.Controllers
{
    [Authorize]
    public class Midias1Controller : Controller
    {
        public static string baseUrl = "http://localhost:62049/api/midias/";

        private readonly ApplicationDBContext _context;

        public Midias1Controller(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Midias1
        public async Task<IActionResult> Index()
        {

            var products = await GetAll();
            return View(products);
        }

        [HttpGet]
        public async Task<List<Midia>> GetAll()
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<List<Midia>>(jsonStr).ToList();

            return res;

        }
        // GET: Midias1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var midia = await _context.Midia
                .FirstOrDefaultAsync(m => m.MidiasId == id);
            if (midia == null)
            {
                return NotFound();
            }

            return View(midia);
        }

        // GET: Midias1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Midias1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Midia pais)
        {

            var Foto = UploadBlob(pais.Imagem);

            if (ModelState.IsValid)
            {
                var connectionString = "Server = (localdb)\\mssqllocaldb; Database = CafeJWTMVC; Trusted_Connection = True; MultipleActiveResultSets = true";
                SqlConnection connection = new SqlConnection(connectionString);
                {
                    var storedprocedure = "CadastrarMidias";
                    var sqlCommand = new SqlCommand(storedprocedure, connection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Foto", await Foto);
                    try
                    {
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pais);
        }


        // GET: Midias1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var midia = await _context.Midia.FindAsync(id);
            if (midia == null)
            {
                return NotFound();
            }
            return View(midia);
        }

        // POST: Midias1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MidiasId,Foto")] Midia midia)
        {
            if (id != midia.MidiasId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(midia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MidiaExists(midia.MidiasId))
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
            return View(midia);
        }

        // GET: Midias1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var midia = await _context.Midia
                .FirstOrDefaultAsync(m => m.MidiasId == id);
            if (midia == null)
            {
                return NotFound();
            }

            return View(midia);
        }

        // POST: Midias1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var midia = await _context.Midia.FindAsync(id);
            _context.Midia.Remove(midia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MidiaExists(int id)
        {
            return _context.Midia.Any(e => e.MidiasId == id);
        }

        public async Task<string> UploadBlob(IFormFile imageFile)
        {

            var reader = imageFile.OpenReadStream();
            var cloundStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=igorsouza0489;AccountKey=bvr5caFtow9DNwVRtfPZZetD+WM1XG7dqbG1R/AxscBAkmvT12xoqDRg+T8ZL6DULmBWCd+4U9mS+AStnnryYw==;EndpointSuffix=core.windows.net");
            var blobClient = cloundStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("imagecontainer");
            await container.CreateIfNotExistsAsync();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            await blob.UploadFromStreamAsync(reader);
            var uri = blob.Uri.ToString();
            return uri;
        }
    }
}
