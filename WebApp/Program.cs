using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Data.Contexts;
using WebApp.Data.Repositories;
using WebApp.Models;
using WebApp.Services;

// Buildern gör att vi kan registrera services
var builder = WebApplication.CreateBuilder(args);

//Registrerar DataContext
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));

builder.Services.AddIdentity<AppUser, IdentityRole>(x =>
{
    x.Password.RequiredLength = 8;
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedEmail = false;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders(); //för lösenordsåterställning

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/auth/signin";
    x.LogoutPath = "/auth/signout";
    x.AccessDeniedPath = "/auth/denied"; 
    x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    x.SlidingExpiration = true;
});

//Lägg till repositories
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<StatusRepository>();


// Lägg till services här
// builder.Services.AddScoped/Singleton/Transient();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<StatusService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

//ChatGPT för Statusarna
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    // Kör EF-migrationer automatiskt (valfritt)
    context.Database.Migrate();

    // Seedar standardstatusar
    DbSeeder.SeedStatuses(context);
}


// Tvingar webbläsaren att bara använda https
app.UseHsts();

// Omdirigerar alla sidor från http till https
app.UseHttpsRedirection();
// Möjliggör routing
app.UseRouting();

//Tillagt på Hans begäran i Tips&Trix backend
app.UseAuthentication();

// Skyddar olika sidor som bara ska visas för inloggade
// Använd autorize på sådana sidor
app.UseAuthorization();

// Hämtar alla statiska resurser (css, bilder, jquery)
app.MapStaticAssets();

// Default route - vart ska förstasidan vara
app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Auth}/{action=SignIn}/{id?}")
    .WithStaticAssets();

// Kör appen   
app.Run();
