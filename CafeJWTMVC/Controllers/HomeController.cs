using CafeJWTMVC.Models;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CafeJWTMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        /*public async Task<IActionResult> Registrar (UserInfo userinfo)
        {
            UserInfo userinfo2 = new UserInfo();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(userinfo2), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync ("http://localhost:62049/api/token/Register", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userinfo2 = JsonConvert.DeserializeObject<UserInfo>(apiResponse);
                }
            }
            return RedirectToRoute(new { controller = "Home" });
        }
        */

        public async Task<IActionResult> LoginUser(UserInfo user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("http://localhost:62049/api/token", stringContent))
                {
                    string token = await response.Content.ReadAsStringAsync();
                    if (token == "Invalid credentials")
                    {
                        ViewBag.Message = "Incorrect UserId or Password!";
                        return Redirect("~/Home/Index");
                    }
                    HttpContext.Session.SetString("JWToken", token);
                }

                return Redirect("~/Cafes/Index");
            }
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();//clear token
            return Redirect("~/Home/Index");
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
