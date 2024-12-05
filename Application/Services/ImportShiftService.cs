using System.Globalization;
using Application.DTO;
using Application.Interfaces;
using Core.Entity;
using Core.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace Application.Services
{
    public class ImportShiftService : IImportShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IStudentsRepository _studentsRepository;

        public ImportShiftService(IShiftRepository shiftRepository, IStudentsRepository studentsRepository)
        {
            _shiftRepository = shiftRepository;
            _studentsRepository = studentsRepository;
        }

        public async Task ImportShiftFromCSV(string file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,
                MissingFieldFound = null,
            };

            try
            {
                using var reader = new StreamReader(file);
                using var csv = new CsvReader(reader, config);

                csv.Context.RegisterClassMap<ShiftMap>();

                var records = csv.GetRecords<ShiftDto>().ToList();

                foreach (var record in records.Where(r => !string.IsNullOrEmpty(r.Id)))
                {
                    try
                    {
                        var shiftelement = await GetShift(record);
                        if (shiftelement == null)
                        {
                            continue; 
                        }

                        await _shiftRepository.SaveShiftAsync(shiftelement);
                    }
                    catch (Exception readEx)
                    {
                        var rawRecord = csv.Context.Parser?.RawRecord;
                        Console.WriteLine($"Error parsing record: {readEx.Message}");
                        Console.WriteLine($"Raw Record: {rawRecord}");
                        Console.WriteLine($"Stack Trace: {readEx.StackTrace}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }

            await _shiftRepository.SaveChangeAsync();
        }

        private async Task<Shift> GetShift(ShiftDto record)
        {
            Shift shift = new Shift
            {
                Id = record.Id,
                StartTime = DateTime.ParseExact(record.StartTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                EndTime = DateTime.ParseExact(record.EndTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
                UnpaidBreak = int.TryParse(record.UnpaidBreak, out var unpaidBreak) ? unpaidBreak : 0,
                BillableRate = int.TryParse(record.BillableRate, out var billableRate) ? billableRate : 0,
                AppointedById = record.AppointedById,
            };

            if (!string.IsNullOrEmpty(record.AppointedById))
            {
                var appointedBy = await _studentsRepository.FindByIdAsync(record.AppointedById);
                if (appointedBy == null)
                {
                    Console.WriteLine($"Warning: Appointed student ID {record.AppointedById} does not exist. Setting 'AppointedBy' to NULL.");
                    return null;
                }
                else
                {
                    shift.AppointedBy = appointedBy;
                }
            }


            return shift;
        }

        public class ShiftMap : ClassMap<ShiftDto>
        {
            public ShiftMap()
            {
                Map(m => m.Id).Name("id");
                Map(m => m.StartTime).Name("start_time");
                Map(m => m.EndTime).Name("end_time");
                Map(m => m.UnpaidBreak).Name("unpaid_break");
                Map(m => m.BillableRate).Name("billable_rate");
                Map(m => m.AppointedById).Name("appointed_by");
            }
        }
    }
}
