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
    public class FollowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FollowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Follows
        [HttpGet("/[controller]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Follows.Include(f => f.Followed).Include(f => f.Follower);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Follows/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follows
                .Include(f => f.Followed)
                .Include(f => f.Follower)
                .FirstOrDefaultAsync(m => m.FollowedId == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // GET: Follows/Create
        public IActionResult Create()
        {
            ViewData["FollowedId"] = new SelectList(_context.Members, "Id", "Id");
            ViewData["FollowerId"] = new SelectList(_context.Members, "Id", "Id");
            return View();
        }

        // POST: Follows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FollowerId,FollowedId")] Follow follow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(follow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FollowedId"] = new SelectList(_context.Members, "Id", "Id", follow.FollowedId);
            ViewData["FollowerId"] = new SelectList(_context.Members, "Id", "Id", follow.FollowerId);
            return View(follow);
        }

        // GET: Follows/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follows.FindAsync(id);
            if (follow == null)
            {
                return NotFound();
            }
            ViewData["FollowedId"] = new SelectList(_context.Members, "Id", "Id", follow.FollowedId);
            ViewData["FollowerId"] = new SelectList(_context.Members, "Id", "Id", follow.FollowerId);
            return View(follow);
        }

        // POST: Follows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FollowerId,FollowedId")] Follow follow)
        {
            if (id != follow.FollowedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(follow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FollowExists(follow.FollowedId))
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
            ViewData["FollowedId"] = new SelectList(_context.Members, "Id", "Id", follow.FollowedId);
            ViewData["FollowerId"] = new SelectList(_context.Members, "Id", "Id", follow.FollowerId);
            return View(follow);
        }

        // GET: Follows/Delete/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follow = await _context.Follows
                .Include(f => f.Followed)
                .Include(f => f.Follower)
                .FirstOrDefaultAsync(m => m.FollowedId == id);
            if (follow == null)
            {
                return NotFound();
            }

            return View(follow);
        }

        // POST: Follows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var follow = await _context.Follows.FindAsync(id);
            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FollowExists(long id)
        {
            return _context.Follows.Any(e => e.FollowedId == id);
        }
    }
}
