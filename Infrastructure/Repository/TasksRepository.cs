using Core.Entity;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class TasksRepository: ITasksRepository
{
    private readonly HogwartsContext _context;

    public TasksRepository(HogwartsContext context)
    {
        _context = context;
    }
    
    public long CountTasks()
    {
        return  _context.Tasks.Count();
    }

    public async Task SaveTasksAsync(Tasks tasks)
    {
        await _context.Tasks.AddAsync(tasks);
    }
    
    
    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }
}