using Bookish.Models.Data;
using Bookish.Models.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Controllers;

public class CopyController : Controller
{
    private readonly ILogger<BooksController> _logger;
    private readonly Library _library;

    public CopyController(ILogger<BooksController> logger, Library library)
    {
        _logger = logger;
        _library = library;
    }

    public IActionResult AddCopy([FromRoute] int id)
    {
        var matchingBook = _library.Books.SingleOrDefault(book => book.Id == id);
        if (matchingBook == null)
        {
            return NotFound();
        }
        return View(matchingBook);
    }

    [HttpPost]
    public IActionResult AddCopy([FromRoute] int id, [FromForm] Copy copy)
    {
        copy.BookId = id;
        _library.Copies.Add(copy);
        _library.SaveChanges();
        return RedirectToAction(nameof(AddCopy));
    }

}