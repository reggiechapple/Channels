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
    public class SupportersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Supporters
        [HttpGet("/[controller]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CauseSupporters.Include(c => c.Cause).Include(c => c.Supporter);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Supporters/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var causeSupporter = await _context.CauseSupporters
                .Include(c => c.Cause)
                .Include(c => c.Supporter)
                .FirstOrDefaultAsync(m => m.SupporterId == id);
            if (causeSupporter == null)
            {
                return NotFound();
            }

            return View(causeSupporter);
        }

        // GET: Supporters/Create
        public IActionResult Create()
        {
            ViewData["CauseId"] = new SelectList(_context.Causes, "Id", "Id");
            ViewData["SupporterId"] = new SelectList(_context.Members, "Id", "Id");
            return View();
        }

        // POST: Supporters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CauseId,SupporterId")] CauseSupporter causeSupporter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(causeSupporter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CauseId"] = new SelectList(_context.Causes, "Id", "Id", causeSupporter.CauseId);
            ViewData["SupporterId"] = new SelectList(_context.Members, "Id", "Id", causeSupporter.SupporterId);
            return View(causeSupporter);
        }

        // GET: Supporters/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var causeSupporter = await _context.CauseSupporters.FindAsync(id);
            if (causeSupporter == null)
            {
                return NotFound();
            }
            ViewData["CauseId"] = new SelectList(_context.Causes, "Id", "Id", causeSupporter.CauseId);
            ViewData["SupporterId"] = new SelectList(_context.Members, "Id", "Id", causeSupporter.SupporterId);
            return View(causeSupporter);
        }

        // POST: Supporters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CauseId,SupporterId")] CauseSupporter causeSupporter)
        {
            if (id != causeSupporter.SupporterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(causeSupporter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CauseSupporterExists(causeSupporter.SupporterId))
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
            ViewData["CauseId"] = new SelectList(_context.Causes, "Id", "Id", causeSupporter.CauseId);
            ViewData["SupporterId"] = new SelectList(_context.Members, "Id", "Id", causeSupporter.SupporterId);
            return View(causeSupporter);
        }

        // GET: Supporters/Delete/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var causeSupporter = await _context.CauseSupporters
                .Include(c => c.Cause)
                .Include(c => c.Supporter)
                .FirstOrDefaultAsync(m => m.SupporterId == id);
            if (causeSupporter == null)
            {
                return NotFound();
            }

            return View(causeSupporter);
        }

        // POST: Supporters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var causeSupporter = await _context.CauseSupporters.FindAsync(id);
            _context.CauseSupporters.Remove(causeSupporter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CauseSupporterExists(long id)
        {
            return _context.CauseSupporters.Any(e => e.SupporterId == id);
        }
    }
}
