using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Models;

namespace HotelManagementSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly HotelDbContext _context;

        public BookingsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(int? clientId, int? roomId)
        {
            var bookings = _context.Bookings
                .Include(b => b.BkCl)
                .Include(b => b.BkRm)
                .AsQueryable();
            
            if (clientId != null)
            {
                bookings = bookings.Where(b => b.BkClId == clientId);
                ViewBag.ClientId = clientId;
                ViewBag.ClientName = _context.Clients.FirstOrDefault(c => c.ClId == clientId)?.ClName;
            }
            
            if (roomId != null)
            {
                bookings = bookings.Where(b => b.BkRmId == roomId);
                ViewBag.RoomId = roomId;
                ViewBag.RoomNumber = _context.Rooms.FirstOrDefault(r => r.RmId == roomId)?.RmNumber;
            }

            return View(await bookings.ToListAsync());
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.BkCl)
                .Include(b => b.BkRm)
                .FirstOrDefaultAsync(m => m.BkId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create(int? clientId, int? roomId)
        {
            ViewBag.ClientId = clientId;
            ViewBag.RoomId = roomId;
            
            if (clientId != null)
            {
                ViewBag.ClientName = _context.Clients.FirstOrDefault(c => c.ClId == clientId)?.ClName;
            }
            if (roomId != null)
            {
                ViewBag.RoomNumber = _context.Rooms.FirstOrDefault(r => r.RmId == roomId)?.RmNumber;
            }
            
            ViewData["BkClId"] = new SelectList(_context.Clients, "ClId", "ClName");
            ViewData["BkRmId"] = new SelectList(_context.Rooms, "RmId", "RmNumber");
    
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BkId,BkDateIn,BkDateOut,BkClId,BkRmId")] Booking booking)
        {
            booking.BkCl = await _context.Clients.FindAsync(booking.BkClId);
            booking.BkRm = await _context.Rooms.FindAsync(booking.BkRmId);

            ModelState.Clear();
            TryValidateModel(booking);
            
            if (booking.BkDateOut <= booking.BkDateIn)
            {
                ModelState.AddModelError("BkDateOut", "Помилка: Дата виїзду має бути пізнішою за дату заїзду!");
            }
            
            bool isRoomBusy = _context.Bookings.Any(b => 
                b.BkRmId == booking.BkRmId && 
                b.BkDateIn < booking.BkDateOut && 
                b.BkDateOut > booking.BkDateIn);

            if (isRoomBusy)
            {
                ModelState.AddModelError(string.Empty, "Увага! На ці дати кімната вже заброньована. Оберіть інші дати або кімнату.");
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
        
                if (booking.BkClId != null) return RedirectToAction("Index", new { clientId = booking.BkClId });
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["BkClId"] = new SelectList(_context.Clients, "ClId", "ClName", booking.BkClId);
            ViewData["BkRmId"] = new SelectList(_context.Rooms, "RmId", "RmNumber", booking.BkRmId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["BkClId"] = new SelectList(_context.Clients, "ClId", "ClId", booking.BkClId);
            ViewData["BkRmId"] = new SelectList(_context.Rooms, "RmId", "RmId", booking.BkRmId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BkId,BkDateIn,BkDateOut,BkClId,BkRmId")] Booking booking)
        {
            if (id != booking.BkId) return NotFound();

            booking.BkCl = await _context.Clients.FindAsync(booking.BkClId);
            booking.BkRm = await _context.Rooms.FindAsync(booking.BkRmId);

            ModelState.Clear();
            TryValidateModel(booking);
            
            if (booking.BkDateOut <= booking.BkDateIn)
            {
                ModelState.AddModelError("BkDateOut", "Помилка: Дата виїзду має бути пізнішою за дату заїзду!");
            }
            
            bool isRoomBusy = _context.Bookings.Any(b => 
                b.BkId != booking.BkId &&
                b.BkRmId == booking.BkRmId && 
                b.BkDateIn < booking.BkDateOut && 
                b.BkDateOut > booking.BkDateIn);

            if (isRoomBusy)
            {
                ModelState.AddModelError(string.Empty, "Неможливо змінити! На ці дати кімната вже зайнята кимось іншим.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BkId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
    
            ViewData["BkClId"] = new SelectList(_context.Clients, "ClId", "ClName", booking.BkClId);
            ViewData["BkRmId"] = new SelectList(_context.Rooms, "RmId", "RmNumber", booking.BkRmId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.BkCl)
                .Include(b => b.BkRm)
                .FirstOrDefaultAsync(m => m.BkId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BkId == id);
        }
    }
}
