using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Channels.Models;
using Channels.Data;
using Microsoft.AspNetCore.Identity;
using Channels.Data.Identity;
using Microsoft.EntityFrameworkCore;

namespace Channels.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, 
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("~/")]
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                var member = _context.Members.Include(m => m.Identity).FirstOrDefault(m => m.IdentityId == _userManager.GetUserId(HttpContext.User));
                if (await _userManager.IsInRoleAsync(member.Identity, "Member"))
                {
                    return RedirectToAction(nameof(Index), "Home", new { area = "Members", id = member.Id });
                }
            }
            return View();
        }

        [HttpGet("/[action]")]
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
