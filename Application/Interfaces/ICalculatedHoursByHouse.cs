using Application.DTO;

namespace Application.Interfaces;

public interface ICalculatedHoursByHouse
{
    public Task<List<HouseHoursDto>> FetchHouseHoursByTask(string taskName);
}