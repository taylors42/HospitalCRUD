using BancoDeDados;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PacientesDbContext>(opt =>
    opt.UseSqlServer(connectionString)
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod());
});

var app = builder.Build();

// Middleware para logar todas as requisições com detalhes completos
app.Use(async (context, next) =>
{
    Console.WriteLine("\n" + new string('=', 80));
    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] NOVA REQUISIÇÃO");
    Console.WriteLine(new string('=', 80));

    // Método e URL
    Console.WriteLine($"Método: {context.Request.Method}");
    Console.WriteLine($"URL: {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}");

    // Headers
    Console.WriteLine("\nHEADERS:");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"  {header.Key}: {header.Value}");
    }

    // Body
    Console.WriteLine("\nBODY:");
    context.Request.EnableBuffering();
    using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
    {
        var body = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        Console.WriteLine(string.IsNullOrEmpty(body) ? "  (vazio)" : $"  {body}");
    }

    await next();

    Console.WriteLine($"\nResponse Status: {context.Response.StatusCode}");
    Console.WriteLine(new string('=', 80) + "\n");
});

app.UseCors("AllowAngular");

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapControllers();

app.Run();
