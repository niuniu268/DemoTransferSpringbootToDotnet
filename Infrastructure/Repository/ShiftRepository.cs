using Application.DTO;
using Core.Entity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ShiftRepository: IShiftRepository
{
    private readonly HogwartsContext _context;

    public ShiftRepository(HogwartsContext context)
    {
        _context = context;
    }
    
    public long CountShifts()
    {
        return _context.Shift.LongCount();
    }

    public async Task<List<BillDto>> AggregateMonthlyBillAsync()
    {
        var query = await _context.Shift
            .GroupBy(s => new { Year = s.EndTime.Year, Month = s.EndTime.Month }) 
            .Select(g => new BillDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Bill = g.Sum(s => (EF.Functions.DateDiffMinute(s.StartTime, s.EndTime) - s.UnpaidBreak) * s.BillableRate)
            })
            .OrderBy(b => b.Year)
            .ThenBy(b => b.Month)
            .ToListAsync();

        return query;
    }

    public async Task<List<SalaryDto>> AggregateMonthlySalaryAsync()
    {
        var salaryList = await _context.Shift
            .Join(_context.Students, s => s.AppointedById, st => st.Id, (s, st) => new { s, st })
            .GroupBy(x => new { Year = x.s.EndTime.Year, Month = x.s.EndTime.Month })
            .Select(g => new SalaryDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Salary = g.Sum(x => 
                    (EF.Functions.DateDiffMinute(x.s.StartTime, x.s.EndTime) * x.st.SalaryRate / 60))
            })
            .OrderBy(s => s.Year)
            .ThenBy(s => s.Month)
            .ToListAsync();

        Console.WriteLine(salaryList);
        return salaryList;
    }

    public async Task SaveShiftAsync(Shift shift)
    {
        if (shift.AppointedById != null)
        {
            var student = await _context.Students.FindAsync(shift.AppointedById);
            if (student == null)
            {
                
                return;

            }
        }

        await _context.Shift.AddAsync(shift);
    }
    
    
    public async Task SaveChangeAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Database update error: {ex.Message}");
            
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
            
            throw;
        }
    }
}
