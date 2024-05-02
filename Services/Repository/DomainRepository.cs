using AutoMapper;
using MemoAccount.Services.Data;

namespace MemoAccount.Services.Repository;

public abstract class DomainRepository<T, TDto, TKey>: RepositoryBase<T, TKey>
    where T : class
{
    protected readonly IMapper Mapper;

    protected DomainRepository(IMapper mapper)
    {
        var dbContext = new MemoDbContext();
        Mapper = mapper;

        dbContext.Database.EnsureCreated();
        dbContext.Dispose();
    }

    public override async Task<ActionResult<T>> DeleteAsync(T item)
    {
        var dbContext = new MemoDbContext();
        var foundedObj = await dbContext.FindAsync(typeof(TDto), KeySelector(item));

        if (foundedObj is not TDto toRemove) return NotFound();

        dbContext.Remove(toRemove);
        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(Mapper.Map<T>(toRemove));
    }

    public override async Task<ActionResult<T>> UpdateAsync(T item)
    {
        var dbContext = new MemoDbContext();

        dbContext.Update(Mapper.Map<TDto>(item)!);

        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(item);
    }

    public override async Task<ActionResult<T>> GetItemAsync(TKey id)
    {
        var dbContext = new MemoDbContext();
        var item = await dbContext.FindAsync(typeof(TDto), id);
        await dbContext.DisposeAsync();
        return item == null ? NotFound() : Success(Mapper.Map<T>(item));
    }
}