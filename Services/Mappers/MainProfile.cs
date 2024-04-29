using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data.Dtos;

namespace MemoAccount.Services.Mappers;

public class MainProfile : Profile
{
    public MainProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<Memo, MemoDto>().ReverseMap();
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<Division, DivisionDto>().ReverseMap();
        CreateMap<Applicant, ApplicantDto>().ReverseMap();
    }   
}