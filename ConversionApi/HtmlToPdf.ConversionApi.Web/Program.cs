using HtmlToPdf.Common.Broker.DI;
using HtmlToPdf.ConversionApi.Broker.Producing.DI;
using HtmlToPdfService.ConversionApi.Data.AppDatabase.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCommonBrokerServices();
builder.Services.AddBrokerProducingServices();
builder.Services.AddAppDatabaseServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_MyAllowSubdomainPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

app.Run();