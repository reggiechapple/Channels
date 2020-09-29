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
    public class AttendController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attend
        [HttpGet("/[controller]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MeetingAttendees.Include(m => m.Attendee).Include(m => m.Meeting);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attend/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingAttendee = await _context.MeetingAttendees
                .Include(m => m.Attendee)
                .Include(m => m.Meeting)
                .FirstOrDefaultAsync(m => m.AttendeeId == id);
            if (meetingAttendee == null)
            {
                return NotFound();
            }

            return View(meetingAttendee);
        }

        // GET: Attend/Create
        public IActionResult Create()
        {
            ViewData["AttendeeId"] = new SelectList(_context.Members, "Id", "Id");
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "Id", "Id");
            return View();
        }

        // POST: Attend/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeetingId,AttendeeId")] MeetingAttendee meetingAttendee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meetingAttendee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AttendeeId"] = new SelectList(_context.Members, "Id", "Id", meetingAttendee.AttendeeId);
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "Id", "Id", meetingAttendee.MeetingId);
            return View(meetingAttendee);
        }

        // GET: Attend/Edit/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingAttendee = await _context.MeetingAttendees.FindAsync(id);
            if (meetingAttendee == null)
            {
                return NotFound();
            }
            ViewData["AttendeeId"] = new SelectList(_context.Members, "Id", "Id", meetingAttendee.AttendeeId);
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "Id", "Id", meetingAttendee.MeetingId);
            return View(meetingAttendee);
        }

        // POST: Attend/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("MeetingId,AttendeeId")] MeetingAttendee meetingAttendee)
        {
            if (id != meetingAttendee.AttendeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meetingAttendee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingAttendeeExists(meetingAttendee.AttendeeId))
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
            ViewData["AttendeeId"] = new SelectList(_context.Members, "Id", "Id", meetingAttendee.AttendeeId);
            ViewData["MeetingId"] = new SelectList(_context.Meetings, "Id", "Id", meetingAttendee.MeetingId);
            return View(meetingAttendee);
        }

        // GET: Attend/Delete/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingAttendee = await _context.MeetingAttendees
                .Include(m => m.Attendee)
                .Include(m => m.Meeting)
                .FirstOrDefaultAsync(m => m.AttendeeId == id);
            if (meetingAttendee == null)
            {
                return NotFound();
            }

            return View(meetingAttendee);
        }

        // POST: Attend/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var meetingAttendee = await _context.MeetingAttendees.FindAsync(id);
            _context.MeetingAttendees.Remove(meetingAttendee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingAttendeeExists(long id)
        {
            return _context.MeetingAttendees.Any(e => e.AttendeeId == id);
        }
    }
}
