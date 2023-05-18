using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bookstore.Controllers
{
    public class UserBooksController : Controller
    {
        private readonly BookstoreContext _context;

        public UserBooksController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: UserBooks
        public async Task<IActionResult> Index()
        {
            string userEmail = User.FindFirstValue(ClaimTypes.Email);
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin) {
                var userBooks = await _context.UserBooks
                    .Include(u => u.Book)
                    .Where(u => u.AppUser == userEmail)
                    .ToListAsync();
                    return View(userBooks);
            }
            else {
                var userBooks = await _context.UserBooks
                    .Include(u => u.Book)
                    .ToListAsync();
                    return View(userBooks);
            }
            
        }

        // GET: UserBooks/BuyBook
        public async Task<IActionResult> BuyBook(int? bookId)
        {
            if (bookId == null &_context.UserBooks == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            var signedUser = User.FindFirstValue(ClaimTypes.Email);
            var userBooks = new UserBooks
            {
                AppUser = signedUser,
                BookId = book.Id
            };

            _context.UserBooks.Add(userBooks);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //WriteReview
        public async Task<IActionResult> WriteReview(int? bookId) {
            
            return RedirectToAction("Create", "Reviews");
        }
        // GET: UserBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserBooks == null)
            {
                return NotFound();
            }

            var userBooks = await _context.UserBooks
                .Include(u => u.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBooks == null)
            {
                return NotFound();
            }

            return View(userBooks);
        }

        // GET: UserBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Title");
            return View();
        }

        // POST: UserBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,BookId")] UserBooks userBooks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBooks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Publisher", userBooks.BookId);
            return View(userBooks);
        }

        // GET: UserBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserBooks == null)
            {
                return NotFound();
            }

            var userBooks = await _context.UserBooks.FindAsync(id);
            if (userBooks == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Publisher", userBooks.BookId);
            return View(userBooks);
        }

        // POST: UserBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUser,BookId")] UserBooks userBooks)
        {
            if (id != userBooks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBooks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBooksExists(userBooks.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "Publisher", userBooks.BookId);
            return View(userBooks);
        }

        // GET: UserBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserBooks == null)
            {
                return NotFound();
            }

            var userBooks = await _context.UserBooks
                .Include(u => u.Book)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBooks == null)
            {
                return NotFound();
            }

            return View(userBooks);
        }

        // POST: UserBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserBooks == null)
            {
                return Problem("Entity set 'BookstoreContext.UserBooks'  is null.");
            }
            var userBooks = await _context.UserBooks.FindAsync(id);
            if (userBooks != null)
            {
                _context.UserBooks.Remove(userBooks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBooksExists(int id)
        {
          return (_context.UserBooks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
