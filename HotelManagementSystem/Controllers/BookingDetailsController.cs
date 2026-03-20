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
    public class BookingDetailsController : Controller
    {
        private readonly HotelDbContext _context;

        public BookingDetailsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: BookingDetails
        public async Task<IActionResult> Index(int? bookingId)
        {
            if (bookingId == null) return RedirectToAction("Index", "Bookings");

            ViewBag.BookingId = bookingId;
            
            var hotelDbContext = _context.BookingDetails
                .Where(b => b.BdBkId == bookingId)
                .Include(b => b.BdBk)
                .Include(b => b.BdSrv);
        
            return View(await hotelDbContext.ToListAsync());
        }

        // GET: BookingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            return RedirectToAction("Index", "BookingDetails", new { bookingId = id });
        }

        // GET: BookingDetails/Create
        public IActionResult Create()
        {
            ViewData["BdBkId"] = new SelectList(_context.Bookings, "BkId", "BkId");
            ViewData["BdSrvId"] = new SelectList(_context.Services, "SrvId", "SrvId");
            return View();
        }

        // POST: BookingDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BdId,BdBkId,BdSrvId,BdQuantity")] BookingDetail bookingDetail)
        {
            bookingDetail.BdBk = await _context.Bookings.FindAsync(bookingDetail.BdBkId);
            bookingDetail.BdSrv = await _context.Services.FindAsync(bookingDetail.BdSrvId);

            ModelState.Clear();
            TryValidateModel(bookingDetail);

            if (ModelState.IsValid)
            {
                _context.Add(bookingDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
    
            ViewData["BdBkId"] = new SelectList(_context.Bookings, "BkId", "BkId", bookingDetail.BdBkId); 
            ViewData["BdSrvId"] = new SelectList(_context.Services, "SrvId", "SrvName", bookingDetail.BdSrvId);
            return View(bookingDetail);
        }

        // GET: BookingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingDetail = await _context.BookingDetails.FindAsync(id);
            if (bookingDetail == null)
            {
                return NotFound();
            }
            ViewData["BdBkId"] = new SelectList(_context.Bookings, "BkId", "BkId", bookingDetail.BdBkId);
            ViewData["BdSrvId"] = new SelectList(_context.Services, "SrvId", "SrvId", bookingDetail.BdSrvId);
            return View(bookingDetail);
        }

        // POST: BookingDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BdId,BdBkId,BdSrvId,BdQuantity")] BookingDetail bookingDetail)
        {
            if (id != bookingDetail.BdId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookingDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingDetailExists(bookingDetail.BdId))
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
            ViewData["BdBkId"] = new SelectList(_context.Bookings, "BkId", "BkId", bookingDetail.BdBkId);
            ViewData["BdSrvId"] = new SelectList(_context.Services, "SrvId", "SrvId", bookingDetail.BdSrvId);
            return View(bookingDetail);
        }

        // GET: BookingDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookingDetail = await _context.BookingDetails
                .Include(b => b.BdBk)
                .Include(b => b.BdSrv)
                .FirstOrDefaultAsync(m => m.BdId == id);
            if (bookingDetail == null)
            {
                return NotFound();
            }

            return View(bookingDetail);
        }

        // POST: BookingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookingDetail = await _context.BookingDetails.FindAsync(id);
            if (bookingDetail != null)
            {
                _context.BookingDetails.Remove(bookingDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingDetailExists(int id)
        {
            return _context.BookingDetails.Any(e => e.BdId == id);
        }
    }
}
