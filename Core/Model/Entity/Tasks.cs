using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity;

public class Tasks
{
    
    public string Id { get; init; }
    
    public string StudentsId { get; init; }
    
    public Students StudentsBy { get; set; }
    
    public string Task { get; init; }

    public int Weight { get; init; }
    
}