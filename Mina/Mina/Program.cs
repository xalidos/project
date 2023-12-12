using Mina.Entities;
using Mina.Repositories;
using Mina.Services;
using System;
using Microsoft.EntityFrameworkCore;
using Mina.DbContexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Mina.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinaDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PGDatabase"), o => o.UseNetTopologySuite());
});

NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
//builder.Services.AddScoped<IPoiRepository, PoiRepository>();
builder.Services.AddScoped<IBuildingService, BuildingService>();

builder.Services.AddControllers(options =>
{
    // Prevent the following exception: 'This method does not     support GeometryCollection arguments' 
    // See: https://github.com/npgsql/Npgsql.EntityFrameworkCore.PostgreSQL/issues/585 
    options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Point)));
    options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(Coordinate)));
    options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(LineString)));
    options.ModelMetadataDetailsProviders.Add(new SuppressChildValidationMetadataProvider(typeof(MultiLineString)));
}).AddNewtonsoftJson(options =>
{
    foreach (var converter in GeoJsonSerializer.Create(new GeometryFactory(new PrecisionModel(), 4326)).Converters)
    {
        options.SerializerSettings.Converters.Add(converter);
    }
}).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

//builder.Services.AddControllers(options =>
//{
//    options.InputFormatters.Insert(0, new GeoJsonInputFormatter());
//});

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
