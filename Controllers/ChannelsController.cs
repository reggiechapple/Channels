using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Channels.Data;
using Channels.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Channels.Data.Identity;
using Channels.Data.Repositories;

namespace Channels.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ChannelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<ApplicationUser> _userManager; 

        public ChannelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, INotificationRepository notificationRepository)
        {
            _context = context;
            _userManager = userManager;
            _notificationRepository = notificationRepository;
        }

        [HttpGet("/[controller]")]
        // GET: Channels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Channels.Include(c => c.Owner).ThenInclude(o => o.Identity);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpGet("{id}")]
        // GET: Channels/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels
                .Include(c => c.Owner)
                    .ThenInclude(o => o.Identity)
                .Include(c => c.Subscribers)
                    .ThenInclude(m => m.Subscriber)
                        .ThenInclude(m => m.Identity)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // GET: Channels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Channels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Channel channel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var member = _context.Members.FirstOrDefault(m => m.IdentityId == userId);
                channel.OwnerId = member.Id;
                _context.Add(channel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(channel);
        }

        [HttpGet("{id}")]
        // GET: Channels/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }
            return View(channel);
        }

        // POST: Channels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Name")] Channel channel)
        {
            if (id != channel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(channel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChannelExists(channel.Id))
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
            return View(channel);
        }

        [HttpGet("{id}")]
        // GET: Channels/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // POST: Channels/Delete/5
        [HttpPost("{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var channel = await _context.Channels.FindAsync(id);
            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult LikeChannel(long? id)
        {
            try
            {
                var channel = _context.Channels.Find(id);

                var userId = _userManager.GetUserId(HttpContext.User);
                var member = _context.Members.Include(m => m.Identity).FirstOrDefault(m => m.IdentityId == userId);

                var notification = new Notification
                {
                    Text = $"{member.Identity.UserName} liked {channel.Name}"
                };

                _notificationRepository.Create(notification, channel.Id);

            } 
            catch(Exception ex)
            {
                return Content("Error occured" + ex.ToString());
            }

            return RedirectToAction(nameof(Details), new {id = id});
        }

        private bool ChannelExists(long id)
        {
            return _context.Channels.Any(e => e.Id == id);
        }
    }
}
