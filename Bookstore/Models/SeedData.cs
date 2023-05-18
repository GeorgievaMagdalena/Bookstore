using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Bookstore.Areas.Identity.Data;

namespace Bookstore.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<BookstoreUser>>();
            IdentityResult roleResult;

            
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }

            BookstoreUser user = await UserManager.FindByEmailAsync("admin@bookstore.com");
            if (user == null)
            {
                var User = new BookstoreUser();
                User.Email = "admin@bookstore.com";
                User.UserName = "admin@bookstore.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                    
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new BookstoreContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BookstoreContext>>());

            CreateUserRoles(serviceProvider).Wait();

            // Look for any movies.
            if (context.Author.Any() && context.Genre.Any() && context.BookGenre.Any())
            {
                return;   // DB has been seeded
            }

            context.Author.AddRange(
                new Author { /*Id = 6, */FirstName = "J.K.", LastName = "Rowling", Nationality = "British", Gender = "Female", BirthDate = DateTime.Parse("1965-7-31"),
                    Books = new List<Book> {
                         new Book
                         {
                             //Id = 1,
                             Title = "Harry Potter and the Goblet of Fire",
                             YearPublished = 2000,
                             NumPages = 636,
                             Description = "In this fourth book of the Harry Potter series, Harry must compete in the dangerous Triwizard Tournament, while also facing the return of Lord Voldemort.",
                             Publisher = "Bloomsbury Publishing",
                             FrontPage = "https://res.cloudinary.com/bloomsbury-atlas/image/upload/w_568,c_scale/jackets/9781408855683.jpg",
                             DownloadURL = "https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&ved=2ahUKEwjqspPZqPP-AhUOrYsKHYuXAqwQFnoECAoQAQ&url=https%3A%2F%2Febookpresssite.files.wordpress.com%2F2017%2F10%2F4_harry_potter_and_the_goblet_of_fire.pdf&usg=AOvVaw3sywOqaDlMpjb_OwEmMXHb",
                             AuthorId = 1,

                             Reviews = new List<Review>
                             {
                                new Review { AppUser = "user1@user.com", Comment = "Great!", Rating = 8, BookId = 1},
                                new Review { AppUser = "user2@user.com", Comment = "Not bad!", Rating = 5, BookId = 1}
                             },

                             UsersBooks = new List<UserBooks>
                             {
                                new UserBooks { AppUser = "user1@user.com", BookId = 1 },
                                new UserBooks { AppUser = "user2@user.com", BookId = 1 },
                             }
                         }
                    }
                },
                new Author { /*Id = 2, */FirstName = "F. Scott", LastName = "Fitzgerald", Nationality = "American", Gender = "Male", BirthDate = DateTime.Parse("1986-9-24"),
                    Books = new List<Book> {
                          new Book
                          {
                               // Id = 2,
                                Title = "The Great Gatsby",
                                YearPublished = 1925,
                                NumPages = 180,
                                Description = "Set in the Roaring Twenties, this novel follows the mysterious Jay Gatsby and his pursuit of the wealthy and beautiful Daisy Buchanan.",
                                Publisher = "Charles Scribner's Sons",
                                FrontPage = "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7a/The_Great_Gatsby_Cover_1925_Retouched.jpg/800px-The_Great_Gatsby_Cover_1925_Retouched.jpg",
                                DownloadURL = "https://www.planetebook.com/free-ebooks/the-great-gatsby.pdf",
                                AuthorId = 2,                  //F. Scott Fitzgerald

                                Reviews = new List<Review>
                                {
                                    new Review { AppUser = "user4@user.com", Comment = "Fantastic!", Rating = 10, BookId = 2},
                                    new Review { AppUser = "user5@user.com", Comment = "Did not like it.", Rating = 1, BookId = 2}
                                },

                                UsersBooks = new List<UserBooks>
                                {
                                    new UserBooks { AppUser = "user4@user.com", BookId = 2 },
                                    new UserBooks { AppUser = "user5@user.com", BookId = 2 },
                                 }
                         }
                    }
                },
                new Author { /*Id = 3, */FirstName = "Jane", LastName = "Austin", Nationality = "British", Gender = "Female", BirthDate = DateTime.Parse("1775-12-16"),
                   Books = new List<Book> {
                     new Book
                     {
                        //Id = 3,
                        Title = "Pride and Prejudice",
                        YearPublished = 1813,
                        NumPages = 432,
                        Description = "This novel follows the story of the Bennet family, and in particular the strong-willed Elizabeth Bennet, as they navigate issues of marriage, love, and class in Regency-era England.",
                        Publisher = "T. Egerton, Whitehall",
                        FrontPage = "https://theperksofbeingnourablog.files.wordpress.com/2021/01/pride-and-prejudice-barnes-noble-collectible-editions-1.jpg",
                        DownloadURL = "https://www.planetebook.com/free-ebooks/pride-and-prejudice.pdf",
                        AuthorId = 3,

                        Reviews = new List<Review>
                        {
                             new Review { AppUser = "user6@user.com", Comment = "Good book!", Rating = 7, BookId = 3},
                        },

                        UsersBooks = new List<UserBooks>
                        {
                             new UserBooks { AppUser = "user6@user.com", BookId = 3 },
                        }
                     }
                   }
                },
                new Author { /*Id = 4, */FirstName = "Franz", LastName = "Kafka", Nationality = "Czech", Gender = "Male", BirthDate = DateTime.Parse("1883-7-3"),
                    Books = new List<Book>
                    {
                        new Book
                        {
                           //  Id = 4,
                             Title = "The Metamorphosis",
                             YearPublished = 1915,
                             NumPages = 55,
                             Description = "This novella tells the story of Gregor Samsa, a traveling salesman who wakes up one day to find that he has transformed into a giant insect. The story explores themes of alienation, family, and identity.",
                             Publisher = "Kurt Wolff Verlag",
                             FrontPage = "https://payload.cargocollective.com/1/10/324976/5528906/Metamorphosis%20done%20done%20small.jpg",
                             DownloadURL = "https://www.planetebook.com/free-ebooks/the-metamorphosis.pdf",
                             AuthorId = 4,

                             Reviews = new List<Review>
                             {
                                new Review { AppUser = "user7@user.com", Comment = "Good book!", Rating = 9, BookId = 4},
                                new Review { AppUser = "user8@user.com", Comment = "Confusing.", Rating = 4, BookId = 4}
                             },

                             UsersBooks = new List<UserBooks>
                             {
                                new UserBooks { AppUser = "user7@user.com", BookId = 4 },
                                new UserBooks { AppUser = "user8@user.com", BookId = 4 }
                             }
                        }
                    }
                },
                new Author { /*Id = 5, */FirstName = "Mary", LastName = "Shelly", Nationality = "British", Gender = "Female", BirthDate = DateTime.Parse("1797-8-30"),
                    Books = new List<Book>
                    {
                        new Book
                        {
                            // Id = 5,
                             Title = "Frankenstein",
                             YearPublished = 1818,
                             NumPages = 280,
                             Description = "This Gothic novel follows Victor Frankenstein, a scientist who creates a creature through unconventional scientific experiments. The story delves into themes of creation, ambition, and the consequences of playing god.",
                             Publisher = "Lackington, Hughes, Harding, Mavor & Jones",
                             FrontPage = "https://res.cloudinary.com/bloomsbury-atlas/image/upload/w_568,c_scale/jackets/9781847493507.jpg",
                             DownloadURL = "https://standardebooks.org/ebooks/mary-shelley/frankenstein/downloads/mary-shelley_frankenstein.epub",
                             AuthorId = 5,

                             Reviews = new List<Review>
                             {
                                new Review { AppUser = "user9@user.com", Comment = "Exciting!", Rating = 9, BookId = 5}
                             },

                             UsersBooks = new List<UserBooks>
                             {
                                new UserBooks { AppUser = "user9@user.com", BookId = 5 }
                             }

                            
                        }
                    }
                }
            );
            context.SaveChanges();

            context.Genre.AddRange(
                new Genre { /*Id = 1, */GenreName = "Fantasy" },
                new Genre { /*Id = 2, */GenreName = "Young Adult" },
                new Genre { /*Id = 3, */GenreName = "Modernist literature" },
                new Genre { /*Id = 4, */GenreName = "Classic literature" },
                new Genre { /*Id = 5, */GenreName = "Romance" },
                new Genre { /*Id = 6, */GenreName = "Horror" },
                new Genre { /*Id = 7, */GenreName = "Fiction" }
            );
            context.SaveChanges();


            context.BookGenre.AddRange(
                new BookGenre { GenreId = 1, BookId = 1 },
                new BookGenre { GenreId = 2, BookId = 1 },
                new BookGenre { GenreId = 7, BookId = 1 },
                new BookGenre { GenreId = 3, BookId = 2 },
                new BookGenre { GenreId = 7, BookId = 2 },
                new BookGenre { GenreId = 4, BookId = 3 },
                new BookGenre { GenreId = 5, BookId = 3 },
                new BookGenre { GenreId = 3, BookId = 4 },
                new BookGenre { GenreId = 7, BookId = 4 },
                new BookGenre { GenreId = 6, BookId = 5 },
                new BookGenre { GenreId = 7, BookId = 5 }
            );
            context.SaveChanges();

        }

    }
}
