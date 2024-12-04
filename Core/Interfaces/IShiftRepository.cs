using Application.DTO;
using Core.Entity;

namespace Core.Interfaces;

public interface IShiftRepository
{
    long CountShifts();
    Task<List<BillDto>> AggregateMonthlyBillAsync();
    Task<List<SalaryDto>> AggregateMonthlySalaryAsync();
    Task SaveShiftAsync(Shift shift);
    Task SaveChangeAsync();
}