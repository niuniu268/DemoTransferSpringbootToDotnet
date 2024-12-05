namespace Application.Interfaces;

public interface IImportShiftService
{
    public Task ImportShiftFromCSV(string file);
}