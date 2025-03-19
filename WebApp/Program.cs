using WebApp.Services;

// Buildern gör att vi kan registrera services
var builder = WebApplication.CreateBuilder(args);

// Lägg till services här
// builder.Services.AddScoped/Singleton/Transient();
builder.Services.AddScoped<ProjectService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Tvingar webbläsaren att bara använda https
app.UseHsts();

// Omdirigerar alla sidor från http till https
app.UseHttpsRedirection();
// Möjliggör routing
app.UseRouting();

// Skyddar olika sidor som bara ska visas för inloggade
// Använd autorize på sådana sidor
app.UseAuthorization();

// Hämtar alla statiska resurser (css, bilder, jquery)
app.MapStaticAssets();

// Default route - vart ska förstasidan vara
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=projects}/{id?}")
    .WithStaticAssets();

// Kör appen
app.Run();
