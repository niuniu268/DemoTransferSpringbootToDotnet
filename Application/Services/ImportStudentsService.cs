using System.Globalization;
using Application.DTO;
using Application.Interfaces;
using Core.Entity;
using Core.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System.Linq;

namespace Application.Services
{
    public class ImportStudentsService : IImportStudentsService
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITasksRepository _tasksRepository;

        public ImportStudentsService(IStudentsRepository studentsRepository, 
                                      ITasksRepository tasksRepository)
        {
            _studentsRepository = studentsRepository;
            _tasksRepository = tasksRepository;
        }

        public void ImportStudentsFromCSV(string file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,  // Ignore header validation
                MissingFieldFound = null // Ignore missing fields
            };

            try
            {
                using var reader = new StreamReader(file);
                using var csv = new CsvReader(reader, config);
                
                // Register ClassMap to handle custom mapping of CSV headers
                csv.Context.RegisterClassMap<StudentsMap>();

                var records = csv.GetRecords<StudentsDto>().ToList();

                foreach (var record in records.Where(r => !string.IsNullOrEmpty(r.Id) && !string.IsNullOrEmpty(r.SalaryRate) && !string.IsNullOrEmpty(r.House)))
                {
                    try
                    {
                        var student = CreateStudentEntity(record);
                        _studentsRepository.SaveStudentsAsync(student);

                        if (string.IsNullOrEmpty(record.Tasks)) continue;

                        var tasks = record.Tasks.Split(';')
                                                 .Select(t => t.Trim())
                                                 .Where(t => !string.IsNullOrEmpty(t))
                                                 .ToList();
                        int weight = CalculateWeight(tasks.Count);

                        foreach (var newTask in tasks.Select(task => CreateTaskEntity(task, student, weight)))
                        {
                            _tasksRepository.SaveTasksAsync(newTask);
                        }

                        _tasksRepository.SaveChangeAsync();
                        _studentsRepository.SaveChangeAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving student {record.Id}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to import students from CSV", ex);
            }
        }

        private static Students CreateStudentEntity(StudentsDto studentsDto)
        {
            return new Students
            {
                Id = studentsDto.Id,
                SalaryRate = int.Parse(studentsDto.SalaryRate),
                ExtraSalary = string.IsNullOrEmpty(studentsDto.ExtraSalary) ? null : (int?)int.Parse(studentsDto.ExtraSalary),
                House = studentsDto.House,
                Tasks = studentsDto.Tasks
            };
        }

        private static Tasks CreateTaskEntity(string task, Students student, int weight)
        {
            return new Tasks
            {
                Id = Guid.NewGuid().ToString(),
                StudentsId = student.Id,
                Task = task,
                Weight = weight
            };
        }

        private static int CalculateWeight(int taskCount)
        {
            return taskCount switch
            {
                3 => 33,
                2 => 50,
                1 => 100,
                _ => 0
            };
        }
    }

    // ClassMap for StudentsDto to map CSV headers to DTO properties
    public class StudentsMap : ClassMap<StudentsDto>
    {
        public StudentsMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.SalaryRate).Name("salary_rate");
            Map(m => m.ExtraSalary).Name("after_17_extra_salary_rate");
            Map(m => m.House).Name("house");
            Map(m => m.Tasks).Name("tasks");
        }
    }
}
