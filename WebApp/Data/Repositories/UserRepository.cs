﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data.Contexts;
using WebApp.Data.Entities;
using WebApp.Models;

namespace WebApp.Data.Repositories;

public class UserRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<AppUser?> GetAsync(Expression<Func<AppUser, bool>> expression)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(expression);
        return entity ?? null!;
    }
}
