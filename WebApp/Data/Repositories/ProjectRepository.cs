﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data.Contexts;
using WebApp.Data.Entities;
using WebApp.Models;

namespace WebApp.Data.Repositories;

public class ProjectRepository(DataContext context)
{
    private readonly DataContext _context = context;

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
        //För att sortera, filtrera och inkludera
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

    public async Task<ProjectEntity?> GetAsync(Expression<Func<ProjectEntity, bool>> predicate)
    {
        return await _context.Projects.Include(x => x.Status).FirstOrDefaultAsync(predicate);
    }

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
    public ProjectEntity GetById(string id)
    {
        return _context.Projects.Find(id);
    }

    public void Delete(ProjectEntity projectEntity)
    {
        _context.Projects.Remove(projectEntity);
        _context.SaveChanges();
    }
}
