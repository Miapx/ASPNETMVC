using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Data.Contexts;
using WebApp.Data.Repositories;
using WebApp.Models;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);
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

builder.Services.AddScoped<ProjectRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<StatusRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<StatusService>();

builder.Services.AddControllersWithViews();
var app = builder.Build();

//ChatGPT for status
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

    // Kör EF-migrationer automatiskt (valfritt)
    context.Database.Migrate();

    // Seedar standardstatusar
    DbSeeder.SeedStatuses(context);
}
//End of ChatGPT

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Auth}/{action=SignIn}/{id?}")
    .WithStaticAssets();
 
app.Run();
