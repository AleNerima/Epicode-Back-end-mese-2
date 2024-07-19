using PoliziaMunicipaleApp.Services.Interfaces;
using PoliziaMunicipaleApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura il servizio di connessione al database
builder.Services.AddSingleton<IDatabaseConnectionService, DatabaseConnectionService>();

// Configura i servizi per le operazioni sui dati
builder.Services.AddScoped<IAnagraficaService, AnagraficaService>();
builder.Services.AddScoped<ITipoViolazioneService, TipoViolazioneService>();
builder.Services.AddScoped<IVerbaleService, VerbaleService>(); // Aggiungi questo per i verbali
builder.Services.AddScoped<IVisualizzazioneService, VisualizzazioneService>();

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
    pattern: "{controller=Verbale}/{action=IndexVerbale}/{id?}");

app.Run();
