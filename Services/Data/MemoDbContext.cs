using MemoAccount.Services.Data.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MemoAccount.Services.Data;

public class MemoDbContext : DbContext
{
    public DbSet<UserDto> Users { get; set; }
    public DbSet<MemoDto> Memos { get; set; }
    public DbSet<DepartmentDto> Departments { get; set; }
    public DbSet<DivisionDto> Divisions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
    }
}