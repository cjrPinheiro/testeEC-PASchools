using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PASchools.Application.Interfaces;
using PASchools.Application.Services;
using PASchools.Domain;
using PASchools.Google.Connector;
using PASchools.Google.Connector.Interfaces;
using PASchools.Persistence;
using PASchools.Persistence.Interfaces;
using PASchools.Persistence.Repositories;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PASchoolsContext>(
             context => context.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
         );



builder.Services.AddScoped<ISchoolPersist, SchoolPersist>();
builder.Services.AddScoped<IGoogleApiClient, GoogleApiClient>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
//builder.Services.AddHttpClient<IGoogleApiClient, GoogleApiClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
             .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
             .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddCors();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowAnyOrigin());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
