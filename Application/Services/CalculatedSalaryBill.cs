using Application.DTO;
using Application.Interfaces;
using Core.Entity;
using Core.Interfaces;

namespace Application.Services;

public class CalculatedSalaryBill: ICalculatedSalaryBill
{
    private readonly IShiftRepository _shiftRepository;

    public CalculatedSalaryBill(IShiftRepository shiftRepository)
    {
        _shiftRepository = shiftRepository;
    }
    
    public async Task<List<SalaryDto>> FetchMonthlySalary()
    {
        return await _shiftRepository.AggregateMonthlySalaryAsync();
    }

    public async Task<List<BillDto>> FetchMonthlyBill()
    {
        return await _shiftRepository.AggregateMonthlyBillAsync();
    }
    
}