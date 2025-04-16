using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data.Contexts;
using WebApp.Data.Entities;

namespace WebApp.Data.Repositories;

public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;

    //CRUD hämtat från StorageAssignment_2
    //OBS Hans har lagt till virtual på alla metoder, vet ej varför
    //CREATE 
    public async Task<bool> CreateAsync(ProjectEntity entity)
    {
        try
        {
            _context.Projects.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex) 
        {
            Console.WriteLine("Error saving project: " + ex.Message);
            return false;
        }
    }


    //READ
    public async Task<IEnumerable<ProjectEntity>> GetAllAsync
    (
        //För att sortera, filtrera och inkludera (Tips&Trix backend)
        bool orderByDescending = false,
        Expression<Func<ProjectEntity, object>>? sortBy = null,
        Expression<Func<ProjectEntity, bool>>? where = null,
        params Expression<Func<ProjectEntity, object>>[] includes
    )
    {
        IQueryable<ProjectEntity> query = _context.Projects;

        if (where != null)
            query = query.Where(where);

        if (includes != null && includes.Length != 0)
                foreach (var include in includes)
                    query = query.Include(include);

        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);


        var entities = await query.ToListAsync();
        return entities;
    }


    //Här la Hans till en till GetAll med TSelect istället för TEntity
    //men sa aldrig varför eller vad den skulle användas till. Tips&Trix backend 1.30min


    //För att hämta ETT projekt, vet ej om det behövs.
    //Hans la också till filtrering, sortering, inkludering här, vet ej om behövs (Tips&Trix backend 1.30min)
    public async Task<ProjectEntity?> GetAsync(Expression<Func<ProjectEntity, bool>> expression)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(expression);
        return entity ?? null!;
    }

    //För att se om ett projekt redan finns, vet ej om det behövs
    //public async Task<bool> ExistsAsync(Expression<Func<ProjectEntity, bool>> expression)
    //{
    //    var exists = await _context.Projects.AnyAsync(expression);
    //    return exists;
    //}


    //UPDATE
    public async Task<bool> UpdateAsync(ProjectEntity entity)
    {
        try
        {
            _context.Projects.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    //DELETE
    public async Task<bool> DeleteAsync(ProjectEntity entity)
    {
        try
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();  
            return true;
        }
        catch
        {
            return false;
        }
    }

}
