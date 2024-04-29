using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoAccount.Services.Data.Repositories;

public class DivisionRepository(IMapper mapper) : DomainRepository<Division, DivisionDto, int>(mapper)
{
    public override IAsyncEnumerable<Division> GetItemsAsync() => DbContext.Divisions
        .Include(d => d.Department)
        .AsAsyncEnumerable()
        .Select(Mapper.Map<Division>);

    public override async Task<ActionResult<Division>> CreateAsync(Division item)
    {
        var created = await DbContext.Divisions.AddAsync(Mapper.Map<DivisionDto>(item));
        return Success(Mapper.Map<Division>(created.Entity));
    }

    protected override int KeySelector(Division item) => item.Id;
}