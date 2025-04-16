using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Data.Contexts;
using WebApp.Data.Repositories;
using WebApp.Models;
using WebApp.Services;

// Buildern g�r att vi kan registrera services
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
    .AddDefaultTokenProviders(); //f�r l�senords�terst�llning

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/auth/signin";
    x.LogoutPath = "/auth/signout";
    x.AccessDeniedPath = "/auth/denied"; 
    x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    x.SlidingExpiration = true;
});

//L�gg till repositories
builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<StatusRepository>();


// L�gg till services h�r
// builder.Services.AddScoped/Singleton/Transient();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<StatusService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

//ChatGPT f�r Statusarna
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    // K�r EF-migrationer automatiskt (valfritt)
    context.Database.Migrate();

    // Seedar standardstatusar
    DbSeeder.SeedStatuses(context);
}


// Tvingar webbl�saren att bara anv�nda https
app.UseHsts();

// Omdirigerar alla sidor fr�n http till https
app.UseHttpsRedirection();
// M�jligg�r routing
app.UseRouting();

//Tillagt p� Hans beg�ran i Tips&Trix backend
app.UseAuthentication();

// Skyddar olika sidor som bara ska visas f�r inloggade
// Anv�nd autorize p� s�dana sidor
app.UseAuthorization();

// H�mtar alla statiska resurser (css, bilder, jquery)
app.MapStaticAssets();

// Default route - vart ska f�rstasidan vara
app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Auth}/{action=SignIn}/{id?}")
    .WithStaticAssets();

// K�r appen   
app.Run();
