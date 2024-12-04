using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoDotNetCoreBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IImportStudentsService _importStudentsService;
    private readonly IImportShiftService _importShiftService;
    private readonly ICalculatedSalaryBill _calculatedSalaryBill;
    private readonly ICalculatedHoursByHouse _calculatedHoursByHouse;
    
    public ReportsController(
        IImportStudentsService importStudentsService,
        IImportShiftService importShiftService,
        ICalculatedSalaryBill calculatedSalaryBill,
        ICalculatedHoursByHouse calculatedHoursByHouse)
    {
        _importStudentsService = importStudentsService;
        _importShiftService = importShiftService;
        _calculatedSalaryBill = calculatedSalaryBill;
        _calculatedHoursByHouse = calculatedHoursByHouse;
    }
    
    [HttpPost("upload")]
    public IActionResult UploadStudents([FromForm] string studentsCsv, [FromForm] string shiftsCsv)
    {
        Console.WriteLine(studentsCsv);
        Console.WriteLine(shiftsCsv);
        
        _importStudentsService.ImportStudentsFromCSV(studentsCsv);
        _importShiftService.ImportShiftFromCSV(shiftsCsv);
    
        return Ok();
    }
    
    [HttpGet("show/salary")]
    public async Task<IActionResult> ReportSalary()
    {
        var result = await _calculatedSalaryBill.FetchMonthlySalary();
        return Ok(result);
    }
    
    [HttpGet("show/bill")]
    public async Task<IActionResult> ReportBill()
    {
        var result = await _calculatedSalaryBill.FetchMonthlyBill();
        return Ok(result);
    }
    
    [HttpGet("show/house")]
    public async Task<IActionResult> ReportHoursByHouse([FromQuery] string taskName)
    {
        var result = await _calculatedHoursByHouse.FetchHouseHoursByTask(taskName);
        return Ok(result);
    }
}
