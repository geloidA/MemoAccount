using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoAccount.Services.Data.Repositories;

public class DepartmentRepository(IMapper mapper) : DomainRepository<Department, DepartmentDto, int>(mapper)
{
    public override async IAsyncEnumerable<Department> GetItemsAsync()
    {
        var dbContext = new MemoDbContext();
        await foreach (var department in dbContext.Departments
                           .Include(d => d.Divisions)
                           .AsAsyncEnumerable()
                           .Select(Mapper.Map<Department>))
        {
            yield return department;
        }

        await dbContext.DisposeAsync();
    }

    public override async Task<ActionResult<Department>> CreateAsync(Department item)
    {
        var dbContext = new MemoDbContext();
        var created = await dbContext.Departments.AddAsync(Mapper.Map<DepartmentDto>(item));
        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(Mapper.Map<Department>(created.Entity));
    }

    protected override int KeySelector(Department item) => item.Id;
}