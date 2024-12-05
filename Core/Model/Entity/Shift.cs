using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity;

public class Shift
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int UnpaidBreak { get; set; }
    public int BillableRate { get; set; }

    public string AppointedById { get; set; }
    
    public Students AppointedBy { get; set; }
}
