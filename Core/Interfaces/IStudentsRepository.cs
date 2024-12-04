using Application.DTO;
using Core.Entity;

namespace Core.Interfaces;

public interface IStudentsRepository
{
    long CountStudents();
    Task<List<HouseHoursDto>> AggregateHoursByHouseForTaskAsync(string taskName);
    Task SaveStudentsAsync(Students students);
    Students FindById(string id);
    Task SaveChangeAsync();
}