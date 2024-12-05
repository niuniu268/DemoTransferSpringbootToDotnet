namespace Application.Interfaces;

public interface IImportStudentsService
{
    public Task ImportStudentsFromCSV(string file);
}