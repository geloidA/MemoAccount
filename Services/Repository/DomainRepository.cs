﻿using System.IO;
using AutoMapper;
using MemoAccount.Models;
using MemoAccount.Services.Data;
using MemoAccount.Services.Data.Dtos;
using Newtonsoft.Json;

namespace MemoAccount.Services.Repository;

public abstract class DomainRepository<T, TDto, TKey>: RepositoryBase<T, TKey>
    where T : class
{
    protected readonly IMapper Mapper;

    protected DomainRepository(IMapper mapper)
    {
        Mapper = mapper;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var dbContext = new MemoDbContext();
        // Проверяем, создана ли база данных
        if (!dbContext.Database.EnsureCreated())
        {
            return; // База данных уже существует, не нужно ничего загружать
        }

        // Десериализуем JSON в объекты
        var data = JsonConvert.DeserializeObject<DatabaseData>(File.ReadAllText("default-db-data.json"));

        // Добавляем отделы и подразделения
        dbContext.Divisions!.AddRange(data!.Departments!.SelectMany(x => x.Divisions!));
        dbContext.Departments.AddRange(data!.Departments!);

        dbContext.SaveChanges();

        // Добавляем записки
        dbContext.Memos.AddRange(data.Memos!);

        dbContext.SaveChanges();
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

public class DatabaseData
{
    public List<DepartmentDto>? Departments { get; set; }
    public List<MemoDto>? Memos { get; set; }
}