using WebApplication1_HW_13_11_2024.Helper;
using WebApplication1_HW_13_11_2024.Middleware;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new BookService(connectionString));

var app = builder.Build();

app.UseMiddleware<BooksMiddleware>();
app.UseMiddleware<AuthMiddleware>();

app.Run(async (context) =>
{
    context.Response.StatusCode = 404;
    await context.Response.WriteAsync("Page not found!");
});

app.Run();