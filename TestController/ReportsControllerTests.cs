using Application.DTO;
using Application.Interfaces;
using DemoDotNetCoreBackend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestController;

public class ReportsControllerTests
{
    
    private readonly Mock<IImportStudentsService> _mockImportStudentsService;
    private readonly Mock<IImportShiftService> _mockImportShiftService;
    private readonly Mock<ICalculatedSalaryBill> _mockCalculatedSalaryBill;
    private readonly Mock<ICalculatedHoursByHouse> _mockCalculatedHoursByHouse;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        _mockImportStudentsService = new Mock<IImportStudentsService>();
        _mockImportShiftService = new Mock<IImportShiftService>();
        _mockCalculatedSalaryBill = new Mock<ICalculatedSalaryBill>();
        _mockCalculatedHoursByHouse = new Mock<ICalculatedHoursByHouse>();

        _controller = new ReportsController(
            _mockImportStudentsService.Object,
            _mockImportShiftService.Object,
            _mockCalculatedSalaryBill.Object,
            _mockCalculatedHoursByHouse.Object
        );
    }
    [Fact]
    public async Task ReportSalary_ReturnsSalaryReport()
    {
        // Arrange
        var sampleSalaryReports = new List<SalaryDto>
        {
            new SalaryDto { Year = 2024, Month = 12, Salary = 5000m },
            new SalaryDto { Year = 2025, Month = 1, Salary = 5500m }
        };

        _mockCalculatedSalaryBill
            .Setup(service => service.FetchMonthlySalary())
            .ReturnsAsync(sampleSalaryReports);

        // Act
        var result = await _controller.ReportSalary();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<SalaryDto>>(okResult.Value);
        Assert.Collection(returnValue,
            item =>
            {
                Assert.Equal(2024, item.Year);
                Assert.Equal(12, item.Month);
                Assert.Equal(5000m, item.Salary);
            },
            item =>
            {
                Assert.Equal(2025, item.Year);
                Assert.Equal(1, item.Month);
                Assert.Equal(5500m, item.Salary);
            });
    }
    
    [Fact]
        public async Task ReportSalary_ServiceReturnsEmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var emptySalaryReports = new List<SalaryDto>();

            _mockCalculatedSalaryBill
                .Setup(service => service.FetchMonthlySalary())
                .ReturnsAsync(emptySalaryReports);

            // Act
            var result = await _controller.ReportSalary();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<SalaryDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }


        #region ReportBill Tests

        [Fact]
        public async Task ReportBill_ReturnsBillReport()
        {
            // Arrange
            var sampleBillReports = new List<BillDto>
            {
                new BillDto { Year = 2024, Month = 12, Bill = 7000m },
                new BillDto { Year = 2025, Month = 1, Bill = 7500m }
            };

            _mockCalculatedSalaryBill
                .Setup(service => service.FetchMonthlyBill())
                .ReturnsAsync(sampleBillReports);

            // Act
            var result = await _controller.ReportBill();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<BillDto>>(okResult.Value);
            Assert.Collection(returnValue,
                item =>
                {
                    Assert.Equal(2024, item.Year);
                    Assert.Equal(12, item.Month);
                    Assert.Equal(7000m, item.Bill);
                },
                item =>
                {
                    Assert.Equal(2025, item.Year);
                    Assert.Equal(1, item.Month);
                    Assert.Equal(7500m, item.Bill);
                });
        }

        [Fact]
        public async Task ReportBill_ServiceReturnsEmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var emptyBillReports = new List<BillDto>();

            _mockCalculatedSalaryBill
                .Setup(service => service.FetchMonthlyBill())
                .ReturnsAsync(emptyBillReports);

            // Act
            var result = await _controller.ReportBill();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<BillDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        #endregion

        #region ReportHoursByHouse Tests

        [Fact]
        public async Task ReportHoursByHouse_WithValidTaskName_ReturnsHouseHours()
        {
            // Arrange
            var taskName = "Cleaning";
            var sampleHouseHours = new List<HouseHoursDto>
            {
                new HouseHoursDto { HouseName = "House A", Hours = 60 },
                new HouseHoursDto { HouseName = "House B", Hours = 60 }
            };

            _mockCalculatedHoursByHouse
                .Setup(service => service.FetchHouseHoursByTask(taskName))
                .ReturnsAsync(sampleHouseHours);

            // Act
            var result = await _controller.ReportHoursByHouse(taskName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<HouseHoursDto>>(okResult.Value);
            Assert.Collection(returnValue,
                item =>
                {
                    Assert.Equal("House A", item.HouseName);
                    Assert.Equal(60.0, item.Hours);
                },
                item =>
                {
                    Assert.Equal("House B", item.HouseName);
                    Assert.Equal(60.0, item.Hours);
                });
        }

    
        [Fact]
        public async Task ReportHoursByHouse_ServiceReturnsEmptyList_ReturnsOkWithEmptyList()
        {
            // Arrange
            var taskName = "Maintenance";
            var emptyHouseHours = new List<HouseHoursDto>();

            _mockCalculatedHoursByHouse
                .Setup(service => service.FetchHouseHoursByTask(taskName))
                .ReturnsAsync(emptyHouseHours);

            // Act
            var result = await _controller.ReportHoursByHouse(taskName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<HouseHoursDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        #endregion
    
}