using DittaSpedizioni.Interfaces;
using DittaSpedizioni.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<DatabaseConnection>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ISpedizioneService, SpedizioneService>();
builder.Services.AddScoped<IAggiornamentoSpedizioneService, AggiornamentoSpedizioneService>();
builder.Services.AddScoped<IUtenteService, UtenteService>();

// Configurazione di autenticazione
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireLavoratoreRole", policy => policy.RequireRole("Lavoratore"));
    options.AddPolicy("RequireClienteRole", policy => policy.RequireRole("Cliente"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // Abilita l'autenticazione
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
