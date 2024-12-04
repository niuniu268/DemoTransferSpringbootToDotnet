using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity;

public class Students: IAggregateRoot
{
    
    public string Id { get; init; }
    
    public int SalaryRate { get; init; }
    
    public int? ExtraSalary { get; init; }
    
    public string House { get; init; } 

    public string Tasks { get; init; }
    
    public ICollection<Shift> ShiftSet { get; init; } = new List<Shift>();

    public ICollection<Tasks> TasksSet { get; init; } = new List<Tasks>();
}