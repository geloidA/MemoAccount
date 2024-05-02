using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoAccount.Services.Data.Repositories;

public class MemoRepository(IMapper mapper) : DomainRepository<Memo, MemoDto, int>(mapper)
{
    public override async IAsyncEnumerable<Memo> GetItemsAsync()
    {
        var dbContext = new MemoDbContext();
        await foreach (var memo in dbContext.Memos
                           .Include(m => m.Division)
                           .Include(m => m.Department)
                           .OrderByDescending(m => m.CreatedDate)
                           .AsAsyncEnumerable()
                           .Select(Mapper.Map<Memo>))
            yield return memo;

        await dbContext.DisposeAsync();
    }

    public override async Task<ActionResult<Memo>> CreateAsync(Memo item)
    {
        var dbContext = new MemoDbContext();
        var sameId = await dbContext.Memos.FindAsync(KeySelector(item));

        if (sameId != null) return Error("Служебная записка с указанным номером уже существует.");

        var created = await dbContext.Memos.AddAsync(Mapper.Map<MemoDto>(item));
        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(Mapper.Map<Memo>(created.Entity));
    }

    protected override int KeySelector(Memo item) => item.Id;
}