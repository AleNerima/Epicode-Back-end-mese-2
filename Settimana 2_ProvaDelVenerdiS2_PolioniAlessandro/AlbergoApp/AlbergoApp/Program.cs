using AlbergoApp.Services.Interfaces;
using AlbergoApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied"; // Aggiungi un percorso per l'accesso negato se necessario
    });

// Configure authorization policies if needed
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

// Add custom services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ICameraService, CameraService>();
builder.Services.AddScoped<IDipendenteService, DipendenteService>();
builder.Services.AddScoped<IPrenotazioneService, PrenotazioneService>();
builder.Services.AddScoped<IServizioService, ServizioService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
