using Bookish.Models.Data;
using Bookish.Models.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Controllers;

public class BooksController : Controller
{
    private readonly ILogger<BooksController> _logger;
    private readonly Library _library;

    public BooksController(ILogger<BooksController> logger, Library library)
    {
        _logger = logger;
        _library = library;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ViewAll()
    {
        var books = _library.Books.ToList();
        var viewModel = new BooksViewModel
        {
            Books = books,
        };
        return View(viewModel);
    }

    public IActionResult ViewIndividual([FromRoute] int id)
    {
        // var Join = _library.Books.Include(copies=>copies.Id)
        //                            //.ThenInclude(copies => copies.BookId);
        var matchingBook = _library.Books.Include(book => book.Copies)
                                        .SingleOrDefault(book => book.Id == id);
        if (matchingBook == null)
        {
            return NotFound();
        }
        // var copies = _library.Copies.Where(copy => copy.BookId==id).ToList();
        // matchingBook.Copies = copies;
        
        return View(matchingBook);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register([FromForm] Book book)
    {
        _library.Books.Add(book);
        _library.SaveChanges();
        return RedirectToAction(nameof(ViewAll));
    }

    
    public IActionResult Update()
    {
        var books = _library.Books.Include(Book => Book.Copies).ToList();
        var viewModel = new BooksViewModel
        {
            Books = books,
        };
        return View(viewModel);

    }

    public IActionResult Unregister()
    {
        var books = _library.Books.Include(Book => Book.Copies).ToList();
        var viewModel = new BooksViewModel
        {
            Books = books,
        };
        return View(viewModel);

    }
[HttpPost]
    public IActionResult Unregister([FromRoute] int id)
    {
        var matchingBook = _library.Books.Where(book => book.Id == id).Single();
        _library.Books.Remove(matchingBook);
        _library.SaveChanges();
        return RedirectToAction(nameof(ViewAll));
    }

}