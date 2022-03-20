using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeJWTMVC.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<UserInfo> userManager;

        public DashboardController(UserManager<UserInfo> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
