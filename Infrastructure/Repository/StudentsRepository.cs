using System.Collections.Immutable;
using Application.DTO;
using Core.Entity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class StudentsRepository:IStudentsRepository
{
    private readonly HogwartsContext _context;
    
    public StudentsRepository(HogwartsContext context)
    {
        _context = context;
    }
    
    public long CountStudents()
    {
        return _context.Students.Count();
    }

    public async Task<List<HouseHoursDto>> AggregateHoursByHouseForTaskAsync(string taskName)
    {
        var List = await _context.Shift
            .Join(_context.Students, s=> s.AppointedById, st => st.Id, (s,st) => new{s, st})
            .GroupBy(g=> new {name = g.st.House}).Select(g => new HouseHoursDto()
            {
                HouseName = g.Key.name,
                Hours = g.Sum(x => 
                    (EF.Functions.DateDiffMinute(x.s.StartTime, x.s.EndTime) * x.st.SalaryRate / 60))
            })
            .OrderBy(s => s.HouseName)
            .ToListAsync();

        return List;


    }
    
    public async Task SaveStudentsAsync(Students students)
    {
        await _context.Students.AddAsync(students);
        
    }

    public Students FindById(string id)
    {
        return _context.Students.Find(id);
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
    
}