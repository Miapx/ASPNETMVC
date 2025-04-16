using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;
using WebApp.Models;

namespace WebApp.Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
{
    public virtual DbSet<ProjectEntity> Projects { get; set; }
    public virtual DbSet<StatusEntity> Statuses { get; set; }
}

//ChatGPT för att lägga till statusarna i databasen
public static class DbSeeder
{
    public static void SeedStatuses(DataContext context)
    {
        if (!context.Statuses.Any())
        {
            var statuses = new List<StatusEntity>
            {
                new StatusEntity { StatusName = "Started" },
                new StatusEntity { StatusName = "Completed" }
            };

            context.Statuses.AddRange(statuses);
            context.SaveChanges();
        }
    }
}