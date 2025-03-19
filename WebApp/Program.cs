using WebApp.Services;

// Buildern g�r att vi kan registrera services
var builder = WebApplication.CreateBuilder(args);

// L�gg till services h�r
// builder.Services.AddScoped/Singleton/Transient();
builder.Services.AddScoped<ProjectService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Tvingar webbl�saren att bara anv�nda https
app.UseHsts();

// Omdirigerar alla sidor fr�n http till https
app.UseHttpsRedirection();
// M�jligg�r routing
app.UseRouting();

// Skyddar olika sidor som bara ska visas f�r inloggade
// Anv�nd autorize p� s�dana sidor
app.UseAuthorization();

// H�mtar alla statiska resurser (css, bilder, jquery)
app.MapStaticAssets();

// Default route - vart ska f�rstasidan vara
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=projects}/{id?}")
    .WithStaticAssets();

// K�r appen
app.Run();
