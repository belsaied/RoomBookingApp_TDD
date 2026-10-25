
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure services (equivalent to ConfigureServices in .NET 5)
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RoomBookingApp.Api",
        Version = "v1"
    });
});

// 2. Configure the SQLite in-memory database
var connString = "Filename=:memory:";
var conn = new SqliteConnection(connString);
conn.Open();

builder.Services.AddDbContext<RoomBookingAppDbContext>(
    options => options.UseSqlite(conn));

EnsureDatabaseCreated(conn);

// 3. Register application services
builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();
builder.Services.AddScoped<IRoomBookingRequestProcessor,
    RoomBookingRequestProcessor>();

var app = builder.Build();

// 4. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "RoomBookingApp.Api v1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

// 5. Create the database
static void EnsureDatabaseCreated(SqliteConnection conn)
{
    var options = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
    options.UseSqlite(conn);

    using var context = new RoomBookingAppDbContext(options.Options);
    context.Database.EnsureCreated();
}
