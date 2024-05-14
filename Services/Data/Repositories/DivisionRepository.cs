using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MemoAccount.Services.Data.Repositories;

public class DivisionRepository(IMapper mapper) : DomainRepository<Division, DivisionDto, int>(mapper)
{
    public override async IAsyncEnumerable<Division> GetItemsAsync()
    {
        var dbContext = new MemoDbContext();
        Log.Information("Division GetItemsAsync");
        await foreach(var division in dbContext.Divisions
                          .Include(d => d.Department)
                          .AsAsyncEnumerable()
                          .Select(Mapper.Map<Division>))
        {
            yield return division;
        }

        await dbContext.DisposeAsync();
    }

    public override async Task<ActionResult<Division>> CreateAsync(Division item)
    {
        var dbContext = new MemoDbContext();
        Log.Information("Division CreateAsync");
        var created = await dbContext.Divisions.AddAsync(Mapper.Map<DivisionDto>(item));
        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(Mapper.Map<Division>(created.Entity));
    }

    protected override int KeySelector(Division item) => item.Id;
}