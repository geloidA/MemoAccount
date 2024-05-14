using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MemoAccount.Services.Data.Repositories;

public class MemoRepository(IMapper mapper) : DomainRepository<Memo, MemoDto, int>(mapper)
{
    public override async IAsyncEnumerable<Memo> GetItemsAsync()
    {
        var dbContext = new MemoDbContext();
        Log.Information("Memo GetItemsAsync");
        await foreach (var memo in dbContext.Memos
                           .Include(m => m.Division)
                           .Include(m => m.Department)
                           .Include(m => m.User)
                           .OrderByDescending(m => m.CreatedDate)
                           .AsAsyncEnumerable()
                           .Select(Mapper.Map<Memo>))
            yield return memo;

        await dbContext.DisposeAsync();
    }

    public override async Task<ActionResult<Memo>> CreateAsync(Memo item)
    {
        var dbContext = new MemoDbContext();
        Log.Information("Memo CreateAsync");
        var sameId = await dbContext.Memos.FindAsync(KeySelector(item));

        if (sameId != null) return Error("Служебная записка с указанным номером уже существует.");

        var add = Mapper.Map<MemoDto>(item);

        add.Department = null!;
        add.Division = null;

        var created = await dbContext.Memos.AddAsync(add);
        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(Mapper.Map<Memo>(created.Entity));
    }

    public override async Task<ActionResult<Memo>> UpdateAsync(Memo item)
    {
        var dbContext = new MemoDbContext();

        Log.Information("Memo UpdateAsync");
        var updated = Mapper.Map<MemoDto>(item);
        updated.Department = null!;
        updated.Division = null;
        dbContext.Update(updated);
        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(item);
    }

    protected override int KeySelector(Memo item) => item.Id;
}