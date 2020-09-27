using System.Threading.Tasks;
using Channels.Data;
using Channels.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Channels.Areas.Members.Components
{
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserInfoViewComponent(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var member = await _context.Members.Include(m => m.Identity).FirstOrDefaultAsync(m => m.IdentityId == _userManager.GetUserId(HttpContext.User));
            return View(member);
        }
    }
}