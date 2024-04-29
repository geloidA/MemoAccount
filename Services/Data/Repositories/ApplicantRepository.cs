using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;
using MemoAccount.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace MemoAccount.Services.Data.Repositories;

public class ApplicantRepository(IMapper mapper) : DomainRepository<Applicant, ApplicantDto, int>(mapper)
{
    public override IAsyncEnumerable<Applicant> GetItemsAsync() => DbContext.Applicants
        .Include(a => a.Division)
        .Include(a => a.Department)
        .AsAsyncEnumerable()
        .Select(Mapper.Map<Applicant>);

    public override async Task<ActionResult<Applicant>> CreateAsync(Applicant item)
    {
        var created = await DbContext.Applicants.AddAsync(Mapper.Map<ApplicantDto>(item));
        return Success(Mapper.Map<Applicant>(created.Entity));
    }

    protected override int KeySelector(Applicant item) => item.Id;
}