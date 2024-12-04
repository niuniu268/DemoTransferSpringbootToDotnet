using Application.Interfaces;
using Application.Services;
using Core.Interfaces;
using DemoDotNetCoreBackend.Controllers;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

// var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();
logger.Debug("init main");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<LogActionAttribute>();

builder.Services.AddDbContext<HogwartsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();

builder.Services.AddScoped<ICalculatedHoursByHouse, CalculatedHoursByHouse>();
builder.Services.AddScoped<ICalculatedSalaryBill, CalculatedSalaryBill>();
builder.Services.AddScoped<IImportShiftService, ImportShiftService>();
builder.Services.AddScoped<IImportStudentsService, ImportStudentsService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();