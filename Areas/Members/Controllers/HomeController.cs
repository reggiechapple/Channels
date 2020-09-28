using System.Linq;
using System.Threading.Tasks;
using Channels.Areas.Members.Models;
using Channels.Data;
using Channels.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Channels.Areas.Members.Controllers
{
    [Authorize(Roles = "Member")]
    [Area("Members")]
    [Route("[area]/{id}/[action]")]
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

        [HttpGet("/[area]/{id}")]
        public async Task<IActionResult> Index(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.Include(m => m.Identity).FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
               return NotFound();
            }

            ViewData["CurrentMemberId"] = member.Id;

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> Campaigns(long? id)
        {
            if (id == null)
            {
               return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Campaigns)
                    .ThenInclude(c => c.Cause)
                .Include(m => m.Identity)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
               return NotFound();
            }

            ViewData["CurrentMemberId"] = member.Id;
            
            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> Feed(long? id)
        {
            if (id == null)
            {
               return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Identity)
                .FirstOrDefaultAsync(m => m.Id == id);

            var posts = await _context.Posts
                .Where(p => p.AuthorId == member.Id)
                .OrderBy(p => p.Created)
                .ToListAsync();

            if (member == null)
            {
               return NotFound();
            }

            // get a list of posts from the members the user follows
            var othersPosts = _context.Posts
                .Where(p => p.Author.Follows.Any(u => u.FollowerId == member.Id))
                .Include(p => p.Author)
                    .ThenInclude(a => a.Identity)
                .OrderBy(p => p.Created)
                .ToList();
            
            // combine and sort list => var mergedList = list1.Union(list2).ToList();
            var mergedList = posts.Union(othersPosts).OrderBy(p => p.Created).ToList();

            var  model = new FeedViewModel
            {
                Member = member,
                Posts = mergedList
            };

            ViewData["CurrentMemberId"] = member.Id;
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Events(long? id)
        {
            if (id == null)
            {
               return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Identity)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
               return NotFound();
            }

            var events = await _context.Meetings
                .Where(p => p.CoordinatorId == member.Id)
                .OrderBy(p => p.Created)
                .ToListAsync();

            var attendingEvents = _context.Meetings
                .Where(e => e.MeetingAttendees.Any(u => u.AttendeeId == member.Id))
                .Include(e => e.Coordinator)
                    .ThenInclude(a => a.Identity)
                .OrderBy(p => p.Created)
                .ToList();

            var mergedList = events.Union(attendingEvents).OrderBy(p => p.Created).ToList();

            var model = new EventViewModel
            {
                Member = member,
                Meetings = mergedList
            };

            ViewData["CurrentMemberId"] = member.Id;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Circles(long? id)
        {
            if (id == null)
            {
               return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.Identity)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
            {
               return NotFound();
            }

            var circles = await _context.Channels
                .Where(p => p.OwnerId == member.Id)
                .OrderBy(p => p.Created)
                .ToListAsync();

            var joinedCircles = _context.Channels
                .Where(c => c.Subscribers.Any(u => u.SubscriberId == member.Id))
                .Include(e => e.Owner)
                    .ThenInclude(a => a.Identity)
                .OrderBy(p => p.Created)
                .ToList();

            var mergedList = circles.Union(joinedCircles).OrderBy(p => p.Created).ToList();

            var model = new CircleViewModel
            {
                Member = member,
                Channels = mergedList
            };

            ViewData["CurrentMemberId"] = member.Id;

            return View(model);
        }
    }
}