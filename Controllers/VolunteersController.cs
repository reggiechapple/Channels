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
    public class VolunteersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Volunteers
        [HttpGet("/[controller]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CampaignVolunteers.Include(c => c.Campaign).Include(c => c.Volunteer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Volunteers/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaignVolunteer = await _context.CampaignVolunteers
                .Include(c => c.Campaign)
                .Include(c => c.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (campaignVolunteer == null)
            {
                return NotFound();
            }

            return View(campaignVolunteer);
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id");
            ViewData["VolunteerId"] = new SelectList(_context.Members, "Id", "Id");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CampaignId,VolunteerId")] CampaignVolunteer campaignVolunteer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(campaignVolunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id", campaignVolunteer.CampaignId);
            ViewData["VolunteerId"] = new SelectList(_context.Members, "Id", "Id", campaignVolunteer.VolunteerId);
            return View(campaignVolunteer);
        }

        // GET: Volunteers/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaignVolunteer = await _context.CampaignVolunteers.FindAsync(id);
            if (campaignVolunteer == null)
            {
                return NotFound();
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id", campaignVolunteer.CampaignId);
            ViewData["VolunteerId"] = new SelectList(_context.Members, "Id", "Id", campaignVolunteer.VolunteerId);
            return View(campaignVolunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CampaignId,VolunteerId")] CampaignVolunteer campaignVolunteer)
        {
            if (id != campaignVolunteer.VolunteerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campaignVolunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaignVolunteerExists(campaignVolunteer.VolunteerId))
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
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "Id", campaignVolunteer.CampaignId);
            ViewData["VolunteerId"] = new SelectList(_context.Members, "Id", "Id", campaignVolunteer.VolunteerId);
            return View(campaignVolunteer);
        }

        // GET: Volunteers/Delete/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaignVolunteer = await _context.CampaignVolunteers
                .Include(c => c.Campaign)
                .Include(c => c.Volunteer)
                .FirstOrDefaultAsync(m => m.VolunteerId == id);
            if (campaignVolunteer == null)
            {
                return NotFound();
            }

            return View(campaignVolunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var campaignVolunteer = await _context.CampaignVolunteers.FindAsync(id);
            _context.CampaignVolunteers.Remove(campaignVolunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampaignVolunteerExists(long id)
        {
            return _context.CampaignVolunteers.Any(e => e.VolunteerId == id);
        }
    }
}
