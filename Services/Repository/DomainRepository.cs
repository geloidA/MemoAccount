using AutoMapper;
using MemoAccount.Services.Data;

namespace MemoAccount.Services.Repository;

public abstract class DomainRepository<T, TDto, TKey>: RepositoryBase<T, TKey>
    where T : class
{
    protected readonly MemoDbContext DbContext;
    protected readonly IMapper Mapper;

    protected DomainRepository(IMapper mapper)
    {
        DbContext = new MemoDbContext();
        Mapper = mapper;

        DbContext.Database.EnsureCreated();
    }

    public override void Dispose() => DbContext.Dispose();

    public override async Task<ActionResult<T>> DeleteAsync(T item)
    {
        var foundedObj = await DbContext.FindAsync(typeof(TDto), KeySelector(item));

        if (foundedObj is not TDto toRemove) return NotFound();

        DbContext.Remove(toRemove);
        await DbContext.SaveChangesAsync();
        return Success(Mapper.Map<T>(toRemove));
    }

    public override async Task<ActionResult<T>> UpdateAsync(T item)
    {
        var foundedObj = await DbContext.FindAsync(typeof(TDto), KeySelector(item));

        if (foundedObj is not TDto) return NotFound();

        var entity = DbContext.Update(Mapper.Map<TDto>(item)!);
        await DbContext.SaveChangesAsync();

        return Success(Mapper.Map<T>(entity.Entity));
    }

    public override async Task<ActionResult<T>> GetItemAsync(TKey id)
    {
        var item = await DbContext.FindAsync(typeof(TDto), id);
        return item == null ? NotFound() : Success(Mapper.Map<T>(item));
    }
}