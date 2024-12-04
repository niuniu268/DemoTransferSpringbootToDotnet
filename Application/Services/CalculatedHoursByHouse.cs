using Application.DTO;
using Application.Interfaces;
using Core.Interfaces;

namespace Application.Services;

public class CalculatedHoursByHouse: ICalculatedHoursByHouse
{
    
    private readonly IStudentsRepository _studentsRepository;

    public CalculatedHoursByHouse(IStudentsRepository studentsRepository)
    {
        _studentsRepository = studentsRepository;
    } 
    
    public async Task<List<HouseHoursDto>> FetchHouseHoursByTask(string taskName)
    {
        return await _studentsRepository.AggregateHoursByHouseForTaskAsync(taskName);
    }
}