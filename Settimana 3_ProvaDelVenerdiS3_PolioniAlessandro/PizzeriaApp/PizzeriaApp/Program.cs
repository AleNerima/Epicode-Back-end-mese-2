using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data;
using PizzeriaApp.Services;
using PizzeriaApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configura il contesto del database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Aggiunge i servizi di autenticazione e autorizzazione
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Configura l'autenticazione dei cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    });

// Configura le politiche di autorizzazione
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// Aggiunge i servizi MVC
builder.Services.AddControllersWithViews();

// Configura i servizi di sessione
builder.Services.AddDistributedMemoryCache(); // Usa una cache in memoria per la sessione
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout della sessione
    options.Cookie.HttpOnly = true; // Cookie solo HTTP
    options.Cookie.IsEssential = true; // Cookie essenziale per il funzionamento dell'app
});

var app = builder.Build();

// Configura la pipeline delle richieste
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Middleware di autenticazione
app.UseAuthorization();  // Middleware di autorizzazione

// Aggiungi il middleware della sessione
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
