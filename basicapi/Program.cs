using Microsoft.EntityFrameworkCore;
using basicapi.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BasicDbContext>(
    opts =>
    {
        opts.EnableSensitiveDataLogging();
        opts.EnableDetailedErrors();
        opts.UseNpgsql(builder.Configuration.GetConnectionString("postgresql_db"));
    }, ServiceLifetime.Transient
);

var app = builder.Build();

app.MapGet("/hello", () => "Hello World!");

app.MapGet("/data", async (BasicDbContext db) => {

    // all data to list
    var data = await db.RequestBodyModel.Select(x => x).ToListAsync();

    return Results.Ok(data);

});

app.Run();
