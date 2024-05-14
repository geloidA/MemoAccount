using MemoAccount.Services.Data.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Serilog;

namespace MemoAccount.Services.Data;

public class MemoDbContext : DbContext
{
    public DbSet<UserDto> Users { get; set; }
    public DbSet<MemoDto> Memos { get; set; }
    public DbSet<DepartmentDto> Departments { get; set; }
    public DbSet<DivisionDto> Divisions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Log.Information("MemoDbContext OnConfiguring");
        try
        {            
            optionsBuilder
                .UseSqlServer(ConfigurationManager.ConnectionStrings["Database"].ConnectionString)
                .LogTo(Log.Information);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
        Log.Information("MemoDbContext Configured");
    }
}