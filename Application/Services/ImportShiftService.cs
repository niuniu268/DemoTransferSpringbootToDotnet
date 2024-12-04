using System.Globalization;
using Application.DTO;
using Application.Interfaces;
using Core.Entity;
using Core.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace Application.Services
{
    public class ImportShiftService: IImportShiftService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IStudentsRepository _studentsRepository;

        public ImportShiftService(IShiftRepository shiftRepository, IStudentsRepository studentsRepository)
        {
            _shiftRepository = shiftRepository;
            _studentsRepository = studentsRepository;
        }
        
        public void ImportShiftFromCSV(string file)
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
                
                // Register ClassMap to ensure correct header-to-property mapping
                csv.Context.RegisterClassMap<ShiftMap>();

                var shiftCount = _shiftRepository.CountShifts();
                for (var i = 0; i <= shiftCount; i++)
                {
                    csv.Read(); // Skip rows if necessary (check if this loop is needed)
                }
                
                // Read records
                var records = csv.GetRecords<ShiftDto>().ToList();
                
                // Process each shift record
                foreach (var shift in records.Select(GetShift))
                {
                    _shiftRepository.SaveShiftAsync(shift);
                }

                _shiftRepository.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private Shift GetShift(ShiftDto record)
        {
            var shift = new Shift
            {
                Id = record.Id,
                StartTime = DateTime.Parse(record.StartTime),
                EndTime = DateTime.Parse(record.EndTime),
                UnpaidBreak = int.Parse(record.UnpaidBreak),
                BillableRate = int.Parse(record.BillableRate),
            };

            if (!string.IsNullOrEmpty(record.AppointedById))
            {
                var appointedBy = _studentsRepository.FindById(record.AppointedById);
                shift.AppointedBy = appointedBy;
            }

            return shift;
        }
    }

    // Create ClassMap for correct mapping of CSV headers to DTO properties
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
