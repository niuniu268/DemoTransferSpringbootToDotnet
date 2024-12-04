using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity;

public class Shift
{
    public string Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public int UnpaidBreak { get; init; }
    public int BillableRate { get; init; }

    public string AppointedById { get; init; }
    
    public Students AppointedBy { get; set; }
}
