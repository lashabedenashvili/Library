using Library.Data.Domein.Domein;
using Library.Data.Domein.Domein.EntityModelBuilders;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System.Text.Json.Serialization;

var logger = NLog.LogManager
    .Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
var builder = WebApplication.CreateBuilder(args);

    // log youe application at trace level 
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    // Register the NLog service
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();


    builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

    // Add services to the container.
    builder.Services.AddDbContext<Context>(options => options
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IContext, Context>();

//validator
builder.Services.AddValidators();

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

}
catch (Exception ex)
{

    logger.Error(ex);
    throw;
}
finally
{
    // Ensure to shout downon the NLog ( Disposing )
    NLog.LogManager.Shutdown();
}
