using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.File("log/villalogs.text",rollingInterval:RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();*/
builder.Services.AddDbContext<ApplicationDbContext> (option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defaultSQLConnection"));
});
builder.Services.AddControllers(Options =>
{
    Options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogging, Logging>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
