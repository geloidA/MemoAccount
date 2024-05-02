using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;

namespace MemoAccount.Services.Mappers;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<Memo, MemoDto>()
            .ForMember(d => d.DepartmentId, opt => opt.MapFrom(s => s.Department.Id))
            .ForMember(d => d.DivisionId, opt => opt.MapFrom<int?>(s =>
                s.Division.Id == 0 ? null : s.Division.Id))
            .ForMember(d => d.Division, opt => opt.MapFrom(s => s.Division == null
                ? null
                : new DivisionDto
                {
                    Name = s.Division.Name,
                    DepartmentId = s.Division.Department.Id,
                    Department = null!
                }))
            .ForMember(d => d.Department, opt => opt.MapFrom(s =>
                new DepartmentDto
                {
                    Name = s.Department.Name
                }))
            .ReverseMap()
            .ForMember(d => d.Division, opt => opt.MapFrom(s => s.Division == null
                ? null
                : new Division
                {
                    Id = s.Division.Id,
                    Name = s.Division!.Name,
                    Department = new Department
                    {
                        Id = s.DepartmentId,
                        Name = s.Department.Name,
                    }
                }));

        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<Division, DivisionDto>().ReverseMap();
    }
}