using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoAccount.Services.Data.Repositories;

public class DepartmentRepository(IMapper mapper) : DomainRepository<Department, DepartmentDto, int>(mapper)
{
    public override IAsyncEnumerable<Department> GetItemsAsync() => DbContext.Departments
        .Include(d => d.Divisions)
        .AsAsyncEnumerable()
        .Select(Mapper.Map<Department>);

    public override async Task<ActionResult<Department>> CreateAsync(Department item)
    {
        var created = await DbContext.Departments.AddAsync(Mapper.Map<DepartmentDto>(item));
        return Success(Mapper.Map<Department>(created.Entity));
    }

    protected override int KeySelector(Department item) => item.Id;
}