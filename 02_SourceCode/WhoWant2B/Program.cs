using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Infrastructure.Data;
using WhoWant2B.Infrastructure.Repositories;
using WhoWant2B.Infrastructure.Services;
using WhoWant2B.Services;

/// <summary>
/// Configuración inicial del WebApplicationBuilder y carga de variables de entorno.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configuración de la persistencia de datos mediante Entity Framework Core y SQL Server.
/// </summary>
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

/// <summary>
/// Registro de servicios de aplicación y repositorios en el contenedor de Inyección de Dependencias.
/// </summary>
builder.Services.AddScoped<IConfiguracionService, ConfiguracionService>();
builder.Services.AddScoped<IJuegoService, JuegoService>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ISecurityService, SecurityService>();

/// <summary>
/// Configuración del estado de sesión y caché distribuida en memoria.
/// </summary>
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión.
    options.Cookie.HttpOnly = true;                // Protege la cookie contra acceso desde JavaScript.
    options.Cookie.IsEssential = true;             // Indica que la cookie es necesaria para el funcionamiento de la app.
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

/// <summary>
/// Configuración del pipeline de manejo de solicitudes HTTP (Middleware).
/// </summary>
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

/// <summary>
/// Definición de la ruta predeterminada del sistema, apuntando inicialmente al motor del juego.
/// </summary>
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Juego}/{action=Index}/{id?}");

app.Run();