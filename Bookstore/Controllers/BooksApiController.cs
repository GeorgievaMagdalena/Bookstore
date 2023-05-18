using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bookstore.Models;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly BookstoreContext _context;

        public BooksApiController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: api/BooksApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook(string bookGenre, string searchString)
        {
            IQueryable<Book> books = _context.Book.Include(b => b.Author).Include(b => b.Genres).ThenInclude(bg => bg.Genre);

            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(bookGenre))
            {
                books = books.Where(b => b.Genres.Any(bg => bg.Genre.GenreName.Contains(bookGenre)));
            }

            return await books.ToListAsync();
        }


        // GET: api/BooksApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
          if (_context.Book == null)
          {
              return NotFound();
          }
            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/BooksApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BooksApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
          if (_context.Book == null)
          {
              return Problem("Entity set 'BookstoreContext.Book'  is null.");
          }
            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/BooksApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Book == null)
            {
                return NotFound();
            }
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
