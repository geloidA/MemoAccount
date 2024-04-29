using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoAccount.Services.Data.Repositories;

public class UserRepository(IMapper mapper) : DomainRepository<User, UserDto, int>(mapper)
{
    public override IAsyncEnumerable<User> GetItemsAsync() => DbContext.Users
            .AsAsyncEnumerable()
            .Select(Mapper.Map<User>);

    public override async Task<ActionResult<User>> CreateAsync(User item)
    {
        var sameLogin = await DbContext.Users.FirstOrDefaultAsync(x => x.Login == item.Login);

        if (sameLogin != null) return Error("Логин уже занят");

        var created = await DbContext.Users.AddAsync(Mapper.Map<UserDto>(item));
        await DbContext.SaveChangesAsync();

        return Success(Mapper.Map<User>(created.Entity));
    }

    protected override int KeySelector(User item) => item.Id;
}