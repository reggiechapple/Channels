using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Channels.Data;
using Channels.Data.Entities;

namespace Channels.Controllers
{
    [Route("[controller]/[action]")]
    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscribersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subscribers
        [HttpGet("/[controller]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChannelSubscribers.Include(c => c.Channel).Include(c => c.Subscriber);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Subscribers/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelSubscriber = await _context.ChannelSubscribers
                .Include(c => c.Channel)
                .Include(c => c.Subscriber)
                .FirstOrDefaultAsync(m => m.SubscriberId == id);
            if (channelSubscriber == null)
            {
                return NotFound();
            }

            return View(channelSubscriber);
        }

        // GET: Subscribers/Create
        public IActionResult Create()
        {
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "UUID");
            ViewData["SubscriberId"] = new SelectList(_context.Members, "Id", "Id");
            return View();
        }

        // POST: Subscribers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChannelId,SubscriberId")] ChannelSubscriber channelSubscriber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(channelSubscriber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "UUID", channelSubscriber.ChannelId);
            ViewData["SubscriberId"] = new SelectList(_context.Members, "Id", "Id", channelSubscriber.SubscriberId);
            return View(channelSubscriber);
        }

        // GET: Subscribers/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelSubscriber = await _context.ChannelSubscribers.FindAsync(id);
            if (channelSubscriber == null)
            {
                return NotFound();
            }
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "UUID", channelSubscriber.ChannelId);
            ViewData["SubscriberId"] = new SelectList(_context.Members, "Id", "Id", channelSubscriber.SubscriberId);
            return View(channelSubscriber);
        }

        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ChannelId,SubscriberId")] ChannelSubscriber channelSubscriber)
        {
            if (id != channelSubscriber.SubscriberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(channelSubscriber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChannelSubscriberExists(channelSubscriber.SubscriberId))
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
            ViewData["ChannelId"] = new SelectList(_context.Channels, "Id", "UUID", channelSubscriber.ChannelId);
            ViewData["SubscriberId"] = new SelectList(_context.Members, "Id", "Id", channelSubscriber.SubscriberId);
            return View(channelSubscriber);
        }

        // GET: Subscribers/Delete/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelSubscriber = await _context.ChannelSubscribers
                .Include(c => c.Channel)
                .Include(c => c.Subscriber)
                .FirstOrDefaultAsync(m => m.SubscriberId == id);
            if (channelSubscriber == null)
            {
                return NotFound();
            }

            return View(channelSubscriber);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var channelSubscriber = await _context.ChannelSubscribers.FindAsync(id);
            _context.ChannelSubscribers.Remove(channelSubscriber);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChannelSubscriberExists(long id)
        {
            return _context.ChannelSubscribers.Any(e => e.SubscriberId == id);
        }
    }
}
