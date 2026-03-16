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
        // Додаємо два необов'язкові параметри
        public async Task<IActionResult> Index(int? clientId, int? roomId)
        {
            // Беремо всі бронювання
            var bookings = _context.Bookings
                .Include(b => b.BkCl)
                .Include(b => b.BkRm)
                .AsQueryable();

            // Якщо ми прийшли від конкретного Клієнта
            if (clientId != null)
            {
                bookings = bookings.Where(b => b.BkClId == clientId);
                ViewBag.ClientId = clientId;
                ViewBag.ClientName = _context.Clients.FirstOrDefault(c => c.ClId == clientId)?.ClName;
            }

            // Якщо ми прийшли від конкретної Кімнати
            if (roomId != null)
            {
                bookings = bookings.Where(b => b.BkRmId == roomId);
                ViewBag.RoomId = roomId;
                ViewBag.RoomNumber = _context.Rooms.FirstOrDefault(r => r.RmId == roomId)?.RmNumber;
            }

            return View(await bookings.ToListAsync());
        }

        // GET: Bookings/Details/5
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
            // Передаємо ID у View, щоб знати, які поля ховати
            ViewBag.ClientId = clientId;
            ViewBag.RoomId = roomId;

            // Дістаємо імена для гарних заголовків
            if (clientId != null)
            {
                ViewBag.ClientName = _context.Clients.FirstOrDefault(c => c.ClId == clientId)?.ClName;
            }
            if (roomId != null)
            {
                ViewBag.RoomNumber = _context.Rooms.FirstOrDefault(r => r.RmId == roomId)?.RmNumber;
            }

            // Залишаємо списки (вони знадобляться, якщо поля не приховані)
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
            // Підтягуємо пов'язані об'єкти для валідації
            booking.BkCl = await _context.Clients.FindAsync(booking.BkClId);
            booking.BkRm = await _context.Rooms.FindAsync(booking.BkRmId);

            ModelState.Clear();
            TryValidateModel(booking);

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
        
                // Після створення повертаємося до загального списку бронювань
                return RedirectToAction(nameof(Index));
            }
    
            // Якщо помилка (наприклад, не обрали дати)
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
            if (id != booking.BkId)
            {
                return NotFound();
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
                    if (!BookingExists(booking.BkId))
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
            ViewData["BkClId"] = new SelectList(_context.Clients, "ClId", "ClId", booking.BkClId);
            ViewData["BkRmId"] = new SelectList(_context.Rooms, "RmId", "RmId", booking.BkRmId);
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
