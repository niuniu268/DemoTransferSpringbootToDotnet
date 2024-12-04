namespace Application.DTO;

public abstract class ShiftDto
{
    public string Id { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string UnpaidBreak { get; set; }
    public string BillableRate { get; set; }
    public string AppointedById { get; set; }
}