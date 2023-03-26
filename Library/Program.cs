using Library;
using Library.Application.UserServ;
using Library.Data.Domein.Domein;
using Library.Data.Domein.Domein.EntityModelBuilders;
using Library.DataBase.GeneralRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));


    //validator
    builder.Services.AddValidators();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(builder.Configuration
                .GetSection("AppSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseMiddleware(typeof(GlobalExceptionHandling));

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
