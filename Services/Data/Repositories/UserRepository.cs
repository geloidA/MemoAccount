using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MemoAccount.Services.Data.Repositories;

public class UserRepository(IMapper mapper) : DomainRepository<User, UserDto, int>(mapper)
{
    public override async IAsyncEnumerable<User> GetItemsAsync()
    {
        var dbContext = new MemoDbContext();
        Log.Information("User GetItemsAsync");
        await foreach (var user in dbContext.Users
                           .AsAsyncEnumerable()
                           .Select(Mapper.Map<User>))
        {
            yield return user;
        }

        await dbContext.DisposeAsync();
    }

    public override async Task<ActionResult<User>> CreateAsync(User item)
    {
        var dbContext = new MemoDbContext();
        Log.Information("User CreateAsync");
        var sameLogin = await dbContext.Users.FirstOrDefaultAsync(x => x.Login == item.Login);

        if (sameLogin != null) return Error("Логин уже занят");

        var created = await dbContext.Users.AddAsync(Mapper.Map<UserDto>(item));
        await dbContext.SaveChangesAsync();
        await dbContext.DisposeAsync();
        return Success(Mapper.Map<User>(created.Entity));
    }

    protected override int KeySelector(User item) => item.Id;
}