using AlbergoApp.Services.Interfaces;
using AlbergoApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Prenotazione}/{action=Index}/{id?}");

app.Run();