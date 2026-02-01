using Medical_Team_B.Extensions;
using Medical_Team_B.Middlewares;
using MedLink.Domain.Interfaces.Repositories;
using MedLink.Infrastructure.Persistence.Context;
using MedLink.Infrastructure.Persistence.Seed;
using MedLink.Infrastructure.Repositories;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Mappers;
using MedLink_Application.Queries;
using MedLink_Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddAutoMapper(typeof(AppointmentProfile).Assembly);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
       Assembly.GetExecutingAssembly(),
       Assembly.GetAssembly(typeof(GetAppointmentByIdQuery))));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDoctorAvailabilityRepository, DoctorAvailabilityRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IStripeService, StripeService>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(Options =>
{
    Options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MediLink",
        Version = "v1",
        Description = "API for MedLink Appointments and Payments",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Ahmed Selim",
            Email = "ahkdddd555@gmail.com",
            Url = new Uri("http://YourWebSit.eg ")
        }
    });
});
builder.Services.AddOpenApi();

var app = builder.Build();


/// Used to implememt the migration automateclly after running the project  - instead of Update-Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
        await ApplicationDbContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred during migration");
    }
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
  
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MediLink API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
