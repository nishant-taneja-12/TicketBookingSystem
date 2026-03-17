using System;
using System.Reflection;
using BookingHub.Infrastructure.Data;
using BookingHub.Infrastructure.Repositories;
using BookingHub.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using MediatR;
using BookingHub.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingHub API", Version = "v1" });
    var xmlFile = ($"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<BookingHub.Infrastructure.SwaggerExamples.CreateBookingRequestExample>();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=bookings.db";
builder.Services.AddDbContext<BookingDbContext>(options => options.UseSqlite(connectionString));

// Infrastructure registrations
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

// Register MediatR handlers from application assembly (compatible with MediatR v11)
builder.Services.AddMediatR(typeof(CreateBookingHandler).Assembly);

var app = builder.Build();

// Apply migrations/EnsureCreated and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    try { db.Database.Migrate(); } catch { db.Database.EnsureCreated(); }
    if (!db.Bookings.Any())
    {
        var sample1 = BookingHub.Domain.Entities.Booking.Create(DateTime.UtcNow.AddDays(1).Date.AddHours(10), 2);
        var sample2 = BookingHub.Domain.Entities.Booking.Create(DateTime.UtcNow.AddDays(2).Date.AddHours(14), 4);
        db.Bookings.AddRange(sample1, sample2);
        db.SaveChanges();
    }
}

// Always enable Swagger UI (available at /swagger)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
