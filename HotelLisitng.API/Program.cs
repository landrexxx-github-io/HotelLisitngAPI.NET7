using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connStrings = builder.Configuration.GetConnectionString("HotelListingDBConnectionString");


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy("ALLOW_ALL", 
        policy => policy.AllowAnyHeader()
                        .AllowAnyOrigin()
                        .AllowAnyMethod());
});

builder.Host.UseSerilog((ctx, config) => config.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

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
