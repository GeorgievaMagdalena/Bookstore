using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using static System.Net.WebRequestMethods;

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookstoreContext _context;

        public BooksController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string bookGenre, string searchString)
        {
            
            IQueryable<Book> books = _context.Book.Include(b => b.Author).Include(b => b.Genres).ThenInclude(bg => bg.Genre).Include(b => b.Reviews); 

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.Where(b => b.Genres.Any(bg => bg.Genre.GenreName == bookGenre));
            }

            var genres = _context.Genre.OrderBy(g => g.GenreName).Select(g => g.GenreName).Distinct();

            
            var viewModel = new BookGenreViewModel
            {
                Books = await books.ToListAsync(),
                Genres = new SelectList(await genres.ToListAsync())
            };

            return View(viewModel);
            
        }
       
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            var reviews = await _context.Review
                .Where(r => r.BookId == id)
                .Select(r => r.Comment) 
                .ToListAsync();

            ViewBag.Reviews = reviews;

            return View(book);
        }
        // GET: Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FullName");
            ViewBag.Genres = new SelectList(_context.Set<Genre>(), "Id", "GenreName");
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,YearPublished,NumPages,Description,Publisher,FrontPage,DownloadURL,AuthorId")] Book book, IFormFile file1, IFormFile file2, List<int> selectedGenres)
        {
            if (ModelState.IsValid)
            {
                if (file1 != null && file1.Length > 0 && file2 != null && file2.Length > 0)
                {
                    var fileName1 = Path.GetFileName(file1.FileName);
                    var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName1);

                    var fileName2 = Path.GetFileName(file2.FileName);
                    var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName2);

                    using (var stream = new FileStream(filePath1, FileMode.Create))
                    {
                        await file1.CopyToAsync(stream);
                    }

                    using (var stream = new FileStream(filePath2, FileMode.Create))
                    {
                        await file2.CopyToAsync(stream);
                    }

                    
                    book.FrontPage = "../UploadedFiles/" + fileName1;
                    book.DownloadURL= "../UploadedFiles/" + fileName2;
                }

                if (selectedGenres != null && selectedGenres.Count > 0)
                {
                    book.Genres = new List<BookGenre>();
                    foreach (var genreId in selectedGenres)
                    {
                        var genre = await _context.Genre.FirstOrDefaultAsync(g => g.Id == genreId);
                        if (genre != null)
                        {
                            book.Genres.Add(new BookGenre { Genre = genre });
                        }
                    }
                }
               
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FirstName", book.AuthorId);
            ViewBag.Genres = new SelectList(_context.Set<Genre>(), "Id", "GenreName");
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            
            
            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FirstName", book.AuthorId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,YearPublished,NumPages,Description,Publisher,FrontPage,DownloadURL,AuthorId")] Book book, IFormFile file1, IFormFile file2)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (file1 != null && file1.Length > 0 && file2 != null && file2.Length > 0)
                {
                    var fileName1 = Path.GetFileName(file1.FileName);
                    var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName1);

                    var fileName2 = Path.GetFileName(file2.FileName);
                    var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName2);

                    using (var stream = new FileStream(filePath1, FileMode.Create))
                    {
                        await file1.CopyToAsync(stream);
                    }

                    using (var stream = new FileStream(filePath2, FileMode.Create))
                    {
                        await file2.CopyToAsync(stream);
                    }

                   
                    var existingBook = await _context.Book.FindAsync(id);

                    if (existingBook != null)
                    {
                      
                        existingBook.Title = book.Title;
                        existingBook.YearPublished = book.YearPublished;
                        existingBook.NumPages = book.NumPages;
                        existingBook.Description = book.Description;
                        existingBook.Publisher = book.Publisher;
                        existingBook.FrontPage = "../UploadedFiles/" + fileName1;
                        existingBook.DownloadURL = "../UploadedFiles/" + fileName2;
                        existingBook.AuthorId = book.AuthorId;

                        
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }

            ViewData["AuthorId"] = new SelectList(_context.Set<Author>(), "Id", "FirstName", book.AuthorId);
            return View(book);
        }


        // GET: Books/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'BookstoreContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
