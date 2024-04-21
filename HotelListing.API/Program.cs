using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("HotelListingDBConnectionString");

builder.Services.AddDbContext<HoteListingDbContext>(options => {
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// This will allow all user to access the API
builder.Services.AddCors(options => {
    options.AddPolicy("ALLOW_ALL"
        , policy => policy.AllowAnyHeader()
                          .AllowAnyOrigin()
                          .AllowAnyMethod());
});

// Initialize logging
builder.Host.UseSerilog((context, config) => config.WriteTo.Console().ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("ALLOW_ALL");

app.UseAuthorization();

app.MapControllers();

app.Run();
