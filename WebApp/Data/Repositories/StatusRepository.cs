using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Data.Contexts;
using WebApp.Data.Entities;

namespace WebApp.Data.Repositories;

public class StatusRepository(DataContext context)
{
    private readonly DataContext _context = context;

    public async Task<StatusEntity?> GetAsync(Expression<Func<StatusEntity, bool>> expression)
    {
        var entity = await _context.Statuses.FirstOrDefaultAsync(expression);
        return entity ?? null!;
    }
}
