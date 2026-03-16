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
    public class RoomsController : Controller
    {
        private readonly HotelDbContext _context;

        public RoomsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "Hotels");

            ViewBag.HotelId = id;
            ViewBag.HotelName = name;

            var roomsByHotel = _context.Rooms
                .Where(r => r.RmHtId == id)
                .Include(r => r.RmCat)
                .Include(r => r.RmHt);

            return View(await roomsByHotel.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
    
            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.RmId == id);
            if (room == null) return NotFound();

            return RedirectToAction("Index", "Bookings", new { roomId = room.RmId });
        }

        // GET: Rooms/Create
        public IActionResult Create(int hotelId)
        {
            ViewBag.HotelId = hotelId;
            ViewBag.HotelName = _context.Hotels.FirstOrDefault(h => h.HtId == hotelId)?.HtName;
    
            ViewData["RmCatId"] = new SelectList(_context.Categories, "CatId", "CatName");
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RmId,RmNumber,RmHtId,RmCatId")] Room room)
        {
            room.RmCat = await _context.Categories.FindAsync(room.RmCatId);
            room.RmHt = await _context.Hotels.FindAsync(room.RmHtId);

            ModelState.Clear();
            TryValidateModel(room);

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Rooms", new { id = room.RmHtId, name = room.RmHt?.HtName });
            }
    
            ViewData["RmCatId"] = new SelectList(_context.Categories, "CatId", "CatName", room.RmCatId);
            ViewBag.HotelId = room.RmHtId;
            ViewBag.HotelName = room.RmHt?.HtName;
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            ViewData["RmCatId"] = new SelectList(_context.Categories, "CatId", "CatName", room.RmCatId);
            ViewData["RmHtId"] = new SelectList(_context.Hotels, "HtId", "HtName", room.RmHtId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RmId,RmNumber,RmHtId,RmCatId")] Room room)
        {
            if (id != room.RmId) return NotFound();

            room.RmCat = await _context.Categories.FindAsync(room.RmCatId);
            room.RmHt = await _context.Hotels.FindAsync(room.RmHtId);

            ModelState.Clear();
            TryValidateModel(room);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RmId)) return NotFound();
                    else throw;
                }
                return RedirectToAction("Index", "Rooms", new { id = room.RmHtId, name = room.RmHt?.HtName });
            }
            ViewData["RmCatId"] = new SelectList(_context.Categories, "CatId", "CatName", room.RmCatId);
            ViewData["RmHtId"] = new SelectList(_context.Hotels, "HtId", "HtName", room.RmHtId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms
                .Include(r => r.RmCat)
                .Include(r => r.RmHt)
                .FirstOrDefaultAsync(m => m.RmId == id);
                
            if (room == null) return NotFound();

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.RmHt)
                .FirstOrDefaultAsync(m => m.RmId == id);
                
            if (room != null)
            {
                int hotelId = room.RmHtId ?? 0;
                string hotelName = room.RmHt?.HtName;

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index", "Rooms", new { id = hotelId, name = hotelName });
            }

            return RedirectToAction("Index", "Hotels");
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.RmId == id);
        }
    }
}