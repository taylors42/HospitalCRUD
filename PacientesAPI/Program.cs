using BancoDeDados;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using PacientesAPI.Repository;
using PacientesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PacientesDbContext>(opt =>
    opt.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IConvenioRepository, ConvenioRepository>();

builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IConvenioService, ConvenioService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors("AllowAngular");

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapControllers();

app.Run();
