using Application.DTO;

namespace Application.Interfaces;

public interface ICalculatedSalaryBill
{
    public Task<List<SalaryDto>> FetchMonthlySalary();

    public Task<List<BillDto>> FetchMonthlyBill();

}