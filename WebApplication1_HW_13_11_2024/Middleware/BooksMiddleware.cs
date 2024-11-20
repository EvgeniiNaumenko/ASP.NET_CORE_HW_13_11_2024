using WebApplication1_HW_13_11_2024.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace WebApplication1_HW_13_11_2024.Middleware
{
    public class BooksMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BookService _bookService;

        public BooksMiddleware(RequestDelegate next, BookService bookService)
        {
            _next = next;
            _bookService = bookService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;

            if (path.StartsWithSegments("/allbooks"))
            {
                var books = await _bookService.GetAllBooksAsync();

                var html = GenerateHtmlTable(books);
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(html);
                return;
            }
            else if (path.StartsWithSegments("/getbooks"))
            {
                var query = context.Request.Query;
                var category = query["category"].ToString();

                var books = string.IsNullOrEmpty(category)
                    ? await _bookService.GetAllBooksAsync()
                    : await _bookService.GetBooksByCategoryAsync(category);

                var html = GenerateHtmlTable(books);
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(html);
                return;
            }

            await _next(context);
        }

        private string GenerateHtmlTable(IEnumerable<Book> books)
        {
            var html = "<table border='1'><tr><th>Title</th><th>Category</th></tr>";
            foreach (var book in books)
            {
                html += $"<tr><td>{book.Title}</td><td>{book.Category}</td></tr>";
            }
            html += "</table>";
            return html;
        }
    }
}
