using Core.Entity;

namespace Core.Interfaces;

public interface ITasksRepository
{
    long CountTasks();

    Task SaveTasksAsync(Tasks tasks);
    
    Task SaveChangeAsync();
}