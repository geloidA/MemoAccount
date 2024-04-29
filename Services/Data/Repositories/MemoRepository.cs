using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoAccount.Services.Data.Repositories;

public class MemoRepository(IMapper mapper) : DomainRepository<Memo, MemoDto, int>(mapper)
{
    public override IAsyncEnumerable<Memo> GetItemsAsync() => DbContext.Memos
        .Include(m => m.Applicant)
        .Include(m => m.Applicant.Division)
        .Include(m => m.Applicant.Department)
        .AsAsyncEnumerable()
        .Select(Mapper.Map<Memo>);

    public override async Task<ActionResult<Memo>> CreateAsync(Memo item)
    {
        var sameId = await DbContext.Memos.FindAsync(KeySelector(item));

        if (sameId != null) return Error("Служебная записка с указанным номером уже существует.");

        var created = await DbContext.Memos.AddAsync(Mapper.Map<MemoDto>(item));
        return Success(Mapper.Map<Memo>(created.Entity));
    }

    protected override int KeySelector(Memo item) => item.Id;
}